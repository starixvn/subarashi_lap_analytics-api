import { getCurrentTime } from '../../../lib/constants';
import { getPeriodByNumberOfLastMonths, getPreviousPeriod } from '../../../lib/dateTimeHelper';
import languageApi from '../../../lib/languageApi';

export const MEASUREMENT_ID = {
  'TURNOVER': 1,
  'GROSS_PROFIT': 2,
  'MARGIN': 3
}

export const defaultData = {
  measurements: [
    { id: MEASUREMENT_ID.TURNOVER, name: languageApi.getDefaultRes('turnover') },
    { id: MEASUREMENT_ID.GROSS_PROFIT, name: languageApi.getDefaultRes('gross_profit') },
    { id: MEASUREMENT_ID.MARGIN, name: languageApi.getDefaultRes('margin') },
  ],
  getComparePeriods: () => {
    const currentPeriod = getPeriodByNumberOfLastMonths(getCurrentTime(), 1);
    const previousPeriod = getPreviousPeriod(currentPeriod);
    return {
      currentPeriod,
      previousPeriod
    }
  }
}

export const tabMenuKey = {
  overview: 'overview',
  currency: 'currency',
  product: 'product',
  bettype: 'bettype',
  platform: 'platform'
}
