import widgetApi from '../../../../../lib/widgetApi';

export const getCombineData = async (filterData) => {
  const param = Object.assign({}, widgetApi.getCommonParam(4), { Data: filterData, Url: '/api/performance/currency' });
  const response = await widgetApi.postData(param);
  if (response && response.data && response.data.isSuccess) {
    return response.data.data;
  }
  return {};
}

export const getAllMonthGrowthData = async (filterData) => {
  const param = Object.assign({}, widgetApi.getCommonParam(4), { Data: filterData, Url: '/api/performance/currency/month-growth-all' });
  const response = await widgetApi.postData(param);
  if (response && response.data && response.data.isSuccess) {
    return response.data.data;
  }
  return {};
}
