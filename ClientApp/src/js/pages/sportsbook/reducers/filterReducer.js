import createReducer from '../../../lib/createReducer'
import * as types from '../actions/types.js'

export const globalFilter = createReducer(null, {
  [types.SET_GLOBAL_FILTER](state, action) {
    return action.data;
  }
});
