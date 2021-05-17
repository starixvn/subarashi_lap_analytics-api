import { combineReducers } from 'redux';
import * as currency from '../../../reducers/currencyReducer';
import * as filter from './filterReducer';
import * as loading from './loadingReducer';
import * as analyticData from './analyticDataReducer';

export default combineReducers(Object.assign(
  loading,
  currency,
  filter,
  analyticData
));
