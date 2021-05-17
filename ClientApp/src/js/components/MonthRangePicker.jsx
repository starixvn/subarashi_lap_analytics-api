import React from 'react';
import PropTypes from 'prop-types';
import { YearSelector, MonthList } from '@9softstudio/react-monthpicker';
import { defaultFormatFunc, addMonths } from '../lib/dateTimeHelper';
import languageApi from '../lib/languageApi';
import { getCurrentTime } from '../lib/constants';

const isTimeEqual = (time1, time2) => {
  return time1.year === time2.year && time1.month === time2.month;
}

export const isPeriodEqual = (period1, period2) => {
  return isTimeEqual(period1.from, period2.from) && isTimeEqual(period1.to, period2.to);
}

const getMaxDateValid = (range = -1) => {
  const currentDate = getCurrentTime();
  let maxDate = new Date(currentDate.year, currentDate.month);
  maxDate = new Date(maxDate.setMonth(maxDate.getMonth() + range));
  return {
    month: maxDate.getMonth(),
    year: maxDate.getFullYear()
  }
}

const getMaxDateTo = ({ year, month }, range) => {
  let maxDate = new Date(year, month);
  maxDate = new Date(maxDate.setMonth(maxDate.getMonth() + range - 1));
  const maxDateValid = getMaxDateValid();
  const currentDate = new Date(maxDateValid.year, maxDateValid.month);

  if(maxDate > currentDate) {
    maxDate = currentDate;
  }

  return {
    maxMonthTo: maxDate.getMonth() + 1,
    maxYearTo:  maxDate.getFullYear()
  }
}

export default class MonthRangePicker extends React.PureComponent {
  constructor(props) {
    super(props);
    const { open, from, to } = this.props;
    this.state = {
      open: open,
      from: from,
      to: to
    }
    this.container = React.createRef();
  }

  static propTypes = {
    open: PropTypes.bool,
    minYearFrom: PropTypes.number,
    maxYearFrom: PropTypes.number,
    minMonthFrom:PropTypes.number,
    maxMonthFrom: PropTypes.number,

    monthNames: PropTypes.arrayOf(PropTypes.arrayOf(PropTypes.string)),
    hasRange: PropTypes.bool,
    range: PropTypes.number,
    onSelect: PropTypes.func,
    onFormat: PropTypes.func,
    isReadonly: PropTypes.bool,
    iconElement: PropTypes.any,
    enable: PropTypes.bool,
    quickSelections: PropTypes.func,

    isDisableTo: PropTypes.bool
  }

  static defaultProps = {
    open: false,
    onSelect: () => { },
    onFormat: defaultFormatFunc,
    isReadonly: true,
    enable: true,

    minYearFrom: getCurrentTime().year - 4,
    maxYearFrom: getMaxDateValid().year,
    minMonthFrom: 1,
    maxMonthFrom: getCurrentTime().month || 12,

    hasRange: true,
    range: 12,
    isDisableTo: false
  }

  static getDerivedStateFromProps(nextProps, prevState) {
    if(!prevState.open) {
      if(!isPeriodEqual({ from: nextProps.from, to: nextProps.to },
         { from: prevState.from, to: prevState.to })) {
        return {
          from: nextProps.from,
          to: nextProps.to
        }
      }
    }
    return {};
  }

  componentDidMount() {
    document.addEventListener('click', this._handleClickOutSide, true);
  }

  componentWillUnmount() {
    document.removeEventListener('click', this._handleClickOutSide, true);
  }

  _toggle = () => {
    if (this.props.enable) {
      this.setState(prevState => {
        !prevState.open && this._saveCurrentRange();
        return {
          open: !prevState.open
        }
      })
    }
  }

  _onChangeYearFrom = selectedYear => {
    let { from } = this.state;
    from = {
      ...from,
      ...{
        dropdownYear: selectedYear
      }
    }

    this.setState({ from });
  }

  _onChangeMonthFrom = selectedMonth => {
    let { from, to } = this.state;
    from = {
      ...from,
      ...{
        month: selectedMonth,
        year: from.dropdownYear || from.year
      }
    };

    to = this._getToValueOnChaneMonthFrom(from);

    this.setState({
      from,
      to
    });
  }

  _getToValueOnChaneMonthFrom(from) {
    const { isDisableTo, range } = this.props;
    if(isDisableTo) {
      let date = new Date(from.year, from.month);
      date = addMonths(date, range - 1);
      return {
        month: date.getMonth(),
        year: date.getFullYear(),
        dropdownYear: date.getFullYear()
      }
    } else {
      return {
        ...from
      }
    }
  }

  _onChangeYearTo = selectedYear => {
    let { to } = this.state;
    to = {
      ...to,
      ...{
        dropdownYear: selectedYear
      }
    }
    this.setState({ to });
  }

  _onChangeMonthTo = selectedMonth => {
    let { to } = this.state;
    to = {
      ...to,
      ...{
        month: selectedMonth,
        year: to.dropdownYear || to.year
      }
    }
    this.setState({ to });
  }

  _handleClickOutSide = (event) => {
    const containerElement = this.container.current;
    const clickOutside = containerElement && !containerElement.contains(event.target);
    if (clickOutside && this.state.open) {
      this._cancel();
    }
  }

  _onConfirmChangeRange = () => {
    const { onChangeRange } = this.props;
    const { from, to } = this.state;

    this.setState({
      ...from,
      ...to,
      ...{
        open: false,
      }
    }, () => onChangeRange && onChangeRange({ from, to }))
  }

  _cancel = () => {
    this.setState({
      ...this.currentRange,
      open: false
    })
  }

  _saveCurrentRange() {
    const { from, to } = this.state;
    this.currentRange = { from, to };
  }

  _getDisplayText() {
    const { from, to } = this.state;
    const { onFormat } = this.props;
    return `${onFormat(from.month + 1, from.year)}  -  ${onFormat(to.month + 1, to.year)}`;
  }

  render() {
    const { open, from, to } = this.state;
    const { monthNames, hasRange, isReadonly, iconElement, enable, range, quickSelections, isDisableTo } = this.props;
    const { minMonthFrom, maxMonthFrom, maxYearFrom, minYearFrom } = this.props;
    const maxDateTo = getMaxDateTo(from, range);
    const { maxYearTo, maxMonthTo } = maxDateTo;

    const displayText = this._getDisplayText();

    return (
      <div className="rmp-main-container">
        <div className={enable ? 'mp-form-container' : 'mp-form-container disable'} onClick={this._toggle}>
          <input type="text" className="mp-form-input" value={displayText} readOnly={isReadonly} />
          {iconElement}
        </div>
        <div className="range-mp-container" style={{ display: open ? 'block' : 'none' }} ref={this.container}>
          <div className="quick-selection-container">
            {quickSelections && quickSelections(this)}
            <div>
              <button className="btn btn-confirm" onClick={this._onConfirmChangeRange}>OK</button>
              <button className="btn btn-cancel m-l-5" onClick={this._cancel}>{languageApi.getDefaultRes('cancel')}</button>
            </div>
          </div>
          <div className="mp-container mp-from" >
            <YearSelector minYear={minYearFrom} maxYear={maxYearFrom}
              selectedDropdownYear={from.dropdownYear || from.year}
              onSelect={this._onChangeYearFrom} />
            <MonthList monthNames={monthNames} hasRange={hasRange}
              selectedDropdownYear={from.dropdownYear || from.year}
              selectedMonth={from.month} selectedYear={from.year}
              minYear={minYearFrom} maxYear={maxYearFrom}
              minMonth={minMonthFrom} maxMonth={maxMonthFrom}
              onSelect={this._onChangeMonthFrom} />
          </div>
          <div className={`mp-container mp-to ${isDisableTo ? 'disabled' : ''}`}>
            <YearSelector minYear={from.year} maxYear={maxYearTo}
              selectedDropdownYear={to.dropdownYear || to.year}
              onSelect={this._onChangeYearTo} />
            <MonthList
              monthNames={monthNames} hasRange={hasRange}
              selectedDropdownYear={to.dropdownYear || to.year}
              selectedMonth={to.month} selectedYear={to.year}
              minYear={from.year} maxYear={isDisableTo ? 1 : maxYearTo}
              minMonth={from.month + 1} maxMonth={maxMonthTo}
              onSelect={this._onChangeMonthTo} />
          </div>
        </div>
      </div>
    )
  }
}
