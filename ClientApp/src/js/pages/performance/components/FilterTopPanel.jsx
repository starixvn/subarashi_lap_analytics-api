import React from 'react';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions';
import languageApi from '../../../lib/languageApi';
import { connect } from 'react-redux';
import { defaultData } from '../lib/constants'
import CoupleMonthRangePicker from '../../../components/CoupleMonthRangePicker';

class FilterTopPanel extends React.PureComponent {
  constructor(props) {
    super(props);
  }

  componentDidMount() {
    this.props.getCurrencyInfo();
  }

  _onChangeBaseCurrency = e => {
    this.props.setBaseCurrency(e.target.value);
  }

  _onChangeMeasurement = e => {
    const value = defaultData.measurements.find(_ => _.id == e.target.value);
    this.props.setMeasurement(value);
  }

  _onChangeRangeOfTwoPeriod = ({ currentPeriod, previousPeriod }) => {
    this.props.setComparePeriods({ currentPeriod, previousPeriod });
  }

  _onSubmit = () => {
    this.props.onSubmit && this.props.onSubmit();
  }

  render() {
    const { selectedComparePeriods } = this.props;

    return (
      <div className="filter-top-panel">
        <div className="filter-item">
          <CoupleMonthRangePicker onChangeRangeOfTwoPeriod={this._onChangeRangeOfTwoPeriod}
            currentPeriod={selectedComparePeriods.currentPeriod}
            previousPeriod={selectedComparePeriods.previousPeriod}
          />
        </div>
        <div className="filter-item">
          <div className="label">{languageApi.getDefaultRes("Based")}</div>
          <select
            className="select-filter"
            onChange={(e) => this._onChangeBaseCurrency(e)}
            value={this.props.selectedBaseCurrency}>
            {this.props.currencyInfo.BaseCurrency.map((item, i) => {
              return <option key={i} value={item}>{item}</option>
            })}
          </select>
        </div>
        <div className="filter-item">
          <div className="label">{languageApi.getDefaultRes("measurement")}</div>
          <select
            className="select-filter"
            onChange={(e) => this._onChangeMeasurement(e)}
            value={this.props.selectedMeasurement.id}>
            {defaultData.measurements.map((item, i) => {
              return <option key={item.id} value={item.id}>{item.name}</option>
            })}
          </select>
        </div>
        <button className="btn btn-confirm" onClick={this._onSubmit}>{languageApi.getDefaultRes("submit")}</button>
      </div>
    );
  }
}

function mapDispatchToProps(dispatch) {
  const { getCurrencyInfo, setBaseCurrency, setMeasurement, setComparePeriods } = ActionCreators;
  return bindActionCreators({
    getCurrencyInfo,
    setBaseCurrency,
    setMeasurement,
    setComparePeriods
  }, dispatch);
}

function mapStateToProps(state) {
  const { selectedBaseCurrency, selectedMeasurement, currencyInfo, selectedComparePeriods } = state;
  return Object.assign({}, {
    selectedBaseCurrency,
    selectedMeasurement,
    selectedComparePeriods,
    currencyInfo
  });
}

export default connect(mapStateToProps, mapDispatchToProps)(FilterTopPanel)
