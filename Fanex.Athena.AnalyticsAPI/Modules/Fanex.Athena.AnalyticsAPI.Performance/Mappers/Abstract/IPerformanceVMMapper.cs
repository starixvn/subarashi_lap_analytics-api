using System;
using System.Collections.Generic;
using Fanex.Athena.AnalyticsAPI.Performance.Models.DTO;
using Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels;

namespace Fanex.Athena.AnalyticsAPI.Performance.Mappers
{
    public interface IPerformanceVMMapper
    {
        IEnumerable<MeasurementPeriodComparisonGrowthVM> ToMeasurementGrowthPeriodVM<T>(
            IEnumerable<T> prevPeriodDtos,
            IEnumerable<T> currentPeriodDtos,
            IEnumerable<DistributionMetaModel> topDistributions)
            where T : MeasurementPeriodDTO;

        IEnumerable<MeasurementGrowthVM> ToMeasurementGrowthTimeVM(IEnumerable<MeasurementTimeDTO> periodDtos, DateTime fromMonth, DateTime toMonth);

        IEnumerable<DistributionVM> ToDistributionVM<T>(
            IEnumerable<T> prevPeriodDtos,
            IEnumerable<T> currentPeriodDtos,
            IEnumerable<DistributionMetaModel> topDistributions)
            where T : DistributionDTO;

        IEnumerable<MeasurementComparisonVM> ToMeasurementComparisonVM<T>(
            IEnumerable<T> prevPeriodDtos,
            IEnumerable<T> currentPeriodDtos,
            IEnumerable<DistributionMetaModel> topDistributions)
            where T : MeasurementComparisonDTO;

        IEnumerable<OverallMeasurementVM> ToOverallMeasurementVM<T>(T prevPeriodDto, T currentPeriodDto)
            where T : TotalDTO;
    }
}