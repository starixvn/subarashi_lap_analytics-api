import React from 'react';
import { Bar } from 'react-chartjs-2';
import WidgetBox from '../../../../components/WidgetBox';

import { widgetKey, widgetApiUrl } from '../../lib/constants';
import widgetApi from '../../../../lib/widgetApi';
import numberFormat from '../../../../lib/numberFormat';
import languageApi from '../../../../lib/languageApi';

export default class LicenseeWinLoss extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            data: {},
            total: 0,
            isLoading: true,
            isMounted: false
        };

        this.options = {
            maintainAspectRatio: false,
            tooltips: {
                enabled: false
            },
            hover: {
                animationDuration: 0
            },
            animation: {
                onComplete: function () {
                    const self = this,
                        chart = this.chart,
                        ctx = chart.ctx;

                    ctx.textAlign = "center";
                    ctx.fillStyle = "#555";
                    ctx.textAlign = "left";
                    ctx.textBaseline = "middle";

                    self.data.datasets.forEach(function (dataset, datasetIndex) {
                        const meta = self.getDatasetMeta(datasetIndex);
                        meta.data.forEach(function (element, elementIndex) {
                            const value = dataset.data[elementIndex];
                            const text = numberFormat.formatDecimal(value);
                            const textWidth = ctx.measureText(text).width;
                            const yOffset = value > 0 ? 6 : -7;
                            ctx.fillText(text, element._model.x - (textWidth / 2), element._model.y - yOffset);
                        });
                    });
                }
            },
            layout: {
                padding: { top: 20, bottom: 50 }
            },
            scales: {
                xAxes: [{
                    gridLines: {
                        display: false
                    }
                }],
                yAxes: [{
                    ticks: {
                        callback: function (value, index, values) {
                            return numberFormat.format(value);
                        }
                    }
                }]
            },
            legend: {
                display: false
            }
        };
    }

    fetchData(props) {
        var param = {
            Data: {
                CurrencyId: props.filter.currency,
                BaseCurrency: props.filter.baseCurrency
            },
            Url: widgetApiUrl.ChartLicenseeWL,
            IsCrossSite: true
        };

        widgetApi.fetchData(param)
            .then((response) => {
                var labelList = [], dataList = [];

                for (var item of response.data.data) {
                    labelList.push(languageApi.getRes(this.props.langCode, item.key) || item.key);
                    dataList.push(item.value);
                }

                this.setState({
                    total: response.data.total,
                    data: {
                        labels: labelList,
                        datasets: [
                            {
                                data: dataList,
                                backgroundColor: [
                                    "#35A1FB",
                                    "#4B966B",
                                    "#4C6375",
                                    "#D9544F",
                                    "#4473C5",
                                    "#AFBF22",
                                    "#FFEC8B",
                                    "#BBFFFF",
                                    "#B8E9D5",
                                    "#FF66C1",
                                    '#524C68',
                                    '#39AF93',
                                    '#5F684C',
                                    '#F1A837',
                                    '#208E9B'
                                ]
                            }]
                    },
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
            <WidgetBox callbackParent={this.props.callbackParent} wgItem={this.props.wgItemFromParent}
                title={languageApi.getRes(this.props.langCode, "licensee_winloss_month_todate")} widgetKey={widgetKey.ChartLicenseeWL}
                widgetName="licenseewl default" isLoading={this.state.isLoading}>
                <Bar data={this.state.data} options={this.options} />
                <div className="footer-total">
                    <span>
                        {languageApi.getRes(this.props.langCode, "total_winloss")}: {numberFormat.formatCurrency(this.state.total, this.props.filter.baseCurrency)}
                    </span>
                </div>
            </WidgetBox>
        );
    }
};
