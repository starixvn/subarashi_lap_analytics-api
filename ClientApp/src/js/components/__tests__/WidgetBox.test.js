import React from 'react';
import renderer from 'react-test-renderer';
import WidgetBox from '../WidgetBox';
import AutoReload from '../AutoReload';

jest.mock('../AutoReload');

const fakeProps = () => ({
    wgItem: {
        key: 'fap fap fap',
        enable: true
    },
    widgetKey: 'fap fap fap',
    widgetName: 'w i d g e t',
    title: 'T.T',
    isLoading: true,
    callbackParent: jest.fn()
});

function findElementByClassName(component, className) {
    if (component.toJSON) {
        return findElementByClassName(component.toJSON(), className);
    }

    if (component.props && component.props.className && component.props.className.match(new RegExp(className))) {
        return component;
    }

    if (component.children) {
        for (let i = 0; i < component.children.length; i++) {
            let found = findElementByClassName(component.children[i], className);
            if (found) {
                return found;
            }
        }

    }

    return null;
}

describe('render correctly', () => {
    test('widget enabled', () => {
        const component = renderer.create(<WidgetBox {...fakeProps() } />);
        expect(component.toJSON()).toMatchSnapshot();
    });

    test('widget disabled', () => {
        const props = fakeProps();
        props.wgItem.enable = false;
        const component = renderer.create(<WidgetBox {...props} />);
        expect(component.toJSON()).toMatchSnapshot();
    });

    test('has auto reload config', () => {
        AutoReload.mockImplementation(() => ({
            render: function () {
                return <span {...this.props}>AutoReload</span>
            }
        }));
        const props = Object.assign({
            countdownConfigs: {
                countdownValues: [1, 2, 3],
                defaultCountdown: 123,
                reloadCallback: 'reloadCallback'
            }
        }, fakeProps());
        const component = renderer.create(<WidgetBox {...props} />);
        expect(component.toJSON()).toMatchSnapshot();
    });
});

test('toggleClose is called: toggle close state and call back to parent', () => {
    const props = fakeProps();
    const component = new WidgetBox(props);
    component.toggleClose();

    expect(props.callbackParent).toBeCalledWith({
        action: 'wgSyncStatus',
        items: [
            {
                key: 'fap fap fap',
                enable: false
            }
        ]
    });
});

test('on close element clicked: toggle close', () => {
    const component = renderer.create(<WidgetBox {...fakeProps() } />);
    const componentInstance = component.getInstance();
    const closeElement = findElementByClassName(component, 'icon-close');
    expect(closeElement)
        .not
        .toBeNull();
    componentInstance.toggleClose = jest.fn();

    closeElement
        .props
        .onClick();

    expect(componentInstance.toggleClose).toBeCalled();
});