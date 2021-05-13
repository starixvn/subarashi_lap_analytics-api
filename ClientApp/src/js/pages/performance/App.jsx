import React, { Component } from 'react';
import { bindActionCreators } from 'redux'
import { ActionCreators } from './actions'
import { connect } from 'react-redux';
import FilterTopPanel from './components/FilterTopPanel';
import TabContainer from './components/TabContainer';
import Loader from '../../components/Loader';
import { generalService } from '../performance/lib/service';
import { tabMenuKey } from '../performance/lib/constants';

class PerformanceDashboard extends Component {
  constructor(props) {
    super(props);
    this.state = {
      selectedTabKey: tabMenuKey.overview
    }
  }

  _onSubmit = () => {
    const { selectedComparePeriods, selectedBaseCurrency, selectedMeasurement } = this.props;
    const { selectedTabKey } = this.state;
    const filterData = generalService.getFilterData({ selectedComparePeriods, selectedBaseCurrency, selectedMeasurement });
    this.props.getAnalyticData(filterData, selectedTabKey);
  }

  _onChangeTab = selectedTabKey => {
    this.setState({
      selectedTabKey
    });
  }

  render() {
    const { isLoading } = this.props;
    const { selectedTabKey } = this.state;
    return (
      <div className="performance-container">
          <Loader isLoading={isLoading} />
          <FilterTopPanel onSubmit={this._onSubmit}/>
          <TabContainer onChangeTab={this._onChangeTab} selectedTabKey={selectedTabKey}/>
      </div >
    );
  }
}

function mapDispatchToProps(dispatch) {
  const { getAnalyticData } = ActionCreators;
  return bindActionCreators({
    getAnalyticData
  }, dispatch);
}

function mapStateToProps(state) {
  const { isLoading, selectedComparePeriods, selectedBaseCurrency, selectedMeasurement } = state;
  return Object.assign({}, {
    isLoading,
    selectedComparePeriods,
    selectedBaseCurrency,
    selectedMeasurement
  });
}
export default connect(mapStateToProps, mapDispatchToProps)(PerformanceDashboard);
