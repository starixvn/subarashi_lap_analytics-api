import BaseNormalWidget from './BaseNormalWidget';
import { widgetApiUrl, widgetKey } from '../../lib/constants';

export default class Withdrawal extends BaseNormalWidget {
    constructor(props) {
        super(props, {
            Url: widgetApiUrl.NormalWithdrawal,
            TitleKey: "withdrawal_today",
            WidgetName: "withdrawal",
            WidgetKey: widgetKey.NormalWithdrawal,
            defaultCountdown: 75,
            countdownValues: [15, 30, 45, 60, 75]
        });
    }
};
