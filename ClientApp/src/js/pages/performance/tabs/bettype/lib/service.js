import { distributionChartDataMapper } from '../../service'

export const bettypeDistributionChartDataMapper =  (distributionData, topDistributions) => {
    const result = distributionChartDataMapper(distributionData, topDistributions, null, 5);
    return result;
}
