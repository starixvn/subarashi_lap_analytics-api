import axios from 'axios';
import asyncLock from 'async-lock';

const locker = new asyncLock();
export default class widgetApi {
  static getCommonParam(type, isCrossSite = true) {
    return {
      IsCrossSite: isCrossSite,
      Data: {},
      Url: '',
      Type: type
    }
  }

  static async _fetchTokenFromLap(action, sysType = 4) {
    var tokenUrl = window.TokenUrl + "&_=" + (new Date()).getTime();
    tokenUrl = tokenUrl.replace("{0}", encodeURIComponent(action));
    tokenUrl = tokenUrl.replace("{1}", sysType)
    const tokenFromLap = await axios.get(tokenUrl);
    return tokenFromLap.data;
  }

  static _fetchAccessToken(action, sysType) {
    return locker.acquire(sysType, async () => {
      if (!window.top['analytics_' + sysType]) {
        const { ApiUrl, Token } = await this._fetchTokenFromLap(action, sysType);
        const response = await axios.post(`${ApiUrl}/Api/Auth/Token`, { token: Token });
        const { status, data } = response;
        if (status === 200 && data.isSuccess) {
          window.top['analytics_' + sysType] = {
            accessToken: data.accessToken,
            apiUrl: ApiUrl
          }
        }
      }
      return window.top['analytics_' + sysType];
    })
  }

  static async _fetchDataWithToken(type, action, apiHandler, isRetry = false) {
    const { accessToken, apiUrl } = await this._fetchAccessToken(action, type);
    const dataUrl = apiUrl + action
    try {
      const response = await apiHandler(dataUrl, accessToken);
      return response;
    }
    catch (error) {
      if (error.response.status === 401 && !isRetry) {
        window.top['analytics_' + type] = null;
        return this._fetchDataWithToken(type, action, apiHandler, true);
      }
      return null;
    }
  }


  static async fetchData(param) {
    var data = param.Data || {};
    var type = param.Type || 4;
    if (param.IsCrossSite) {
      return this._fetchDataWithToken(type, param.Url, (dataUrl, accessToken) => {
        return axios.get(dataUrl, {
          params: data,
          headers: {
            'Authorization': `Bearer ${accessToken}`,
            'X-Requested-With': 'XMLHttpRequest',
            'Pragma': 'no-cache'
          }
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
    return this._fetchDataWithToken(type, param.Url, (dataUrl, accessToken) => {
      return axios.post(dataUrl, postContent, {
        headers: {
          'Authorization': `Bearer ${accessToken}`,
          'X-Requested-With': 'XMLHttpRequest'
        }
      });
    });
  }
}
