import React from 'react';
import renderer from 'react-test-renderer';
import * as ChartJs from 'react-chartjs-2';
import OnlineCustomer from '../OnlineCustomer';
import widgetApi from '../../../../../lib/widgetApi';
import WidgetBox from '../../../../../components/WidgetBox';

jest.mock('../../../../../lib/widgetApi');
jest.mock('../../../../../components/WidgetBox');
jest.mock('react-chartjs-2');

const HorizontalBar = ChartJs.HorizontalBar;

const fakeProps = {
    langCode: 'ch',
    refresh: 12345,
    callbackParent: 'fake callbackParent',
    wgItemFromParent: 'fake wgItemFromParent'
};

const fakeResponse = {
    data: {
        Web: 'webwwebwebweb',
        Wap: 'wapwapwap',
        SmartSite: 'smsmsmsm'
    }
};

function fakeHorizontalBarInstance() {
    return {
        chart: {
            ctx: {
                fillText: jest.fn()
            },
            width: 123456789
        },
        data: {
            datasets: [
                {
                    data: [1.2, 2.3]
                }, {
                    data: [3.4, 4.5]
                }
            ]
        },
        getDatasetMeta: jest.fn(index => index === 0
            ? {
                data: [
                    {
                        _model: {
                            y: 999
                        }
                    }, {
                        _model: {
                            y: 9999
                        }
                    }
                ]
            }
            : (index === 1
                ? {
                    data: [
                        {
                            _model: {
                                y: 888
                            }
                        }, {
                            _model: {
                                y: 8888
                            }
                        }
                    ]
                }
                : null))
    }
}

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
    HorizontalBar.mockImplementation(() => ({
        render: function () {
            return (
                <div {...this.props} name="HorizontalBar">
                    hello bar
                </div>
            )
        }
    }));
});

test('render correctly', () => {
    const promise = Promise.resolve();
    widgetApi.fetchData = jest.fn(() => promise);
    promise.then = () => promise;

    const component = renderer.create(<OnlineCustomer {...fakeProps} />);
    expect(component.toJSON()).toMatchSnapshot();
});

describe('componentDidMount:', () => {
    test('fetch new data', () => {
        const promise = Promise.resolve(fakeResponse);
        widgetApi.fetchData = jest.fn(() => promise);

        const component = renderer.create(<OnlineCustomer {...fakeProps} />);
        promise.then(() => expect(component.toJSON()).toMatchSnapshot());
    });

    test('component did mount set mount state true', () => {
        const component = new OnlineCustomer(fakeProps);
        component.setState = jest.fn();
        component.componentDidMount();
        expect(component.setState.mock.calls[0][0].isMounted).toBeTruthy();
    });
});

test('component will un-mount set mount state false', () => {
    const component = new OnlineCustomer(fakeProps);
    component.setState = jest.fn();
    component.componentWillUnmount();
    expect(component.setState).toBeCalledWith({ isMounted: false });
});

describe('componentDidUpdate(nextProps)', () => {
    test('component is unmounted: do nothing', () => {
        const prevState = { isMounted: false };
        const component = new OnlineCustomer(fakeProps);
        component.fetchData = jest.fn();
        component.setState = jest.fn();

        component.componentDidUpdate(fakeProps, prevState);

        expect(component.fetchData).not.toBeCalled();
        expect(component.setState).not.toBeCalled();
    });

    test('refresh greater than old value: fetch data', () => {
        const component = new OnlineCustomer(fakeProps);
        component.setState = jest.fn();
        component.fetchData = jest.fn();

        const prevState = { isMounted: true };
        const prevProps = Object.assign({}, fakeProps, { refresh: 123 });

        component.componentDidUpdate(prevProps, prevState);

        expect(component.setState).toBeCalledWith({ isLoading: true });
        expect(component.fetchData).toBeCalled();
    });

    test('refresh less than or equal old value: do nothing', () => {
        const prevState = { isMounted: true };
        const props = Object.assign({}, fakeProps, { refresh: 123 });
        const component = new OnlineCustomer(props);
        component.setState = jest.fn();
        component.fetchData = jest.fn();

        component.componentDidUpdate(fakeProps, prevState);
        expect(component.setState).not.toBeCalled();
        expect(component.fetchData).not.toBeCalled();
    });
});

test('fetch data by criteria', () => {
    const component = new OnlineCustomer(fakeProps);
    const promise = Promise.resolve();
    promise.then = () => promise;
    widgetApi.fetchData = jest.fn(() => promise);
    component.fetchData();
    expect(widgetApi.fetchData).toBeCalledWith({ Url: 'GetOnlineCustomers', IsCrossSite: false });
});

test('fetch data: return fetch data promise', () => {
    // assign
    const component = new OnlineCustomer(fakeProps);
    const promise = Promise.resolve();

    promise.then = () => promise;
    promise.catch = () => promise;
    widgetApi.fetchData = jest.fn(() => promise);

    // action
    let result = component.fetchData(fakeProps);

    // assert
    expect(result).toBe(promise);
});

describe('animation complete', () => {
    test('re-format chart', () => {
        const component = new OnlineCustomer(fakeProps);
        const horizontalBar = fakeHorizontalBarInstance();
        component
            .options
            .animation
            .onComplete
            .call(horizontalBar);

        const formatObject = {
            fillStyle: "#5162BA",
            textAlign: "left",
            textBaseline: "middle"
        };

        expect(horizontalBar.chart.ctx).toEqual(expect.objectContaining(formatObject));
    });

    test('fill text each data set', () => {
        const component = new OnlineCustomer(fakeProps);
        const horizontalBar = fakeHorizontalBarInstance();
        component
            .options
            .animation
            .onComplete
            .call(horizontalBar);

        const formatObject = {
            fillStyle: "#5162BA",
            textAlign: "left",
            textBaseline: "middle"
        };

        expect(horizontalBar.chart.ctx.fillText.mock.calls).toEqual([
            ['1', 123456699, 999],
            ['2', 123456699, 9999],
            ['3', 123456699, 888],
            ['5', 123456699, 8888],
        ]);
    });
});

describe('when reloadCallback is called:', () => {
    test('if component is mounted then fetch data', () => {
        // assign
        const component = new OnlineCustomer(fakeProps);
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
        const component = new OnlineCustomer(fakeProps);
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
