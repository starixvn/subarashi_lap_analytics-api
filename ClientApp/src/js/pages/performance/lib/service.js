import { defaultFormatFunc } from '../../../lib/dateTimeHelper';
import { defaultData } from '../lib/constants';
import { getCurrencyDisplayName  } from '../../../lib/currencyHelper';
export const generalService = {
  getFilterData: ({ selectedMeasurement, selectedBaseCurrency, selectedComparePeriods }) => {
    return {
      measurement: selectedMeasurement,
      baseCurrencyName: selectedBaseCurrency,
      currentPeriod: _convertPeriodToMetaModel(_convertPeriod(selectedComparePeriods.currentPeriod)),
      previousPeriod: _convertPeriodToMetaModel(_convertPeriod(selectedComparePeriods.previousPeriod)),
    }
  },
  getFilterDataMonthGrowth: ({ analyticData, selectedPeriod }) => {
    return {
      baseCurrencyName: analyticData.baseCurrency,
      currentPeriod: selectedPeriod,
      previousPeriod: analyticData.previousPeriod,
      measurement: { id: analyticData.measurementId }
    }
  },
  getFieldMonthGrowth: (selectedCategory, selectedPeriod) => {
    return selectedCategory + '_' + selectedPeriod.fromMonth + '_' + selectedPeriod.toMonth;
  },
  getMeasurementNameById: measurementId => {
    return defaultData.measurements.find(_ => _.id === measurementId)
  },
  getCurrencyDisplayName: getCurrencyDisplayName,
  getScrollTabStyle: () => {
    const { innerHeight } = window;
    return {
      maxHeight: (innerHeight - 200) + 'px'
    }
  }
}

const _convertPeriod = ({ from, to }) => {
  return {
    from: {
      ...from,
      ...{
        month: from.month + 1
      }
    },
    to: {
      ...to,
      ...{
        month: to.month + 1
      }
    }
  }
}

const _convertPeriodToMetaModel = ({ from, to }) => {
  const fromMonth = defaultFormatFunc(from.month, from.year);
  const toMonth = defaultFormatFunc(to.month, to.year);
  return {
    fromMonth,
    toMonth
  }
}
