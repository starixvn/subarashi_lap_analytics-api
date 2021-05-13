import { distributionChartDataMapper, differenceTableDataMapper } from '../../service'
import { generalService } from '../../../lib/service';

export const currencyDistributionChartDataMapper =  (distributionData, topDistributions) => {
  const displayNameMapper = currency => {
    return generalService.getCurrencyDisplayName(currency, 'displayId', 'displayName');
  };
  const result = distributionChartDataMapper(distributionData, topDistributions, displayNameMapper);
  return result;
}

export const currencyDifferenceTableDataMapper =  (measurementDifferenceData, tabKey) => {
  const displayNameMapper = currency => {
    return generalService.getCurrencyDisplayName(currency, 'id', 'name');
  };
  const result = differenceTableDataMapper(measurementDifferenceData, tabKey, displayNameMapper);
  return result;
}

export const growthChartDisplayNameMapper =  currency => {
    return generalService.getCurrencyDisplayName(currency, 'id', 'name');
}
