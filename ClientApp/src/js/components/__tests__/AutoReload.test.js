import React from 'react';
import renderer from 'react-test-renderer';
import AutoReload from '../AutoReload';
import localStorageHelper from '../../lib/localStorageHelper';

jest.mock('../../lib/localStorageHelper');

const fakeProps = {
    dataKey: 'test',
    defaultCountdown: 1000,
    reloadCallback: jest.fn()
};

function findElement(component, selector) {
    if (component._component) {
        return findElement(component._component, selector);
    }

    if (component._currentElement) {
        const element = component._currentElement;

        if (element.props && element.props.className) {
            const reg = new RegExp(selector.substring(1));
            if (component._currentElement.props.className.match(reg)) {
                return element;
            }
        }
    }

    if (component._renderedComponent) {
        return findElement(component._renderedComponent, selector);
    }

    if (component._renderedChildren) {
        for (let key in component._renderedChildren) {
            let found = findElement(component._renderedChildren[key], selector);
            if (found) {
                return found;
            }
        }
    }

    return null;
}

describe('render corectly:', () => {
    const _setInterval = window.setInterval;
    const _clearInterval = window.clearInterval;

    beforeEach(() => {
        window.setInterval = jest.fn();
        window.clearInterval = jest.fn();
        localStorageHelper.getWidgetReloadCountdown = jest.fn(() => ({}));
    });

    afterEach(() => {
        window.setInterval = _setInterval;
        window.clearInterval = _clearInterval;
    });

    test('without user settings: use default countdown', () => {
        const component = renderer.create(<AutoReload {...fakeProps} />);
        expect(component.toJSON()).toMatchSnapshot();
    });

    test('load countdown from user settings', () => {
        localStorageHelper.getWidgetReloadCountdown = jest.fn(() => ({ test: 100 }));
        const component = renderer.create(<AutoReload {...fakeProps} />);
        expect(component.toJSON()).toMatchSnapshot();
    });

    test('on timer tick: decrease countdown by 1', () => {
        const component = renderer.create(<AutoReload {...fakeProps} />);
        const instance = component.getInstance();
        instance._onTick();
        expect(component.toJSON()).toMatchSnapshot();
    });

    [null, undefined, []].forEach(countdownValues => {
        test('on counter click: show empty dropdown', () => {
            const props = Object.assign({
                countdownValues: countdownValues
            }, fakeProps);
            const component = renderer.create(<AutoReload {...props} />);
            const element = findElement(component, '.btn-refresh');

            component.getInstance().setState({ isShowSelection: false });

            element.props.onClick();
            expect(component.toJSON()).toMatchSnapshot();
        });
    });

    test('component is loading: disable countdown', () => {
        const props = Object.assign({ isLoading: true }, fakeProps);
        const component = renderer.create(<AutoReload {...props} />);

        expect(component.toJSON()).toMatchSnapshot();
    });
});

describe('_onCounterClick()', () => {
    test('dropdown is hidden: show dropdown', () => {
        const props = Object.assign({
            countdownValues: [1, 2, 3, 4]
        }, fakeProps);
        const component = renderer.create(<AutoReload {...props} />);
        const element = findElement(component, 'btn-refresh');

        component.getInstance().setState({ isShowSelection: false });

        element.props.onClick();
        expect(component.toJSON()).toMatchSnapshot();
    });

    test('dropdown is shown: hide dropdown', () => {
        const component = renderer.create(<AutoReload {...fakeProps} />);
        const element = findElement(component, 'btn-refresh');

        component.getInstance().setState({ isShowSelection: true });

        element.props.onClick();
        expect(component.toJSON()).toMatchSnapshot();
    });

    test('component unmounted: do nothing', () => {
        const component = new AutoReload(fakeProps);
        component.setState = jest.fn();
        component.state.isMounted = false;

        component._onCounterClick();
        expect(component.setState).not.toBeCalled();
    });

    test('component is loading: do nothing', () => {
        const props = Object.assign({ isLoading: true }, fakeProps);
        const component = new AutoReload(props);
        component.setState = jest.fn();
        component.state.isMounted = false;

        component._onCounterClick();
        expect(component.setState).not.toBeCalled();
    });
});

describe('componentDidMount', () => {
    const _setInterval = window.setInterval;

    beforeEach(() => {
        localStorageHelper.getWidgetReloadCountdown = jest.fn(() => ({}));
    });

    afterEach(() => {
        window.setInterval = _setInterval;
    });

    test('set isMounted = true', () => {
        const component = new AutoReload({ dataKey: 'test', defaultCountdown: 123 });
        component.setState = jest.fn();

        component.componentDidMount();

        expect(component.setState.mock.calls[0][0].isMounted).toBeTruthy();
    });

    test('setState callback: countdown equal or less than zero: do nothing', () => {
        [-1, 0].forEach(countdown => {
            localStorageHelper.getWidgetReloadCountdown = jest.fn(() => ({ test: countdown }));
            window.setInterval = jest.fn();
            const component = new AutoReload({
                dataKey: 'test',
                defaultCountdown: countdown
            });
            component.setState = jest.fn();
            component.componentDidMount();
            component.setState.mock.calls[0][1]();
            expect(window.setInterval).not.toBeCalled();
        });
    });

    test('setState callback: countdown greater than zero: start timer', () => {
        const component = new AutoReload(fakeProps);
        window.setInterval = jest.fn(() => 123456);
        component._onTick = jest.fn();
        component.setState = jest.fn();
        component.componentDidMount();
        component.setState.mock.calls[0][1]();

        expect(component.timerId).toBe(123456);
        expect(window.setInterval.mock.calls[0][1]).toBe(1000);

        window.setInterval.mock.calls[0][0]();
        expect(component._onTick).toBeCalled();
    });
});

describe('componentWillUnmount', () => {
    const _clearInterval = window.clearInterval;

    beforeEach(() => {
        window.clearInterval = jest.fn();
        localStorageHelper.getWidgetReloadCountdown = jest.fn(() => ({}));
    });

    afterEach(() => {
        window.clearInterval = _clearInterval;
    });

    test('no timerId: do nothing', () => {
        [null, undefined, 0, false].forEach(timerId => {
            const component = new AutoReload(fakeProps);
            component.timerId = timerId;
            component.setState = jest.fn();
            component.componentWillUnmount();
            expect(window.clearInterval).not.toBeCalled();
        });
    });

    test('has timerId: stop timer', () => {
        const component = new AutoReload(fakeProps);
        component.timerId = 123;
        component.setState = jest.fn();
        component.componentWillUnmount();
        expect(window.clearInterval).toBeCalledWith(123);
    });

    test('set isMounted = false', () => {
        const component = new AutoReload(fakeProps);
        component.setState = jest.fn();
        component.componentWillUnmount();
        expect(component.setState).toBeCalledWith({ isMounted: false });
    });
});

describe('_onTick()', () => {
    test('when mounted and countdown = 0: stop timer and invoke callback', () => {
        const component = new AutoReload(fakeProps);
        component._stopTimer = jest.fn();
        component.state.currentCountdown = 0;
        component.state.isMounted = true;
        fakeProps.reloadCallback = jest.fn();

        component._onTick();

        expect(component._stopTimer).toBeCalled();
        expect(fakeProps.reloadCallback).toBeCalled();
    });

    test('when umounted : do nothing', () => {
        const component = new AutoReload(fakeProps);
        component.setState = jest.fn();
        component.state.isMounted = false;

        component._onTick();

        expect(component.setState).not.toBeCalled();
    });

    test('when loading: do nothing', () => {
        const props = Object.assign({ isLoading: true }, fakeProps);
        const component = new AutoReload(props);
        component.setState = jest.fn();
        component.state.isMounted = true;

        component._onTick();

        expect(component.setState).not.toBeCalled();
    });
});

describe('_onSelectCounterValue', () => {
    beforeEach(() => {
        localStorageHelper.getWidgetReloadCountdown = jest.fn(() => ({}));
    });

    test('component is unmounted: do nothing', () => {
        const component = new AutoReload(fakeProps);
        component.setState = jest.fn();
        component.state.isMounted = false;
        component._onSelectCounterValue(4444);

        expect(component.setState).not.toBeCalled();
    });

    test('component mounted: update countdown and hide dropdown', () => {
        const component = new AutoReload(fakeProps);
        component.setState = jest.fn();
        component.state.isMounted = true;
        component._onSelectCounterValue(4444);

        expect(component.setState).toBeCalledWith({
            isShowSelection: false,
            countdown: 4444,
            currentCountdown: 4444
        });
    });

    test('component mounted: save new value', () => {
        const component = new AutoReload(fakeProps);
        component.setState = jest.fn();
        component.state.isMounted = true;
        component._onSelectCounterValue(4444);

        expect(localStorageHelper.saveWidgetReloadCountdown).toBeCalledWith({
            test: 4444
        });
    });
});

test('on counter value click: call _onSelectCounterValue(value)', () => {
    const props = Object.assign({
        countdownValues: [1, 2, 3, 4]
    }, fakeProps);
    const component = renderer.create(<AutoReload {...props} />);
    const instance = component.getInstance();
    instance.setState({ isShowSelection: true });
    instance._onSelectCounterValue = jest.fn();

    const element = findElement(component, '.dropdown-timer');
    element.props.children[0].props.onClick();
    expect(instance._onSelectCounterValue).toBeCalledWith(1);

    element.props.children[3].props.onClick();
    expect(instance._onSelectCounterValue).toBeCalledWith(4);
});

describe('componentDidUpdate', () => {
    test('still loading: do nothing', () => {
        const props = Object.assign({}, fakeProps, { isLoading: true });
        const component = new AutoReload(props);
        component.setState = jest.fn();

        component.componentDidUpdate({ isLoading: true });

        expect(component.setState).not.toBeCalled();
    });

    test('already stopped loading: do nothing', () => {
        const props = Object.assign({}, fakeProps, { isLoading: false });
        const component = new AutoReload(Object.assign(fakeProps));
        component.setState = jest.fn();

        component.componentDidUpdate({ isLoading: false });

        expect(component.setState).not.toBeCalled();
    });

    test('stop loading: reset countdown if it is zero', () => {
        const prevState = {
            currentCountdown: 0,
            countdown: 10001
        };
        const props = Object.assign({}, fakeProps, { isLoading: true });
        const component = new AutoReload(Object.assign(fakeProps));
        component.setState = jest.fn();
        component.timerId = 100;

        component.componentDidUpdate({ isLoading: true }, prevState);

        expect(component.setState).toBeCalledWith({ currentCountdown: 10001 });
    });

    test('stop loading: restart timer if it stopped', () => {
        const prevState = {
            currentCountdown: 10
        };
        const props = Object.assign({}, fakeProps, { isLoading: true });
        const component = new AutoReload(Object.assign(fakeProps));
        component.timerId = 0;
        component._startTimer = jest.fn();

        component.componentDidUpdate({ isLoading: true }, prevState);

        expect(component._startTimer).toBeCalled();
    });
});
