import createReducer from '../lib/createReducer.js'
import * as types from '../actions/types.js'
export const currencyInfo = createReducer({}, {
  [types.SET_CURRENCY_INFO](state, action) {
    return action.data;
  }
});

export const selectedBaseCurrency = createReducer({}, {
  [types.SET_BASE_CURRENCY](state, action) {
    return action.data;
  }
});
