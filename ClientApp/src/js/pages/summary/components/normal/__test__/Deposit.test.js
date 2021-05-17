import React from 'react';
import renderer from 'react-test-renderer';
import Deposit from '../Deposit';
import widgetApi from '../../../../../lib/widgetApi';
import { widgetApiUrl, widgetKey } from '../../../modules/constants';
import WidgetBox from '../../../../../components/WidgetBox';

jest.mock('../../../../../components/WidgetBox');
jest.mock('../../../../../lib/widgetApi');

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
    const component = renderer.create(<Deposit {...fakeProps} />);
    expect(component.toJSON()).toMatchSnapshot();
});

test('componentDidMount: fetch data with correct criteria', () => {
    const component = renderer.create(<Deposit {...fakeProps} />);
    expect(widgetApi.fetchData).toBeCalledWith({
        Data: {
            CurrencyId: fakeProps.filter.currency,
            BaseCurrency: fakeProps.filter.baseCurrency
        },
        Url: widgetApiUrl.NormalDeposit,
        IsCrossSite: true
    })
});

test('has specific configs', () => {
    const component = new Deposit(fakeProps);
    expect(component.configs).toEqual({
        Url: widgetApiUrl.NormalDeposit,
        TitleKey: "deposit_today",
        WidgetName: "deposit",
        WidgetKey: widgetKey.NormalDeposit,
        defaultCountdown: 75,
        countdownValues: [15, 30, 45, 60, 75]
    })
});
