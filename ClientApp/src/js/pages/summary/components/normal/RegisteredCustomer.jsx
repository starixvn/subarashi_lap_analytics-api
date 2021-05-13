import React from 'react';
import WidgetBox from '../../../../components/WidgetBox';

import widgetApi from '../../../../lib/widgetApi';
import languageApi from '../../../../lib/languageApi';
import numberFormat from '../../../../lib/numberFormat';
import { widgetKey, widgetApiUrl } from '../../lib/constants';

export default class RegisteredCustomer extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            deposit: 0,
            noneDeposit: 0,
            isLoading: true,
            isMounted: false
        };
    }

    fetchData(props) {
        var param = {
            Data: {
                CurrencyId: props.filter.currency,
                BaseCurrency: props.filter.baseCurrency
            },
            Url: widgetApiUrl.NormalRegisterCustomer,
            IsCrossSite: false
        };

        widgetApi.fetchData(param)
            .then((response) => {
                this.setState({
                    deposit: response.data.Deposit,
                    noneDeposit: response.data.NoneDeposit,
                    isLoading: false
                })
            })
            .catch(function (error) {
                console.log("error:", error);
            });
    }

    componentDidUpdate(prevProps, prevState) {
        if (prevState.isMounted && this.props.refresh > prevProps.refresh) {
            this.setState({ isLoading: true });
            this.fetchData(this.props);
        }
    }

    componentDidMount() {
        this.setState({ isMounted: true }, () => this.fetchData(this.props));
    }

    componentWillUnmount() {
        this.setState({ isMounted: false });
    }

    render() {
        return (
            <WidgetBox callbackParent={this.props.callbackParent} wgItem={this.props.wgItemFromParent} widgetKey={widgetKey.NormalRegisterCustomer} title={languageApi.getRes(this.props.langCode, "registered_customer")}
                widgetName="registered-customer default" isLoading={this.state.isLoading}>
                <div className="clearfix registered-customer-content">
                    <span className="icon-group-users left"></span>
                    <div className="right">
                        <div className="registered-info">
                            <p className="number">{numberFormat.format(this.state.deposit + this.state.noneDeposit)}</p>
                            <p className="text">{languageApi.getRes(this.props.langCode, "total_registered_cus_today")}</p>
                        </div>
                    </div>
                </div>
                <div className="clearfix registered-customer-content dark-bg">
                    <div className="registered-info">
                        <div className="center-block">
                            <p className="number">{numberFormat.format(this.state.deposit)}</p>
                            <p className="text">{languageApi.getRes(this.props.langCode, "deposit")}</p>
                        </div>
                    </div>
                    <div className="registered-info">
                        <div className="center-block">
                            <p className="number">{numberFormat.format(this.state.noneDeposit)}</p>
                            <p className="text">{languageApi.getRes(this.props.langCode, "none_deposit")}</p>
                        </div>
                    </div>
                </div>
            </WidgetBox>
        );
    }
};
