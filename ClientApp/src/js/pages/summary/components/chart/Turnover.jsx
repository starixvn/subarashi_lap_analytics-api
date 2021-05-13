import React from 'react';
import ReactDOM from 'react-dom';
import { Doughnut, Chart } from 'react-chartjs-2';
import WidgetBox from '../../../../components/WidgetBox';

import languageApi from '../../../../lib/languageApi';
import { widgetKey, widgetApiUrl } from '../../lib/constants';
import widgetApi from '../../../../lib/widgetApi';
import numberFormat from '../../../../lib/numberFormat';

export default class Turnover extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            data: {},
            total: 0,
            isLoading: true,
            isMounted: false
        };

        this.options = {
            legend: { display: false, custom: true, id: 'custom-legend' },
            maintainAspectRatio: false,
            tooltips: {
                callbacks: {
                    label: function (tooltipItem, data) {
                        return data.labels[tooltipItem.index];
                    },
                    title: function (tooltipItem, data) {
                        return '';
                    }
                }
            },

            layout: { padding: { top: 10, bottom: 50, right: 130 } },
            hover: { animationDuration: 0 },
            animation: {
                onComplete: function (e) {
                    var self = this,
                        chart = this.chart,
                        ctx = chart.ctx;

                    var chartSize = Math.min(chart.width, chart.height);
                    if (chartSize < 100) {
                        return;
                    }

                    ctx.textAlign = "center";
                    ctx.fillStyle = "#fff";
                    ctx.save();
                    ctx.shadowColor = "black";
                    ctx.shadowBlur = 3;
                    var chartCircumference = chart.config.options.circumference;
                    self.data.datasets.forEach((dataset, datasetIndex) => {
                        var meta = self.getDatasetMeta(datasetIndex);

                        meta.data.forEach((element, index) => {
                            var segmentPercent = element._model.circumference / chartCircumference * 100;
                            var position = element.tooltipPosition();
                            if (element._model.circumference > 0) {
                                var label = segmentPercent < 1 ? "<1" : segmentPercent.toFixed(2);
                                ctx.fillText(label + '%', position.x, position.y);
                            }
                        });
                    });

                    ctx.restore();
                }
            }
        };
    }

    fetchData(props) {
        var param = {
            Data: {
                CurrencyId: props.filter.currency,
                BaseCurrency: props.filter.baseCurrency
            },
            Url: widgetApiUrl.ChartTurnover,
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
                                currency: props.filter.baseCurrency,
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
                                    '#25BE25',
                                    '#208E9B',
                                    '#688A08'
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
            <WidgetBox callbackParent={this.props.callbackParent} wgItem={this.props.wgItemFromParent} widgetKey={widgetKey.ChartTurnover}
                title={languageApi.getRes(this.props.langCode, "turnover_byplatform_month_todate")}
                widgetName="turnover default" isLoading={this.state.isLoading}>
                <Doughnut data={this.state.data} options={this.options} />
                <div id="custom-legend"></div>
                <div className="footer-total">
                    <span>
                        {languageApi.getRes(this.props.langCode, "total_turnover")}: {numberFormat.formatCurrency(this.state.total, this.props.filter.baseCurrency)}
                    </span>
                </div>
            </WidgetBox>
        );
    }
};

Chart.plugins.register({
    afterUpdate: function (chartInstance) {
        const options = chartInstance.options;
        const data = chartInstance.data;
        if (options.legend && options.legend.custom && data.datasets.length) {
            const ds = data.datasets[0];
            const legendOptions = data.labels.map((text, i) => ({
                text: text,
                index: i,
                value: numberFormat.formatCurrency(ds.data[i], ds.currency),
                color: ds.backgroundColor[i]
            }));

            const customLegendElement = document.getElementById(options.legend.id);
            if (!customLegendElement) return;

            const globalDefault = Chart.defaults.global;

            ReactDOM.render(
                (
                    <table className="chart-legend" style={{
                        fontSize: globalDefault.defaultFontSize,
                        fontStyle: globalDefault.defaultFontStyle,
                        fontFamily: globalDefault.defaultFontFamily
                    }}>
                        <tbody>
                            {
                                legendOptions.map(x => (
                                    <tr key={x.index}>
                                        <td className="cl__label">
                                            <span style={{ backgroundColor: x.color }} className="cl__point"></span>
                                            <span className="cl__text">{x.text}</span>
                                        </td>
                                        <td className="cl__value">
                                            <span className="cl__text">{x.value}</span>
                                        </td>
                                    </tr>
                                ))
                            }
                        </tbody>
                    </table>
                ),
                customLegendElement
            );
        }
    }
});
