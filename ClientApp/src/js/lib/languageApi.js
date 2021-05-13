
import { cs } from '../resources/resource.cs';
import { en } from '../resources/resource.en';
import { dashboardLangCode, langCodes } from './constants';

export const langCodeMapper = {
  [langCodes.english]: 'en-US',
  [langCodes.simplifiedChinese]: 'zh-CN'
}

export default class languageApi {
  static getRes(langcode, resKey) {
    switch (langcode) {
      case langCodes.english:
        return en[resKey];
      case langCodes.simplifiedChinese:
        return cs[resKey]
      default:
        return en[resKey];
    }
  }
  static getDefaultRes(resKey) {
    return languageApi.getRes(dashboardLangCode, resKey) || resKey;
  }
}
