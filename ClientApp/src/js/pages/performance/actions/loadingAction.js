import * as types from './types';

export const setLoading = data => {
  return {
    type: types.SET_LOADING,
    data
  }
}
