import BaseNormalWidget from './BaseNormalWidget';
import { widgetKey, widgetApiUrl } from '../../lib/constants';

export default class CustomerBalance extends BaseNormalWidget {
    constructor(props) {
        super(props, {
            Url: widgetApiUrl.NormalCustomerBalance,
            TitleKey: "customer_balance",
            WidgetName: "customer-balance",
            WidgetKey: widgetKey.NormalCustomerBalance,
            defaultCountdown: 75,
            countdownValues: [15, 30, 45, 60, 75]
        });
    }
};
