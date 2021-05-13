import * as types from './types';
import { setLoading } from './loadingAction';
import { factoryApi } from '../lib/api';

export const getAnalyticData = (filterData, tabKey) => {
  return async (dispatch) => {
    dispatch(setLoading(true));
    try {
      const mainData = await factoryApi[tabKey].mainApi(filterData);
      dispatch(setAnalyticData({ mainData }));

      dispatch(getLazyData(mainData, filterData, tabKey));
    } catch (error) {
      console.log('error', error.message)
    }
    dispatch(setLoading(false));
  }
}

const getLazyData = (mainData, filterData, tabKey) => {
  const lazyApi = factoryApi[tabKey].lazyApi;
  if(!lazyApi) {
    return { type: null };
  }

  return async (dispatch) => {
    try {
      const data = await lazyApi(mainData, filterData);
      dispatch(setAnalyticData(data));
    } catch (error) {
      console.log('error', error.message)
    }
  }
}

export const setAnalyticData = ({ mainData, handlerData }) => {
  return {
    type: types.SET_ANALYTIC_DATA,
    ...{
      mainData,
      handlerData
    }
  };
}
