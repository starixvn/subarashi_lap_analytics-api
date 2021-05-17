import React, { Component } from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';

import BoxInfoMeasureList from '../../components/normal/BoxInfoMeasureList';
import ComparisonTable from '../../components/normal/ComparisonTable';
import GrowthChart from '../../components/charts/GrowthChart';
import ValueChangeIcon from '../../components/normal/ValueChangeIcon';

import { getMajorChangeList } from './lib/constants';
import { generalService } from '../../lib/service';
import { setAnalyticData } from '../../actions/analyticDataAction';
import { defaultData, MEASUREMENT_ID} from '../../lib/constants';
import numberFormat from '../../../../lib/numberFormat';
import languageApi from '../../../../lib/languageApi';
import { dashboardLangCode, langCodes } from '../../../../lib/constants';
import withGA from '../../../../components/HOC/WithGA';

class OverviewTab extends Component {
  constructor(props) {
    super(props);
    this.state = {
      isPeriodGrowthDataType: true,
      selectedDrillDownColumn: 0,
      selectedDrillDownColumnName: ''
    };
  }

  componentDidMount() {
    this.props.setAnalyticData({});
  }

  componentDidUpdate(prevProps) {
    const { isPeriodGrowthDataType } = this.state;
    if(this.props.analyticData !== prevProps.analyticData && !isPeriodGrowthDataType) {
      this.setState({
        isPeriodGrowthDataType: true
      })
    }
  }

  _renderTopChangeTableList() {
    let { topChangeData, measurementId } = this.props.analyticData;
    const majorChangeList = getMajorChangeList(measurementId);

    return topChangeData && majorChangeList.map(_ => <ComparisonTable
      key={'major_change_table_' + _.key}
      {...{
        ..._,
        ...{
          data: _.handlerData ? _.handlerData(topChangeData[_.key]) : topChangeData[_.key]
        }
      }}
      renderRow={(item, index) => <tr key={`major_change_row_${index}_${item.perspectiveName}`}>
        <td><ValueChangeIcon value={item.changePercent} />{item.perspectiveName}</td>
        <td>{numberFormat.formatDecimal(item.total)}</td>
        <td>{item.changePercent + '%'}</td>
      </tr>}
    />);
  }

  _renderBoxInfoMeasureList() {
    let { summaryData, baseCurrency } = this.props.analyticData;
    summaryData = summaryData || [];
    return <BoxInfoMeasureList data={summaryData} currencyUnit={baseCurrency} />;
  }

  _renderGrowthChart() {
    const { measurementId } = this.props.analyticData;
    const { isPeriodGrowthDataType, selectedDrillDownColumnName } = this.state;

    const growthChartData = this._getGrowthChartData();
    const measurement = defaultData.measurements.find(_ => _.id === measurementId) || {};

    const labels = growthChartData.map(_ => _.name);

    const lineChartData = {
      name: languageApi.getDefaultRes('growth_rate'),
      dataSet: growthChartData.map(_ => _.growthPercent)
    };

    const barChartData = [{
      name: measurement.name,
      dataSet: growthChartData.map(_ => _.total),
      events: {
        click: isPeriodGrowthDataType ? growthChartData.map((_, colIndex) => {
          return {
            columnIndex: colIndex,
            callBack: () => {
              this.setState({
                isPeriodGrowthDataType: false,
                selectedDrillDownColumn: colIndex,
                selectedDrillDownColumnName: _.name
              })
            }
          }
        }) : []
      }
    }];

    const title =  `${measurement.name}${dashboardLangCode !== langCodes.english ? '' : ' '}${languageApi.getDefaultRes('comparison')}${isPeriodGrowthDataType ? '' : ' - ' + selectedDrillDownColumnName}`;
    const leftAxisName = `${measurement.name} ${measurementId === MEASUREMENT_ID.MARGIN ? '(%)' : ''}`;

    return <GrowthChart
      autoScale={true}
      getStyle={this._getStyleGrowthChart}
      title={title}
      barChartData={barChartData}
      lineChartData={lineChartData}
      labels={labels}
      leftAxisName={leftAxisName}
      showBackIcon={!isPeriodGrowthDataType}
      backToDefaultChart={() => this.setState({ isPeriodGrowthDataType: true })}
    />;
  }

  _getGrowthChartData() {
    const { isPeriodGrowthDataType, selectedDrillDownColumn } = this.state;
    const { growthData } = this.props.analyticData;
    if (isPeriodGrowthDataType) {
      return growthData['periodGrowth'];
    } else {
      const drillDownData = growthData['monthGrowth'][selectedDrillDownColumn];
      return drillDownData;
    }
  }

  _getStyleGrowthChart = (refSelf) => {
    const { current } = refSelf;
    if (current) {
      const { offsetLeft } = current;
      const { innerWidth } = window;
      const width = offsetLeft < 30 ? innerWidth - 60 : innerWidth - 710;
      return {
        width: width + 'px'
      }
    } else {
      return {}
    }
  }

  render() {
    const { measurementId } = this.props.analyticData;

    return measurementId ? (
      <div className="child-tab overview-tab" style={generalService.getScrollTabStyle()}>
        {this._renderBoxInfoMeasureList()}
        <div className="table-major-list">
          {this._renderTopChangeTableList()}
        </div>
        {this._renderGrowthChart()}
      </div>
    ) : null;
  }
}

function mapDispatchToProps(dispatch) {
  return bindActionCreators(Object.assign({}, {
    setAnalyticData
  }), dispatch);
}

function mapStateToProps(state) {
  const { analyticData } = state;
  return Object.assign({}, {
    analyticData
  });
}

export default connect(mapStateToProps, mapDispatchToProps)(withGA(OverviewTab, 'performance/overview'));
