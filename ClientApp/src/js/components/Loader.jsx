import React from 'react';
import { widgetKey } from '../lib/constants';

const baseNormalWidgets = [widgetKey.NormalCustomerBalance, widgetKey.NormalPendingFund, widgetKey.NormalDeposit, widgetKey.NormalWithdrawal]

export default class Loader extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <div className="loading-process" style={{ display: `${this.props.isLoading ? 'block' : 'none'}` }}>
                <div className="icon-loading">
                    <div className={`loader-inner ball-beat ${baseNormalWidgets.filter(x => x === this.props.widgetKey).length > 0 ? 'loader-white' : ''}`}>
                        <div></div>
                        <div></div>
                        <div></div>
                    </div>
                </div>
            </div>
        );
    }
}
