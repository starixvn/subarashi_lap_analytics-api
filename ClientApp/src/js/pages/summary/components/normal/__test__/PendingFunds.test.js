import React from 'react';
import renderer from 'react-test-renderer';
import PendingFunds from '../PendingFunds';
import widgetApi from '../../../../../modules/widgetApi';
import { widgetApiUrl, widgetKey } from '../../../modules/constants';
import WidgetBox from '../../../../../components/WidgetBox';

jest.mock('../../../../../components/WidgetBox');
jest.mock('../../../../../modules/widgetApi');

const fakeProps = {
    filter: {
        baseCurrency: "EUR",
        currency: "123"
    },
    refresh: 100,
    langCode: 'en',
    callbackParent: 'fake callbackParent',
    wgItemFromParent: 'fake wgItemFromParent'
};

beforeAll(() => {
    widgetApi
        .fetchData
        .mockImplementation(() => Promise.resolve({ data: 9999 }));
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

test('renders correctly', () => {
    const component = renderer.create(<PendingFunds {...fakeProps} />);
    expect(component.toJSON()).toMatchSnapshot();
});

test('componentDidMount: fetch data with correct criteria', () => {
    const component = renderer.create(<PendingFunds {...fakeProps} />);
    expect(widgetApi.fetchData).toBeCalledWith({
        Data: {
            CurrencyId: fakeProps.filter.currency,
            BaseCurrency: fakeProps.filter.baseCurrency
        },
        Url: widgetApiUrl.NormalPendingFund,
        IsCrossSite: true
    })
});

test('has specific configs', () => {
    const component = new PendingFunds(fakeProps);
    expect(component.configs).toEqual({
        Url: widgetApiUrl.NormalPendingFund,
        TitleKey: "pending_funds",
        WidgetName: "pending-fund",
        WidgetKey: widgetKey.NormalPendingFund,
        defaultCountdown: 75,
        countdownValues: [15, 30, 45, 60, 75]
    })
});