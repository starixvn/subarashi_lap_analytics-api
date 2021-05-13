import React from 'react';
import renderer from 'react-test-renderer';
import RegisteredCustomer from '../RegisteredCustomer';
import widgetApi from '../../../../../modules/widgetApi';
import WidgetBox from '../../../../../components/WidgetBox';

jest.mock('../../../../../modules/widgetApi');
jest.mock('../../../../../components/WidgetBox');

const fakeProps = {
    filter: {
        baseCurrency: "EUR",
        currency: 111
    },
    refresh: 100,
    langCode: "zhcn",
    callbackParent: 'fake callbackParent',
    wgItemFromParent: 'fake wgItemFromParent'
};

beforeAll(() => {
    WidgetBox.mockImplementation(() => ({
        render: function () {
            return (
                <div {...this.props} name="WidgetBox">
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
        .mockImplementationOnce(() => Promise.resolve({
            data: {
                Deposit: 12345.6789,
                NoneDeposit: 98765.4321
            }
        }));
    const component = renderer.create(<RegisteredCustomer {...fakeProps} />);
    let tree = component.toJSON();
    expect(tree).toMatchSnapshot();
});

test('componentDidMount: fetch data with given props', () => {
    // assert
    const promise = Promise.resolve({
        data: {
            Deposit: 12345.6789,
            NoneDeposit: 98765.4321
        }
    });
    widgetApi.fetchData.mockImplementationOnce(() => promise);

    // action
    const component = renderer.create(<RegisteredCustomer { ...fakeProps } />);

    // assert
    promise.then(() => expect(component.toJSON()).toMatchSnapshot());
});

test('componentDidMount: set isMounted = true', () => {
    const component = new RegisteredCustomer(fakeProps);
    component.setState = jest.fn();
    component.componentDidMount();
    expect(component.setState.mock.calls[0][0].isMounted).toBeTruthy();
});

test('componentWillUnmount: set isMounted = false', () => {
    const component = new RegisteredCustomer(fakeProps);
    component.setState = jest.fn();
    component.componentWillUnmount();
    expect(component.setState).toBeCalledWith({ isMounted: false });
});

test('componentDidUpdate: next.refresh > old.refresh, fetch new data', () => {
    // assign
    const prevState = { isMounted: true };
    const props = Object.assign({}, fakeProps);
    props.refresh += 1000;
    props.langCode = 'zhcn';

    const component = new RegisteredCustomer(props);
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

    const component = new RegisteredCustomer(props);
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
    const component = new RegisteredCustomer(fakeProps);
    component.fetchData = jest.fn();
    component.setState = jest.fn();

    // action
    component.componentDidUpdate(fakeProps, prevState);

    // assert
    expect(component.fetchData).not.toBeCalled();
    expect(component.setState).not.toBeCalled();
});

test('fetch data by criteria', () => {
    // assign
    const component = new RegisteredCustomer(fakeProps);
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
        Url: 'GetRegisteredUserOnline',
        IsCrossSite: false
    })
});

test('write log when fetch data fail', () => {
    // assign
    const component = new RegisteredCustomer(fakeProps);
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