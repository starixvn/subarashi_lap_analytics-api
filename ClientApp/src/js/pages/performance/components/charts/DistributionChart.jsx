import React, { PureComponent } from 'react';
import StackedBarChart from './base/StackedBarChart';
import MixPieChart from './base/MixPieChart';
import ReportTable from './base/ReportTable';
import {ChartMode} from '../../enums/enum';
import DistributionChartSwitcher from '../buttons/DistributionChartSwitcher';

export default class DistributionChart extends PureComponent {
  constructor(props) {
    super(props);
    this.state = {
      style: {},
      chartMode: ChartMode.PIE_CHART
    };
    this.refSelf = React.createRef();
  }

  static limitAxisY = {
     max: 100
  }

  componentDidMount() {
    const { getStyle } = this.props;
    getStyle && this.setState({
      style: getStyle(this.refSelf)
    });
  }

  handleChangeChartMode = chartMode => {
    this.setState({ chartMode });
  }

  renderChart = (mode, labels, tabKey, stackedChartData, pieChartData, reportInTextData) => {
    if (mode == ChartMode.PIE_CHART ) {
        return (
          <MixPieChart pieChartData={pieChartData} labels={labels}/>
        );
    } else if (mode == ChartMode.COLUMN_CHART) {
        return (
          <StackedBarChart stackedChartData={stackedChartData} labels={labels} limitAxisY={DistributionChart.limitAxisY}/>
        );
    } else {
      return (
        <ReportTable reportTableData={reportInTextData} tabKey = {tabKey} />
      );
    }
  }

  render() {
    const { style, chartMode } = this.state;
    const { getTitle, tabKey, measurementId, distributionData, topDistributions, dataMapper } = this.props;
    const { labels, stackedChartData, pieChartData, reportInTextData } = dataMapper(distributionData, topDistributions);
    const titleDistribution = getTitle(chartMode, measurementId);
    return (
      <div className="info-zone-container" ref={this.refSelf} style={style}>
        <div className="info-zone-header">
          <div className="flex-container">
            <div className="title-distribution">
              {titleDistribution}
            </div>
            <div>
              <DistributionChartSwitcher value={chartMode} onSelectChart={this.handleChangeChartMode}/>
            </div>
          </div>
        </div>
        <div className="info-zone-body">
          {this.renderChart(chartMode, labels, tabKey, stackedChartData, pieChartData, reportInTextData)}
        </div>
      </div>
    );
  }
}
