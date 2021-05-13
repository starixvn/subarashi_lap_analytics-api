import React from 'react';
import renderer from 'react-test-renderer';
import Withdrawal from '../Withdrawal';
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
    const component = renderer.create(<Withdrawal {...fakeProps} />);
    expect(component.toJSON()).toMatchSnapshot();
});

test('componentDidMount: fetch data with correct criteria', () => {
    const component = renderer.create(<Withdrawal {...fakeProps} />);
    expect(widgetApi.fetchData).toBeCalledWith({
        Data: {
            CurrencyId: fakeProps.filter.currency,
            BaseCurrency: fakeProps.filter.baseCurrency
        },
        Url: widgetApiUrl.NormalWithdrawal,
        IsCrossSite: true
    })
});

test('has specific configs', () => {
    const component = new Withdrawal(fakeProps);
    expect(component.configs).toEqual({
        Url: widgetApiUrl.NormalWithdrawal,
        TitleKey: "withdrawal_today",
        WidgetName: "withdrawal",
        WidgetKey: widgetKey.NormalWithdrawal,
        defaultCountdown: 75,
        countdownValues: [15, 30, 45, 60, 75]
    })
});