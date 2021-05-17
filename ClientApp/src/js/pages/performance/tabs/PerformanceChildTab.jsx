import React, { PureComponent } from 'react';
import { growthChartConsts } from './constants';
import { MEASUREMENT_ID } from '../lib/constants';
import { differenceTableDataMapper, distributionChartDataMapper, overallChartDataMapper, calculateBarWidthOfGrowthChart } from './service';
import DistributionChart from '../components/charts/DistributionChart';
import OverallChart from '../components/charts/OverallChart';
import ComparisonTable from '../components/normal/ComparisonTable';
import GrowthChart from '../components/charts/GrowthChart';
import { generalService } from '../lib/service';
import languageApi from '../../../lib/languageApi';
import { dashboardLangCode, langCodes } from '../../../lib/constants';
import { tabMenuKey } from '../../performance/lib/constants';
import { ChartMode } from '../enums/enum';

export default class PerformanceChildTab extends PureComponent {
  constructor(props) {
    super(props);
    this.state = {
      isPeriodGrowthDataType: true,
      selectedColumnDrillDown: null
    }
    this.titleDistributionChart = '';
    this.isAutoScaleGrowthChart = true;
  }

  componentDidMount() {
    this.props.setAnalyticData({});
  }

  componentDidUpdate(prevProps) {
    const { isPeriodGrowthDataType } = this.state;
    if (this.props.analyticData !== prevProps.analyticData && !isPeriodGrowthDataType) {
      this.setState({
        isPeriodGrowthDataType: true
      })
    }
  }

  getTitle = (chartMode, measurementId) => {
      const measurement = generalService.getMeasurementNameById(measurementId) || {};
      if (this.tabKey === tabMenuKey.bettype && chartMode === ChartMode.PIE_CHART) {
        return `${measurement.name}${dashboardLangCode !== langCodes.english ? '' : ' '}${languageApi.getDefaultRes('distribution_chart_title_bettype_top5')}`
      }
      return `${measurement.name}${dashboardLangCode !== langCodes.english ? '' : ' '}${this.titleDistributionChart}`;
  }

  renderDistributionChart() {
    const { distributionData, topDistributions, measurementId, dataMapper } = this._getDistributionChartData();
    if (!distributionData) {
      return null;
    }
    return <DistributionChart
      distributionData={distributionData}
      topDistributions={topDistributions}
      measurementId={measurementId}
      getTitle={this.getTitle}
      tabKey={this.tabKey}
      dataMapper={dataMapper || distributionChartDataMapper}
    />
  }

  renderOverallChart() {
    const { overallMarginData, title } = this._getOverallChartData();
    if (!overallMarginData) {
      return null;
    }
    return <OverallChart
      title={title}
      overallData={overallMarginData}
      dataMapper={overallChartDataMapper}
    />
  }

  renderDifferenceTable() {
    const { measurementDifferenceData, dataMapper } = this._getDifferenceTableData();
    if (!measurementDifferenceData) {
      return null;
    }
    const tableData = dataMapper ? dataMapper(measurementDifferenceData, this.tabKey) : differenceTableDataMapper(measurementDifferenceData, this.tabKey);
    return <ComparisonTable
      {...tableData}
    />;
  }

  renderGrowthChart() {
    const { growthData, measurementId, displayNameMapper } = this._getGrowthChartData();
    if (!growthData) {
      return null;
    }
    const measurement = generalService.getMeasurementNameById(measurementId) || {};
    const { isPeriodGrowthDataType, selectedColumnDrillDown } = this.state;

    const growthChartData = this._getGrowthData() || [];
    const lineChartData = {
      name: languageApi.getDefaultRes('growth_rate'),
      dataSet: growthChartData.map(_ => _.growthPercent)
    };
    const barChartData = this._getBarChartData(growthChartData, measurement, displayNameMapper);
    const labels = isPeriodGrowthDataType ? growthChartData.map(_ => displayNameMapper ? displayNameMapper(_) : _.name)
                 :  growthChartData.map(_ => _.name);
    const title = `${measurement.name}${dashboardLangCode !== langCodes.english ? '' : ' '}${languageApi.getDefaultRes('comparison')}${isPeriodGrowthDataType ? '' :
      ` - ${selectedColumnDrillDown.name} - ${growthChartConsts[selectedColumnDrillDown.datasetIndex].periodName}`}`;
    const leftAxisName = `${measurement.name} ${measurementId === MEASUREMENT_ID.MARGIN ? '(%)' : ''}`;
    const autoScale = this.isAutoScaleGrowthChart;

    return <GrowthChart
      autoScale={autoScale}
      title={title}
      barChartData={barChartData}
      lineChartData={lineChartData}
      labels={labels}
      leftAxisName={leftAxisName}
      showBackIcon={!isPeriodGrowthDataType}
      calculateBarWidth={calculateBarWidthOfGrowthChart}
      backToDefaultChart={() => this.setState({ isPeriodGrowthDataType: true })}
    />;
  }

  _getDrillDownField() {
    const { selectedColumnDrillDown } = this.state;
    const { datasetIndex, id } = selectedColumnDrillDown;
    const { analyticData } = this.props;

    const selectedPeriod = analyticData[growthChartConsts[datasetIndex]['periodField']];
    const field = generalService.getFieldMonthGrowth(id, selectedPeriod);
    return field;
  }

  _getGrowthData() {
    const { isPeriodGrowthDataType } = this.state;
    const { analyticData } = this.props;
    const { growthData } = analyticData;

    if (isPeriodGrowthDataType) {
      return growthData['periodGrowth'];
    } else {
      const field = this._getDrillDownField();
      const drillDownData = growthData['monthGrowth'][field];
      return drillDownData;
    }
  }

  _getBarChartData(growthChartData, measurement, displayNameMapper) {
    const { isPeriodGrowthDataType, selectedColumnDrillDown } = this.state;
    if (isPeriodGrowthDataType) {
      return growthChartConsts.map((_, datasetIndex) => {
        return {
          name: _.periodName,
          dataSet: growthChartData.map(__ => __[_.periodFieldName]),
          events: {
            click: growthChartData.map((periodData, colIndex) => {
              const drillDownData = {
                id: periodData.id,
                name: displayNameMapper ? displayNameMapper(periodData) : periodData.name,
                distributions: periodData[_.distributionFieldName],
                datasetIndex
              }
              return {
                columnIndex: colIndex,
                callBack: () => {
                  this.setState({
                    isPeriodGrowthDataType: false,
                    selectedColumnDrillDown: drillDownData
                  })
                }
              }
            })
          }
        }
      })
    } else {
      return [{
        name: selectedColumnDrillDown.name + ' ' + measurement.name,
        dataSet: growthChartData.map(_ => _.total),
        color: growthChartConsts[selectedColumnDrillDown.datasetIndex].columnColor
      }, { hidden: true, dataSet: [] }];
    }
  }
}
