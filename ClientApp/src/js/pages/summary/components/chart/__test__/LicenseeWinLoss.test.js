import React from 'react';
import renderer from 'react-test-renderer';
import * as ChartJs from 'react-chartjs-2';

import LicenseeWinLoss from '../LicenseeWinLoss';
import WidgetBox from '../../../../../components/WidgetBox';

import widgetApi from '../../../../../lib/widgetApi';
import numberFormat from '../../../../../lib/numberFormat';

jest.mock('../../../../../lib/widgetApi');
jest.mock('../../../../../components/WidgetBox');
jest.mock('react-chartjs-2');

const Bar = ChartJs.Bar;

const fakeProps = {
    filter: {
        currency: 999,
        baseCurrency: 'fap fap baseCurrency'
    },
    langCode: 'ch',
    refresh: 1000,
    some_prop: 'fap fap fap!',
    callbackParent: 'fake callbackParent',
    wgItemFromParent: 'fake wgItemFromParent'
};

function getDummyFetchData(count) {
    const data = [];
    let total = 56.78;

    while (count--) {
        data.push({
            Key: 'total_turnover',
            Value: (1.234 + count)
        });
        total += count;
    }

    return {
        data: {
            Data: data,
            Total: total
        }
    };
}

function fakeChartContext() {
    return {
        measureText: text => ({ width: text.length }),
        fillText: jest.fn()
    }
}

function fakeCharBarInstance() {
    return {
        chart: { ctx: fakeChartContext() },
        data: { datasets: [] },
        getDatasetMeta: jest.fn()
    }
}

function getDummyDataSets() {
    return [
        { data: [1.2, -2.3] },
        { data: [3.4, -4.5] }
    ]
}

function getDummyMetaData1() {
    return {
        data: [{ _model: { x: 5.8, y: 80 } }]
    }
}

function getDummyMetaData2() {
    return {
        data: [
            { _model: { x: 7.8, y: 9 } },
            { _model: { x: 9.10, y: 100 } }
        ]
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
    Bar.mockImplementation(() => ({
        render: function () {
            return (
                <div {...this.props} name="Bar">
                    hello bar
                </div>
            )
        }
    }));
});

afterEach(() => {
    widgetApi.mockReset();
});

test('render correctly', () => {
    // assign
    widgetApi
        .fetchData
        .mockImplementationOnce(() => Promise.resolve(getDummyFetchData(1)));

    // action
    const component = renderer.create(<LicenseeWinLoss { ...fakeProps } />);

    // assert
    expect(component.toJSON()).toMatchSnapshot();
});

test('componentDidMount: fetch data with given props', () => {
    // assign
    const promise = Promise.resolve(getDummyFetchData(2));
    widgetApi.fetchData.mockImplementationOnce(() => promise);

    // action
    const component = renderer.create(<LicenseeWinLoss { ...fakeProps } />);

    // assert
    return promise.then(() => expect(component.toJSON()).toMatchSnapshot());
});

test('componentDidMount: set mount state true', () => {
    // assign
    var component = new LicenseeWinLoss(fakeProps);
    component.setState = jest.fn();

    // action
    component.componentDidMount();

    // assert
    expect(component.setState.mock.calls[0][0].isMounted).toBeTruthy();
});

describe('componentDidUpdate(prevProps, prevState):', () => {
    test('next [refresh] > old [refresh], fetch new data', () => {
        // assign
        const prevState = { isMounted: true };
        const props = Object.assign({}, fakeProps);
        props.refresh += 1000;
        props.langCode = 'zhcn';

        const component = new LicenseeWinLoss(props);
        component.fetchData = jest.fn();
        component.setState = jest.fn();

        // action
        component.componentDidUpdate(fakeProps, prevState);

        // assert
        expect(component.setState).toBeCalledWith({ isLoading: true });
        expect(component.fetchData).toBeCalledWith(props);
    });

    test('next [refresh] <= old [refresh], do nothing', () => {
        // assign
        const prevState = { isMounted: true };
        const props = Object.assign({}, fakeProps);
        props.refresh -= 1000;

        const component = new LicenseeWinLoss(props);
        component.fetchData = jest.fn();
        component.setState = jest.fn();

        // action
        component.componentDidUpdate(fakeProps, prevState);

        // assert
        expect(component.fetchData).not.toBeCalled();
        expect(component.setState).not.toBeCalled();
    });

    test('component is umounted: do nothing', () => {
        // assign
        const prevState = { isMounted: false };
        const component = new LicenseeWinLoss(fakeProps);
        component.fetchData = jest.fn();
        component.setState = jest.fn();

        // action
        component.componentDidUpdate(fakeProps, prevState);

        // assert
        expect(component.fetchData).not.toBeCalled();
        expect(component.setState).not.toBeCalled();
    });
});

test('componentWillUnmount: set mount state false', () => {
    // assign
    const component = new LicenseeWinLoss(fakeProps);
    component.setState = jest.fn();

    // action
    component.componentWillUnmount();

    // assert
    expect(component.setState).toBeCalledWith({ isMounted: false });
});

test('fetch data by criteria', () => {
    // assign
    const component = new LicenseeWinLoss(fakeProps);
    const promise = Promise.resolve();
    promise.then = () => promise;
    widgetApi.fetchData.mockImplementationOnce(() => promise);

    // action
    component.fetchData(fakeProps);

    // assert
    expect(widgetApi.fetchData).toBeCalledWith({
        Data: {
            CurrencyId: fakeProps.filter.currency,
            BaseCurrency: fakeProps.filter.baseCurrency
        },
        Url: '/api/LicenseeWinLossByPlatform/GetData',
        IsCrossSite: true
    })
});

test('write log when fetch data fail', () => {
    // assign
    const component = new LicenseeWinLoss(fakeProps);
    const promise = Promise.resolve();
    promise.then = () => promise;
    promise.catch = callback => {
        callback('should write log when fail');
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

test('chart.scales.yAxes.ticks: format number', () => {
    const component = new LicenseeWinLoss(fakeProps);
    expect(component.options.scales.yAxes[0].ticks.callback(11111)).toBe(numberFormat.format(11111));
});

test('animation complete: re-format chart', () => {
    // assign
    const component = new LicenseeWinLoss(fakeProps);
    const bar = fakeCharBarInstance();

    // action
    component.options.animation.onComplete.call(bar);

    expect(bar.chart.ctx.fillStyle).toBe('#555');
    expect(bar.chart.ctx.textAlign).toBe('left');
    expect(bar.chart.ctx.textBaseline).toBe('middle');
});
test('animation complete: fill text each dataset', () => {
    // assign
    const component = new LicenseeWinLoss(fakeProps);
    const bar = fakeCharBarInstance();

    bar.data.datasets = getDummyDataSets();
    bar.getDatasetMeta.mockImplementation(datasetIndex => datasetIndex === 0
        ? getDummyMetaData1()
        : (datasetIndex === 1
            ? getDummyMetaData2()
            : null));

    // action
    component.options.animation.onComplete.call(bar);

    // build expected data
    const expected = [];
    bar.data.datasets.forEach((datasetValue, datasetIndex) => {
        bar.getDatasetMeta(datasetIndex).data.forEach((value, index) => {
            const text = numberFormat.formatDecimal(datasetValue.data[index]);
            const textWidth = bar.chart.ctx.measureText(text).width;
            const isNegativeValue = datasetValue.data[index] < 0;

            expected.push([
                text, value._model.x - (textWidth / 2),
                value._model.y - (isNegativeValue ? -7 : 6)
            ]);
        })
    });

    expect(bar.chart.ctx.fillText.mock.calls).toEqual(expected);
});
