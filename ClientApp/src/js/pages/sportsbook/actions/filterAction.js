import * as types from './types';

export const setGlobalFilter = data => {
  return {
    type: types.SET_GLOBAL_FILTER,
    data
  }
}
