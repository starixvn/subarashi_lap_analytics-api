import React from 'react';
import renderer from 'react-test-renderer';
import BaseNormalWidget from '../BaseNormalWidget';
import widgetApi from '../../../../../modules/widgetApi';
import WidgetBox from '../../../../../components/WidgetBox';

jest.mock('../../../../../modules/widgetApi');
jest.mock('../../../../../components/WidgetBox');

const fakeProps = {
    filter: {
        baseCurrency: "EUR",
        currency: 1111
    },
    refresh: 1000,
    langCode: "en",
    callbackParent: 'fake callbackParent',
    wgItemFromParent: 'f**k wgItemFromParent'
};
const fakeConfigs = {
    WidgetKey: 'this is a f**k key',
    Url: 'this is a f**k url',
    WidgetName: 'this is a f**k name',
    TitleKey: 'licensee_winloss_month_todate',
    defaultCountdown: 123,
    countdownValues: [1, 2, 3]
};
class FakeConfiguredBaseNormalWidget extends BaseNormalWidget {
    constructor(props) {
        super(props, fakeConfigs);
    }
}

beforeAll(() => {
    WidgetBox.mockImplementation(() => ({
        render: function () {
            return (
                <div {...this.props}>
                    {this.props.children}
                </div>
            )
        }
    }));
});

afterEach(() => {
    widgetApi.mockReset();
});

test('renders correctly', () => {
    widgetApi
        .fetchData
        .mockImplementation(() => Promise.resolve({ data: 999.888 }))
    const component = renderer.create(<FakeConfiguredBaseNormalWidget {...fakeProps} />);
    let tree = component.toJSON();
    expect(tree).toMatchSnapshot();
});

test('data was rendered correctly from API', () => {
    const promise = Promise.resolve({ data: 888.777 });
    widgetApi
        .fetchData
        .mockImplementation(() => promise);
    const component = renderer.create(<FakeConfiguredBaseNormalWidget {...fakeProps} />);

    promise.then(() => expect(component.toJSON()).toMatchSnapshot());
});

test('componentDidUpdate: next.refresh > old.refresh, fetch new data', () => {
    // assign
    const prevState = { isMounted: true };
    const props = Object.assign({}, fakeProps);
    props.refresh += 1000;
    props.langCode = 'zhcn';

    const component = new FakeConfiguredBaseNormalWidget(props);
    component.fetchData = jest.fn();
    component.setState = jest.fn();

    // action
    component.componentDidUpdate(fakeProps, prevState);

    // assert
    expect(component.setState).toBeCalledWith({ isLoading: true });
    expect(component.fetchData).toBeCalledWith(props);
});

test('componentDidUpdate: next.refresh <= old.refresh, do nothing', () => {
    // assign
    const prevState = { isMounted: true };
    const props = Object.assign({}, fakeProps);
    props.refresh -= 1000;

    const component = new FakeConfiguredBaseNormalWidget(props);
    component.fetchData = jest.fn();
    component.setState = jest.fn();

    // action
    component.componentDidUpdate(fakeProps, prevState);

    // assert
    expect(component.fetchData).not.toBeCalled();
    expect(component.setState).not.toBeCalled();
});

test('componentDidUpdate: not mounted: do nothing', () => {
    // assign
    const prevState = { isMounted: false };
    const component = new FakeConfiguredBaseNormalWidget(fakeProps);
    component.fetchData = jest.fn();
    component.setState = jest.fn();

    // action
    component.componentDidUpdate(fakeProps, prevState);

    // assert
    expect(component.fetchData).not.toBeCalled();
    expect(component.setState).not.toBeCalled();
});

test('componentDidMount: set mount state true', () => {
    // assign
    const component = new FakeConfiguredBaseNormalWidget(fakeProps);
    component.setState = jest.fn();

    // action
    component.componentDidMount();

    // assert
    expect(component.setState.mock.calls[0][0].isMounted).toBeTruthy();
});

test('componentWillUnmount: set mount state false', () => {
    // assign
    const component = new FakeConfiguredBaseNormalWidget(fakeProps);
    component.setState = jest.fn();

    // action
    component.componentWillUnmount();

    // assert
    expect(component.setState).toBeCalledWith({ isMounted: false });
});

test('fetch data by criteria', () => {
    // assign
    const component = new FakeConfiguredBaseNormalWidget(fakeProps);
    const promise = Promise.resolve();

    promise.then = () => promise;
    widgetApi
        .fetchData
        .mockImplementationOnce(() => promise);

    // action
    component.fetchData(fakeProps);

    // assert
    expect(widgetApi.fetchData).toBeCalledWith({
        Data: {
            CurrencyId: fakeProps.filter.currency,
            BaseCurrency: fakeProps.filter.baseCurrency
        },
        Url: fakeConfigs.Url,
        IsCrossSite: true
    })
});

test('fetch data: return fetch data promise', () => {
    // assign
    const component = new FakeConfiguredBaseNormalWidget(fakeProps);
    const promise = Promise.resolve();

    promise.then = () => promise;
    promise.catch = () => promise;
    widgetApi
        .fetchData
        .mockImplementationOnce(() => promise);

    // action
    let result = component.fetchData(fakeProps);

    // assert
    expect(result).toBe(promise);
});

test('write log when fetch data fail', () => {
    // assign
    const component = new FakeConfiguredBaseNormalWidget(fakeProps);
    const promise = Promise.resolve();
    promise.then = () => promise;
    promise.catch = c => {
        c('should write log when fail');
        return promise;
    };

    widgetApi
        .fetchData
        .mockImplementation(() => promise);

    const _log = console.log;
    console.log = jest.fn();

    //  action
    component.fetchData(fakeProps);

    // assert
    expect(console.log).toBeCalledWith('error:', 'should write log when fail');
    console.log = _log;
});

describe('when reloadCallback is called:', () => {
    test('if component is mounted then fetch data', () => {
        // assign
        const component = new FakeConfiguredBaseNormalWidget(fakeProps);
        component.setState = jest.fn();
        component.fetchData = jest.fn(() => Promise.resolve());
        component.state.isMounted = true;

        //  action
        const rendered = component.render();
        rendered.props.countdownConfigs.reloadCallback();

        // assert
        expect(component.setState).toBeCalledWith({ isLoading: true })
        expect(component.fetchData).toBeCalledWith(fakeProps);
    });

    test('component is un-mounted: do nothing', () => {
        // assign
        const component = new FakeConfiguredBaseNormalWidget(fakeProps);
        component.setState = jest.fn();
        component.fetchData = jest.fn();
        component.state.isMounted = false

        //  action
        const rendered = component.render();
        rendered.props.countdownConfigs.reloadCallback();

        // assert
        expect(component.setState).not.toBeCalled();
        expect(component.fetchData).not.toBeCalled();
    });
});
