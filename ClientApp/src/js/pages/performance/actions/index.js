import * as currencyAction from '../../../actions/currencyAction'
import * as filterAction from './filterAction';
import * as analyticDataAction from './analyticDataAction';
import * as loadingAction from './loadingAction';

export const ActionCreators = Object.assign(
  currencyAction,
  filterAction,
  loadingAction,
  analyticDataAction
);
