import axios from 'axios';
import { langCodeMapper } from './languageApi';
import { dashboardLangCode } from './constants';

export default class widgetApi {
  static getCommonParam(type, isCrossSite = true) {
    return {
      IsCrossSite: isCrossSite,
      Data: {},
      Url: '',
      Type: type
    }
  }

  static _getAuthToken(action, type = 4) {
    var tokenUrl = window.TokenUrl + "&_=" + (new Date()).getTime();
    tokenUrl = tokenUrl.replace("{0}", encodeURIComponent(action));
    tokenUrl = tokenUrl.replace("{1}", type)
    return axios.get(tokenUrl, {

    });
  }

  static fetchData(param) {
    var data = param.Data || {};
    var type = param.Type || 4;
    if (param.IsCrossSite) {
      return widgetApi._getAuthToken(param.Url, type).then((response) => {
        data['X-Auth-Token'] = response.data.Token
        const dataUrl = response.data.ApiUrl + param.Url
        return axios.get(dataUrl, {
          params: data
        });
      });
    } else {
      var lapRootUrl = window.HostUrl;
      const lapApi = lapRootUrl.replace("SessionIsAlive", param.Url)
      return axios.get(lapApi, {
        params: data
      });
    }
  }

  static async postData(param) {
    if (!param.IsCrossSite) return new Promise((resolve, reject) => {
      resolve({});
    });
    var postContent = param.Data || {};
    var type = param.Type || 1;
    let response = await widgetApi._getAuthToken(param.Url, type);
    const dataUrl = response.data.ApiUrl + param.Url
    return axios.post(dataUrl, postContent, {
      headers: {
        'X-Auth-Token': response.data.Token,
        'X-Requested-With': 'XMLHttpRequest',
        'Accept-Language': langCodeMapper[dashboardLangCode]
      }
    });
  }
}
