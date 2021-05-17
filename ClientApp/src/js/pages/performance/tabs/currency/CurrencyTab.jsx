import React from 'react';
import { bindActionCreators } from 'redux';
import { setAnalyticData } from '../../actions/analyticDataAction';
import { connect } from 'react-redux';
import { MEASUREMENT_ID } from '../../lib/constants';
import { generalService } from '../../lib/service';
import { currencyDifferenceTableDataMapper, currencyDistributionChartDataMapper, growthChartDisplayNameMapper } from './lib/service';
import { titleDistributionChart, titleOverallMarginChart } from "./lib/constants";
import { tabMenuKey } from '../../lib/constants';
import PerformanceChildTab from '../PerformanceChildTab';
import withGA from '../../../../components/HOC/WithGA';

export class CurrencyTab extends PerformanceChildTab {
  constructor(props) {
    super(props);
    this.state = {
      ...this.state,
      ...{}
    }
    this.titleDistributionChart = titleDistributionChart;
    this.isAutoScaleGrowthChart = false;
    this.tabKey = tabMenuKey.currency;
  }

  _getDistributionChartData() {
    const { distributionData, topDistributions, measurementId } = this.props.analyticData;
    return {
      distributionData,
      topDistributions,
      measurementId,
      dataMapper: currencyDistributionChartDataMapper
    }
  }

  _getOverallChartData() {
    const { overallMarginData } = this.props.analyticData;
    return {
      overallMarginData,
      title: titleOverallMarginChart
    }
  }

  _getDifferenceTableData() {
    const { measurementDifferenceData } = this.props.analyticData;
    return {
      measurementDifferenceData,
      dataMapper: currencyDifferenceTableDataMapper
    }
  }

  _getGrowthChartData() {
    const { growthData, measurementId } = this.props.analyticData;
    return {
      growthData,
      measurementId,
      displayNameMapper: growthChartDisplayNameMapper
    }
  }

  render() {
    const { measurementId } = this.props.analyticData;
    return measurementId ? (
      <div className="child-tab currency-tab" style={generalService.getScrollTabStyle()}>
       <div className="side-tab-container">
          <div className='side-tab-left'>{measurementId === MEASUREMENT_ID.MARGIN ? this.renderOverallChart() : this.renderDistributionChart()}</div>
          <div className='side-tab-right'>{this.renderDifferenceTable()}</div>
        </div>
        <div className="side-tab-container">
          {this.renderGrowthChart()}
        </div>
      </div>
    ) : null;
  }
}

function mapDispatchToProps(dispatch) {
  return bindActionCreators({
    setAnalyticData
  }, dispatch);
}

function mapStateToProps(state) {
  const { analyticData } = state;
  return Object.assign({}, {
    analyticData
  });
}

export default connect(mapStateToProps, mapDispatchToProps)(withGA(CurrencyTab, 'performance/currency'));
