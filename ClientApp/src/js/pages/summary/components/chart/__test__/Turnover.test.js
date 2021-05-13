import React from 'react';
import ReactDOM from 'react-dom';
import renderer from 'react-test-renderer';
import * as ChartJs from 'react-chartjs-2';
import Turnover from '../Turnover';
import widgetApi from '../../../../../lib/widgetApi';
import WidgetBox from '../../../../../components/WidgetBox';

jest.mock('../../../../../lib/widgetApi');
jest.mock('../../../../../components/WidgetBox');
jest.mock('react-chartjs-2');
jest.mock('react-dom');

const Doughnut = ChartJs.Doughnut;

const fakeProps = {
    langCode: 'en',
    refresh: 12345,
    filter: {
        baseCurrency: 'USD',
        currency: 'HKD'
    },
    callbackParent: 'fake callbackParent',
    wgItemFromParent: 'fake wgItemFromParent'
};

const fakeResponse = {
    data: {
        Data: [{
            Key: 'total_turnover',
            Value: 1.2
        }, {
            Key: 'none_deposit',
            Value: 3.4
        }],
        Total: 98765.4321
    }
};

function fakeDoughnutInstance() {
    return {
        chart: {
            ctx: {
                fillText: jest.fn(),
                save: jest.fn(),
                restore: jest.fn()
            },
            width: 12345,
            height: 98765,
            config: { options: { circumference: 222 } }
        },
        data: {
            datasets: [
                { data: [1.2, 2.3] },
                { data: [3.4, 4.5] }
            ]
        },
        getDatasetMeta: jest.fn(index => index === 0 ? {
                data: [
                    { _model: { circumference: 999 }, tooltipPosition: () => ({ x: 1, y: 2 }) },
                    { _model: { circumference: 9999 }, tooltipPosition: () => ({ x: 11, y: 22 }) }
                ]
            } :
            (index === 1 ? {
                    data: [
                        { _model: { circumference: 888 }, tooltipPosition: () => ({ x: 111, y: 222 }) },
                        { _model: { circumference: 8888 }, tooltipPosition: () => ({ x: 1111, y: 2222 }) }
                    ]
                } :
                null))
    }
}

beforeAll(() => {
    WidgetBox.mockImplementation(() => ({
        render: function() {
            return (<div {...this.props }
                name = "WidgetBox" > { this.props.children } </div>
            )
        }
    }));
    Doughnut.mockImplementation(() => ({
        render: function() {
            return ( <div {...this.props }
                name = "Doughnut" >
                hello bar </div>
            )
        }
    }));
});

test('render correctly', () => {
            const promise = Promise.resolve();
            widgetApi.fetchData = jest.fn(() => promise);
            promise.then = () => promise;

            const component = renderer.create( < Turnover {...fakeProps }
                />);
                expect(component.toJSON()).toMatchSnapshot();
            });

        test('component did mount then fetch new data', () => {
                const promise = Promise.resolve(fakeResponse);
                widgetApi.fetchData = jest.fn(() => promise);

                const component = renderer.create( < Turnover {...fakeProps }
                    />);
                    promise.then(() => expect(component.toJSON()).toMatchSnapshot());
                });

            test('component did mount: set mount state true', () => {
                const component = new Turnover(fakeProps);
                component.setState = jest.fn();

                component.componentDidMount();
                expect(component.setState.mock.calls[0][0].isMounted).toBeTruthy();
            });

            test('component will un-mount: set mount state false', () => {
                const component = new Turnover(fakeProps);
                component.setState = jest.fn();

                component.componentWillUnmount();
                expect(component.setState).toBeCalledWith({ isMounted: false });
            });

            describe('componentWillReceiveProps(nextProps)', () => {
                test('refresh greater than old value: fetch data', () => {
                    const prevState = { isMounted: true };
                    const props = Object.assign({}, fakeProps);
                    props.refresh += 1000;
                    props.langCode = 'zhcn';

                    const component = new Turnover(props);
                    component.fetchData = jest.fn();
                    component.setState = jest.fn();

                    component.componentDidUpdate(fakeProps, prevState);

                    expect(component.setState).toBeCalledWith({ isLoading: true });
                    expect(component.fetchData).toBeCalledWith(props);
                });

                test('refresh less than or equal old value: do nothing', () => {
                    const prevState = { isMounted: true };
                    const props = Object.assign({}, fakeProps);
                    props.refresh -= 1000;

                    const component = new Turnover(props);
                    component.fetchData = jest.fn();
                    component.setState = jest.fn();

                    component.componentDidUpdate(fakeProps, prevState);

                    expect(component.fetchData).not.toBeCalled();
                    expect(component.setState).not.toBeCalled();
                });

                test('component unmouted: do nothing', () => {
                    const prevState = { isMounted: false };
                    const component = new Turnover(fakeProps);
                    component.fetchData = jest.fn();
                    component.setState = jest.fn();

                    component.componentDidUpdate(fakeProps, prevState);

                    expect(component.fetchData).not.toBeCalled();
                    expect(component.setState).not.toBeCalled();
                });
            });

            test('fetch data by criteria', () => {
                const component = new Turnover(fakeProps);
                const promise = Promise.resolve();
                promise.then = () => promise;
                widgetApi.fetchData = jest.fn(() => promise);

                const nextProps = {
                    filter: {
                        baseCurrency: 'USD 999',
                        currency: 'HKD 888'
                    }
                };

                component.fetchData(nextProps);
                expect(widgetApi.fetchData).toBeCalledWith({
                    Data: {
                        CurrencyId: nextProps.filter.currency,
                        BaseCurrency: nextProps.filter.baseCurrency
                    },
                    Url: '/api/TurnoverByPlatform/GetData',
                    IsCrossSite: true
                });
            });

            test('write log when fetch data fail', () => {
                // assign
                const component = new Turnover(fakeProps);
                const promise = Promise.resolve();
                promise.then = () => promise;
                promise.catch = c => {
                    c('should write log when fail');
                    return promise;
                };

                widgetApi.fetchData.mockImplementation(() => promise);

                const _log = console.log;
                console.log = jest.fn();

                //  action
                component.fetchData(fakeProps);

                // assert
                expect(console.log).toBeCalledWith('error:', 'should write log when fail');
                console.log = _log;
            });

            describe('tooltips', () => {
                test('label - return correct value', () => {
                    const expectedResult = "Asia";
                    const component = new Turnover(fakeProps);
                    const tooltipItem = { index: 0 };
                    const data = { labels: [expectedResult, "SmartSite"] };

                    const result = component.options.tooltips.callbacks.label(tooltipItem, data);

                    expect(result).toBe(expectedResult);
                });

                test('title - return empty string', () => {
                    const expectedResult = "";
                    const component = new Turnover(fakeProps);

                    const result = component.options.tooltips.callbacks.title({}, {});

                    expect(result).toBe(expectedResult);
                });
            });

            describe('animation complete', () => {
                test('re-format chart', () => {
                    const component = new Turnover(fakeProps);
                    const doughnut = fakeDoughnutInstance();
                    component.options.animation.onComplete.call(doughnut);

                    const formatObject = {
                        fillStyle: "#fff",
                        textAlign: "center",
                        shadowColor: "black",
                        shadowBlur: 3
                    };

                    expect(doughnut.chart.ctx).toEqual(expect.objectContaining(formatObject));
                });

                test('size of chart < 100: do nothing', () => {
                    const component = new Turnover(fakeProps);
                    const doughnut = fakeDoughnutInstance();

                    doughnut.chart.width = 99;
                    doughnut.chart.height = 88;

                    component.options.animation.onComplete.call(doughnut);

                    expect(doughnut.chart.ctx.save).not.toBeCalled();
                    expect(doughnut.chart.ctx.restore).not.toBeCalled();
                    expect(doughnut.chart.ctx.fillText).not.toBeCalled();
                });

                test('_model.circumference <= 0: do not fill text', () => {
                    const component = new Turnover(fakeProps);
                    const doughnut = fakeDoughnutInstance();
                    const meta0 = doughnut.getDatasetMeta(0);
                    const meta1 = doughnut.getDatasetMeta(1);

                    meta0.data.forEach(e => e._model.circumference = -1);
                    meta1.data.forEach(e => e._model.circumference = 0);

                    doughnut.getDatasetMeta = jest.fn(index => index === 0 ? meta0 : (index === 1 ? meta1 : null));

                    component.options.animation.onComplete.call(doughnut);

                    expect(doughnut.chart.ctx.save).toBeCalled();
                    expect(doughnut.chart.ctx.restore).toBeCalled();
                    expect(doughnut.chart.ctx.fillText).not.toBeCalled();
                });

                test('save context, fill text then restore', () => {
                    const component = new Turnover(fakeProps);
                    const doughnut = fakeDoughnutInstance();
                    const callOrders = [];

                    doughnut.chart.ctx.save = () => callOrders.push('save');
                    doughnut.chart.ctx.fillText = () => callOrders.push('fillText');
                    doughnut.chart.ctx.restore = () => callOrders.push('restore');

                    component.options.animation.onComplete.call(doughnut);

                    expect(callOrders).toEqual([
                        'save',
                        'fillText',
                        'fillText',
                        'fillText',
                        'fillText',
                        'restore'
                    ]);
                });

                test('fill text each data set when segmentPercent > 1', () => {
                    const component = new Turnover(fakeProps);
                    const doughnut = fakeDoughnutInstance();
                    component.options.animation.onComplete.call(doughnut);

                    expect(doughnut.chart.ctx.fillText.mock.calls).toEqual([
                        ['450.00%', 1, 2],
                        ['4504.05%', 11, 22],
                        ['400.00%', 111, 222],
                        ['4003.60%', 1111, 2222]
                    ]);
                });

                test('fill text each data set when segmentPercent < 1', () => {
                    const component = new Turnover(fakeProps);
                    const doughnut = fakeDoughnutInstance();
                    const meta = doughnut.getDatasetMeta(0);

                    meta.data.forEach(e => e._model.circumference = 1);
                    doughnut.getDatasetMeta = jest.fn(() => meta);

                    component.options.animation.onComplete.call(doughnut);

                    expect(doughnut.chart.ctx.fillText.mock.calls).toEqual([
                        ['<1%', 1, 2],
                        ['<1%', 11, 22],
                        ['<1%', 1, 2],
                        ['<1%', 11, 22]
                    ]);
                });
            });

            describe('custom legend plugin:', () => {
                    const afterUpdate = ChartJs.Chart.plugins.register.mock.calls[0][0].afterUpdate;

                    test('without legend: do not render', () => {
                        const chartInstance = { options: {} };
                        ReactDOM.render.mockReset();
                        afterUpdate(chartInstance);
                        expect(ReactDOM.render).not.toBeCalled();
                    });

                    test('not enable custom: do not render', () => {
                        const chartInstance = {
                            options: {
                                legend: { custom: false }
                            }
                        };
                        ReactDOM.render.mockReset();
                        afterUpdate(chartInstance);
                        expect(ReactDOM.render).not.toBeCalled();
                    });

                    test('no dataset: do not render', () => {
                        const chartInstance = {
                            options: {
                                legend: { custom: true }
                            },
                            data: {
                                datasets: []
                            }
                        };
                        ReactDOM.render.mockReset();
                        afterUpdate(chartInstance);
                        expect(ReactDOM.render).not.toBeCalled();
                    });

                    test('legend element id not found: do not render', () => {
                        const chartInstance = {
                            options: {
                                legend: { custom: true, id: 'fake-id' }
                            },
                            data: {
                                datasets: [{
                                    data: [11, 22],
                                    backgroundColor: ['red', 'green']
                                }],
                                labels: ['a', 'b']
                            }
                        };

                        ReactDOM.render.mockReset();
                        afterUpdate(chartInstance);
                        expect(ReactDOM.render).not.toBeCalled();
                    });

                    test('render correctly', () => {
                            const chartInstance = {
                                options: {
                                    legend: { custom: true, id: 'fake-id' }
                                },
                                data: {
                                    datasets: [{
                                        data: [11, 22],
                                        backgroundColor: ['red', 'green'],
                                        currency: 'EUR'
                                    }],
                                    labels: ['a', 'b']
                                }
                            };

                            var fakeElement = document.createElement('div');
                            fakeElement.id = 'fake-id';
                            document.body.appendChild(fakeElement);

                            ReactDOM.render.mockReset();
                            afterUpdate(chartInstance);
                            var result = renderer.create(<div> { ReactDOM.render.mock.calls[0][0] } </div>);
                                expect(result.toJSON()).toMatchSnapshot();
                            });

                        test('render to given legend element id', () => {
                            const chartInstance = {
                                options: {
                                    legend: { custom: true, id: 'fake-id' }
                                },
                                data: {
                                    datasets: [{
                                        data: [11, 22],
                                        backgroundColor: ['red', 'green']
                                    }],
                                    labels: ['a', 'b']
                                }
                            };

                            var fakeElement = document.createElement('div');
                            fakeElement.id = 'fake-id';
                            document.body.appendChild(fakeElement);

                            ReactDOM.render.mockReset();
                            afterUpdate(chartInstance);
                            expect(ReactDOM.render.mock.calls[0][1]).toEqual(fakeElement);
                        });
                    });
