import * as types from './types.js'
import { CurrenyListApiUrl } from '../lib/constants'
import widgetApi from '../lib/widgetApi'

export const getCurrencyInfo = () => {
  return (dispatch) => {
    var param = {
      Url: CurrenyListApiUrl,
      IsCrossSite: false
    };
    widgetApi.fetchData(param)
      .then((response) => {
        response
          .data
          .Currency
          .unshift({ Currency: "All", CurrencyId: "0" });
        dispatch(setCurrencyInfo({ Currency: response.data.Currency, BaseCurrency: response.data.BaseCurrency }));
      })
      .catch(function (error) {
        console.log("error:", error);
      });
  }
}

const setCurrencyInfo = (newData) => {
  return {
    type: types.SET_CURRENCY_INFO,
    data: newData,
  }
};

export const setBaseCurrency = (newData) =>{
 return {
  type: types.SET_BASE_CURRENCY,
  data: newData,
 }
}
