import React, { useEffect } from 'react';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions';
import languageApi from '../../../lib/languageApi';
import { connect } from 'react-redux';
import { DateRange } from '@legion-lib/react-daterangepicker';
import { convertDateToString } from '../../../lib/dateTimeHelper';
import { dateConstraint } from '../lib/constants';

const FormFilter = props => {
  const { selectedBaseCurrency, currencyInfo, dateRange, getCurrencyInfo, setBaseCurrency, setDateRange, setGlobalFilter } = props;

  useEffect(() => {
    getCurrencyInfo();
  }, []);

  const _onChangeBaseCurrency = e => {
    setBaseCurrency(e.target.value);
  }

  const _onChangeDateRange = (date, isValidMonthRange = true) => {
    if (isValidMonthRange) {
      setDateRange(date);
    }
    else {
      alert(languageApi.getDefaultRes('DateRangeExceedMsg').replace("{0}", dateConstraint.limitedMonthRange))
    }
  }

  const _renderDateRange = () => {
    return (<DateRange
      startDate={dateRange.fromDate} endDate={dateRange.toDate}
      minDate={dateConstraint.minDate} maxDate={dateConstraint.maxDate}
      today={window.today}
      limitedMonthRange={dateConstraint.limitedMonthRange}
      onChange={_onChangeDateRange}
      langHandler={key => languageApi.getDefaultRes(key.toLowerCase())}
    />)
  }

  const _renderBaseCurrencyList = () => {
    return (
      <select
        className="select-filter"
        onChange={_onChangeBaseCurrency}
        value={selectedBaseCurrency}>
        {currencyInfo.BaseCurrency.map((item, i) => {
          return <option key={i} value={item}>{item}</option>
        })}
      </select>
    )
  }

  const _onSubmit = () => {
    setGlobalFilter({
      baseCurrency: selectedBaseCurrency,
      fromDate: convertDateToString(dateRange.fromDate),
      toDate: convertDateToString(dateRange.toDate)
    })
  }

  return (
    <div className="filter-top-panel">
      <div className="filter-item">
        <div className="label">{languageApi.getDefaultRes("date")}</div>
        {_renderDateRange()}
      </div>
      <div className="filter-item">
        <div className="label">{languageApi.getDefaultRes("Based")}</div>
        {_renderBaseCurrencyList()}
      </div>
      <button className="btn btn-confirm" onClick={_onSubmit}>{languageApi.getDefaultRes("submit")}</button>
    </div>
  );
}

function mapDispatchToProps(dispatch) {
  const { getCurrencyInfo, setBaseCurrency, setDateRange, setGlobalFilter } = ActionCreators;
  return bindActionCreators({
    getCurrencyInfo,
    setBaseCurrency,
    setDateRange,
    setGlobalFilter
  }, dispatch);
}

function mapStateToProps(state) {
  const { selectedBaseCurrency, currencyInfo, dateRange } = state;
  return Object.assign({}, {
    selectedBaseCurrency,
    currencyInfo,
    dateRange
  });
}

export default connect(mapStateToProps, mapDispatchToProps)(React.memo(FormFilter))
