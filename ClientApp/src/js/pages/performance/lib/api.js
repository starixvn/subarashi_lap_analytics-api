import { tabMenuKey } from '../lib/constants';
import * as overviewApi from '../tabs/overview/lib/api';
import * as currencyApi from '../tabs/currency/lib/api';
import * as productApi from '../tabs/product/lib/api';
import * as platformApi from '../tabs/platform/lib/api';
import * as bettypeApi from '../tabs/bettype/lib/api';

const overviewApiFactory = {
  mainApi: filterData => {
    return overviewApi.getCombineData(filterData);
  },
  lazyApi: null
}

const currencyApiFactory = {
  mainApi: filterData => {
    return currencyApi.getCombineData(filterData);
  },
  lazyApi: async (mainData, filterData) => {
    const topDistributions = mainData.topDistributions;
    const lazyFilterData = {
      ...filterData,
      ...{
        topDistributions
      }
    }
    const responseData = await currencyApi.getAllMonthGrowthData(lazyFilterData);
    const data = {
      handlerData: state => {
        state['growthData']['monthGrowth'] = responseData
        return state;
      }
    }
    return data;
  }
}

const productApiFactory = {
  mainApi: filterData => {
    return productApi.getCombineData(filterData);
  },
  lazyApi: async (mainData, filterData) => {
    const topDistributions = mainData.topDistributions;
    const lazyFilterData = {
      ...filterData,
      ...{
        topDistributions
      }
    }
    const responseData = await productApi.getAllMonthGrowthData(lazyFilterData);
    const data = {
      handlerData: state => {
        state['growthData']['monthGrowth'] = responseData
        return state;
      }
    }
    return data;
  }
}

const platformApiFactory = {
  mainApi: filterData => {
    return platformApi.getCombineData(filterData);
  },
  lazyApi: async (mainData, filterData) => {
    const topDistributions = mainData.topDistributions;
    const lazyFilterData = {
      ...filterData,
      ...{
        topDistributions
      }
    }
    const responseData = await platformApi.getAllMonthGrowthData(lazyFilterData);
    const data = {
      handlerData: state => {
        state['growthData']['monthGrowth'] = responseData
        return state;
      }
    }
    return data;
  }
}

const bettypeApiFactory = {
  mainApi: filterData => {
    return bettypeApi.getCombineData(filterData);
  },
  lazyApi: async (mainData, filterData) => {
    const topDistributions = mainData.topDistributions;
    const lazyFilterData = {
      ...filterData,
      ...{
        topDistributions
      }
    }
    const responseData = await bettypeApi.getAllMonthGrowthData(lazyFilterData);
    const data = {
      handlerData: state => {
        state['growthData']['monthGrowth'] = responseData
        return state;
      }
    }
    return data;
  }
}

export const factoryApi = {
  [tabMenuKey.overview]: overviewApiFactory,
  [tabMenuKey.currency]: currencyApiFactory,
  [tabMenuKey.product]: productApiFactory,
  [tabMenuKey.platform]: platformApiFactory,
  [tabMenuKey.bettype]: bettypeApiFactory
}
