import React from 'react';
import WidgetBox from '../../../../components/WidgetBox';
import languageApi from '../../../../lib/languageApi';
import widgetApi from '../../../../lib/widgetApi';
import numberFormat from '../../../../lib/numberFormat';

export default class BaseNormalWidget extends React.Component {
    constructor(props, configs) {
        super(props);

        this.state = {
            data: 0,
            isLoading: true,
            isMounted: false
        };

        this.configs = configs;
        // configs = {
        //  Url: "",
        //  TitleKey: "",
        //  WidgetName: "",
        //  WidgetKey: ""
        // }
    }

    fetchData(props) {
        var param = {
            Data: {
                CurrencyId: props.filter.currency,
                BaseCurrency: props.filter.baseCurrency
            },
            Url: this.configs.Url,
            IsCrossSite: true
        };

        return widgetApi.fetchData(param)
            .then((response) => {
                this.setState({
                    data: response.data,
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

    _reload() {
        if (this.state.isMounted) {
            this.setState({ isLoading: true });
            this.fetchData(this.props);
        }
    }

    render() {
        return (
            <WidgetBox
                callbackParent={this.props.callbackParent}
                countdownConfigs={{
                    reloadCallback: () => this._reload(),
                    defaultCountdown: this.configs.defaultCountdown,
                    countdownValues: this.configs.countdownValues,
                    colorMode: 'white'
                }}
                wgItem={this.props.wgItemFromParent}
                widgetKey={this.configs.WidgetKey}
                title={languageApi.getRes(this.props.langCode, this.configs.TitleKey)}
                widgetName={this.configs.WidgetName}
                isLoading={this.state.isLoading} >
                <div className="center-block">
                    <span className="number-big">{numberFormat.formatCurrency(this.state.data, this.props.filter.baseCurrency)}</span>
                </div>
            </WidgetBox>
        );
    }
};
