import * as types from './types';

export const setMeasurement = data =>
{
  return {
    type: types.SET_MEASUREMENT,
    data
  }
}

export const setComparePeriods = data => {
  return {
    type: types.SET_COMPARE_PERIODS,
    data
  }
}
