import createReducer from '../../../lib/createReducer'
import * as types from '../actions/types.js'

export const isLoading = createReducer({}, {
  [types.SET_LOADING](state, action) {
    return action.data;
  }
});
