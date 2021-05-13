import createReducer from '../../../lib/createReducer'
import * as types from '../actions/types.js'

export const selectedMeasurement = createReducer({}, {
  [types.SET_MEASUREMENT](state, action) {
    return action.data;
  }
});

export const selectedComparePeriods = createReducer({}, {
  [types.SET_COMPARE_PERIODS](state, action) {
    return action.data;
  }
});
