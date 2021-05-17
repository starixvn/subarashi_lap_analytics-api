import { combineReducers } from 'redux';
import * as currency from './currencyReducer'

export default combineReducers(Object.assign(
  currency
));
