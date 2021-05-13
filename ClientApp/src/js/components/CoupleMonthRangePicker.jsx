import React from 'react';
import MonthRangePicker, { isPeriodEqual } from './MonthRangePicker';
import { getDistanceBetweenPeriod, getPreviousPeriod, getPreviousYear, getPeriodByNumberOfLastMonths, getPeriodByLastQuarter } from '../lib/dateTimeHelper';
import { getCurrentTime } from '../lib/constants';
import languageApi from '../lib/languageApi';

const previousPeriodQuickSelections = [
  {
    label: languageApi.getDefaultRes('previous_period'),
    onSelect: selectedCurrentPeriod => getPreviousPeriod(selectedCurrentPeriod)
  },
  {
    label: languageApi.getDefaultRes('previous_year'),
    onSelect: selectedCurrentPeriod => getPreviousYear(selectedCurrentPeriod)
  }
]

const currentPeriodQuickSelections = [
  {
    label: languageApi.getDefaultRes('last_month'),
    onSelect: currentTime => getPeriodByNumberOfLastMonths(currentTime, 1)
  },
  {
    label: languageApi.getDefaultRes('last_quarter'),
    onSelect: currentTime => getPeriodByLastQuarter(currentTime)
  },
  {
    label: languageApi.getDefaultRes('last_3_months'),
    onSelect: currentTime => getPeriodByNumberOfLastMonths(currentTime, 3)
  },
  {
    label: languageApi.getDefaultRes('last_6_months'),
    onSelect: currentTime => getPeriodByNumberOfLastMonths(currentTime, 6)
  }
]

const getLimitPeriod = period => ({
    maxFrom: {
      month: period.from.month + 1,
      year: period.from.year
    }
})

export default class CoupleMonthRangePicker extends React.PureComponent {
  constructor(props) {
    super(props);
    const { currentPeriod, previousPeriod } = this.props;
    this.state = {
      currentPeriod: currentPeriod,
      previousPeriod: previousPeriod
    }
    this._previousPeriodLimitFrom = getLimitPeriod(previousPeriod);
  }

  _onChangeRangeCurrentPeriod = selectedRange => {
    const { onChangeRangeOfTwoPeriod } = this.props;
    const previousPeriod = getPreviousPeriod(selectedRange);
    const periods = {
      currentPeriod: selectedRange,
      previousPeriod
    }

    this._previousPeriodLimitFrom = getLimitPeriod(previousPeriod);

    this.setState(periods, () => onChangeRangeOfTwoPeriod && onChangeRangeOfTwoPeriod(periods));
  }

  _onChangeRangePreviousPeriod = selectedRange => {
    const { onChangeRangeOfTwoPeriod } = this.props;
    const periods = {
      currentPeriod: this.state.currentPeriod,
      previousPeriod: selectedRange
    }
    this.setState(periods, () => onChangeRangeOfTwoPeriod && onChangeRangeOfTwoPeriod(periods));
  }

  _renderCurrentPeriodQuickSelection = self => {
    return currentPeriodQuickSelections
      .map((_, index) => {
        const isSelected = isPeriodEqual(_.onSelect(getCurrentTime()), self.state);
        return <div key={`cp_selection_${index}`} className={`quick-selection ${isSelected ? 'quick-selection-active' : ''}`}
          onClick={() => {
            const currentPeriod = _.onSelect(getCurrentTime());
            self.setState(currentPeriod);
          }}>{_.label}
        </div>
      });
  }

  _renderPreviousPeriodQuickSelection = self => {
    return previousPeriodQuickSelections
      .map((_, index) => {
        const { currentPeriod } = this.state;
        const isSelected = isPeriodEqual(_.onSelect(currentPeriod), self.state);
        return <div key={`pp_selection_${index}`} className={`quick-selection ${isSelected ? 'quick-selection-active' : ''}`}
        onClick={() => {
          const previousPeriod = _.onSelect(currentPeriod);
          self.setState(previousPeriod)
        }}>{_.label}
      </div>
      });
  }

  render() {
    const { currentPeriod, previousPeriod } = this.state;
    const rangeTo = getDistanceBetweenPeriod(currentPeriod) + 1;
    const { maxFrom } = this._previousPeriodLimitFrom;

    return (<div className="couple-month-range">
      <div className="month-range-item">
        <div className="label">{languageApi.getDefaultRes("current_period")}</div>
        <MonthRangePicker
          from={currentPeriod.from}
          to={currentPeriod.to}
          onChangeRange={this._onChangeRangeCurrentPeriod}
          quickSelections={this._renderCurrentPeriodQuickSelection} />
      </div>
      <div className="month-range-item">
        <div className="label">{languageApi.getDefaultRes("previous_period")}</div>
        <MonthRangePicker
          isDisableTo={true}
          range={rangeTo}
          maxMonthFrom={maxFrom.month}
          maxYearFrom={maxFrom.year}
          from={previousPeriod.from}
          to={previousPeriod.to}
          onChangeRange={this._onChangeRangePreviousPeriod}
          quickSelections={this._renderPreviousPeriodQuickSelection} />
      </div>
    </div>);
  }
}

