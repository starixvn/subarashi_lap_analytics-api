
import * as currencyAction from '../../../actions/currencyAction';
import * as dateRangeAction from '../../../actions/dateRangeAction';
import * as filterAction from './filterAction';

export const ActionCreators = Object.assign(
  currencyAction,
  dateRangeAction,
  filterAction
);
