import React from 'react';
import { HorizontalBar } from 'react-chartjs-2';
import WidgetBox from '../../../../components/WidgetBox';

import { widgetKey, widgetApiUrl } from '../../lib/constants';
import widgetApi from '../../../../lib/widgetApi';
import languageApi from '../../../../lib/languageApi';
import numberFormat from '../../../../lib/numberFormat';

export default class OnlineCustomer extends React.Component {
    constructor(props) {
        super(props);

        const fontColor = "#5162BA";

        this.state = {
            data: {
                labels: [
                    languageApi.getRes(this.props.langCode, "Web"),
                    languageApi.getRes(this.props.langCode, "Wap"),
                    languageApi.getRes(this.props.langCode, "SmartSite")
                ],
                datasets: []
            },
            isLoading: true,
            isMounted: false
        };

        this.options = {
            beginAtZero: true,
            type: 'line',
            maintainAspectRatio: false,
            tooltips: { enabled: false },
            hover: { animationDuration: 0 },
            animation: {
                onComplete: function () {
                    var self = this,
                        chart = this.chart,
                        ctx = chart.ctx;

                    ctx.textAlign = "center";
                    ctx.fillStyle = fontColor;
                    ctx.textAlign = "left";
                    ctx.textBaseline = "middle";

                    self.data.datasets.forEach(function (dataset, datasetIndex) {
                        var meta = self.getDatasetMeta(datasetIndex);
                        meta.data.forEach(function (element, elementIndex) {
                            ctx.fillText(numberFormat.format(dataset.data[elementIndex]), chart.width - 90, element._model.y);
                        });
                    });
                }
            },
            layout: {
                padding: { right: 100, bottom: 20, left: 30 }
            },
            scales: {
                xAxes: [{
                    barPercentage: 0.5,
                    display: false,
                    ticks: { min: 0 }
                }],
                yAxes: [{
                    ticks: {
                        fontColor: fontColor
                    },
                    gridLines: {
                        display: false
                    }
                }]
            },
            legend: {
                display: false
            }
        };
    }

    fetchData() {
        var param = {
            Url: widgetApiUrl.ChartlOnlineCustomer,
            IsCrossSite: false
        };

        return widgetApi.fetchData(param)
            .then((response) => {
                this.setState({
                    data: {
                        datasets: [{
                            backgroundColor: [
                                'rgb(241, 91, 32)',
                                'rgb(75, 150, 107)',
                                'rgb(61, 165, 250)'
                            ],
                            data: [response.data.Web, response.data.Wap, response.data.SmartSite],
                            stacked: true
                        }]
                    },
                    isLoading: false
                });
            });
    }

    componentDidUpdate(prevProps, prevState) {
        if (prevState.isMounted && this.props.refresh > prevProps.refresh) {
            this.setState({ isLoading: true });
            this.fetchData();
        }
    }

    componentDidMount() {
        this.setState({ isMounted: true }, () => this.fetchData());
    }

    componentWillUnmount() {
        this.setState({ isMounted: false });
    }

    _reload() {
        if (this.state.isMounted) {
            this.setState({ isLoading: true });
            this.fetchData();
        }
    }

    render() {
        return (
            <WidgetBox callbackParent={this.props.callbackParent}
                wgItem={this.props.wgItemFromParent}
                widgetKey={widgetKey.ChartlOnlineCustomer}
                title={languageApi.getRes(this.props.langCode, "online_customer")}
                widgetName="online-customer default"
                isLoading={this.state.isLoading}
                countdownConfigs={{
                    reloadCallback: () => this._reload(),
                    defaultCountdown: 75,
                    countdownValues: [15, 30, 45, 60, 75],
                    colorMode: 'blue'
                }}>
                <HorizontalBar data={this.state.data} options={this.options} />
            </WidgetBox>
        );
    }
};
