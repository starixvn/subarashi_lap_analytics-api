import languageApi from '../../../lib/languageApi';

export const differenceTableTitle = languageApi.getDefaultRes('difference_table_title');

export const growthChartConsts = [
  {
    periodName: languageApi.getDefaultRes('previous_period'),
    periodFieldName: 'previousPeriodTotal',
    periodField: 'previousPeriod',
    distributionFieldName: 'previousPeriodDistributions'
  },
  {
    periodName: languageApi.getDefaultRes('current_period'),
    periodFieldName: 'currentPeriodTotal',
    periodField: 'currentPeriod',
    distributionFieldName: 'currentPeriodDistributions',
    columnColor: '#36A2EB'
  }
]

export const rowDataMapper = [
  data => ({
    value: data.differenceTurnover.differenceDistribution,
    isPercentUnit: true
  }),
  data => ({
    value: data.differenceTurnover.differenceTotal,
    isPercentUnit: false
  }),
  data => ({
    value: data.differenceGrossProfit.differenceDistribution,
    isPercentUnit: true
  }),
  data => ({
    value: data.differenceGrossProfit.differenceTotal,
    isPercentUnit: false
  }),
  data => ({
    value: data.differenceMargin.differenceTotal,
    isPercentUnit: true
  })
]
