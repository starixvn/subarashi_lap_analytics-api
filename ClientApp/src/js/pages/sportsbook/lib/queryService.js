
import { apiUrl, defaultResponseData } from './constants';
import widgetApi from '../../../lib/widgetApi';

export default class QueryService {
  static async getDataBySport(params, after) {
    const response = await this.fetchData(params, apiUrl.bySport, defaultResponseData);
    after && after(response)
    return response;
  }

  static async getDataByLeagueSoccer(params, after) {
    const response = await this.fetchData(params, apiUrl.byLeagueSoccer, defaultResponseData);
    after && after(response)
    return response;
  }

  static async getDataByLeagueBasketball(params, after) {
    const response = await this.fetchData(params, apiUrl.byLeagueBasketball, defaultResponseData);
    after && after(response)
    return response;
  }

  static async getDataByBettypeSoccer(params, after) {
    const response = await this.fetchData(params, apiUrl.byBettypeSoccer, defaultResponseData);
    after && after(response)
    return response;
  }

  static async getDataByBettypeBasketball(params, after) {
    const response = await this.fetchData(params, apiUrl.byBettypeBasketball, defaultResponseData);
    after && after(response)
    return response;
  }

  static async getDataByCurrency(params, after) {
    const response = await this.fetchData(params, apiUrl.byCurrency, defaultResponseData);
    after && after(response)
    return response;
  }

  static async getDataByPlatform(params, after) {
    const response = await this.fetchData(params, apiUrl.byPlatform, defaultResponseData);
    after && after(response)
    return response;
  }

  static fetchData = async (params, url, defaultData = null) => {
    const _params = Object.assign({}, widgetApi.getCommonParam(4), { Data: params, Url: url });
    let responseData = defaultData;
    try {
      const response = await widgetApi.postData(_params);
      if (response.status === 200 && response.data.isSuccess) {
        responseData = response.data.data;
      }
    }
    catch (e) {
      console.log(e)
    }

    return responseData;
  }
}
