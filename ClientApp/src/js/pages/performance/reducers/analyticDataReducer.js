import createReducer from '../../../lib/createReducer'
import * as types from '../actions/types.js'

export const analyticData = createReducer({}, {
  [types.SET_ANALYTIC_DATA](state, action) {
    const { mainData, handlerData } = action;
    if (handlerData) {
      const newData = handlerData(state);
      return newData;
    }
    return mainData || {};
  }
});
