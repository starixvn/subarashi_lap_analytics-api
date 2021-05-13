import { combineReducers } from 'redux';
import * as currency from '../../../reducers/currencyReducer';
import * as filterReducer from './filterReducer';
import * as dateRangeReducer from '../../../reducers/dateRangeReducer';

export default combineReducers(Object.assign(
  currency,
  dateRangeReducer,
  filterReducer
));
