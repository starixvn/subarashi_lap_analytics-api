using Fanex.Athena.AnalyticsAPI.Performance.Models.DTO;
using Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels;
using System;
using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Performance.Mappers
{
    public interface IPerformanceVMMapper
    {
        IEnumerable<MeasurementGrowthVM> ToMeasurementGrowthPeriodVM(IEnumerable<TotalDTO> prevPeriodOfPrevDtos, IEnumerable<TotalDTO> prevPeriodDtos, IEnumerable<TotalDTO> currentPeriodDtos);

        IEnumerable<MeasurementGrowthVM> ToMeasurementGrowthTimeVM(IEnumerable<MeasurementTimeDTO> periodDtos, DateTime fromMonth, DateTime toMonth);

        IEnumerable<MeasurementPeriodComparisonGrowthVM> ToMeasurementGrowthPeriodVM<T>(IEnumerable<T> prevPeriodDtos, IEnumerable<T> currentPeriodDtos)
            where T : MeasurementPeriodDTO;

        IEnumerable<DistributionVM> ToDistributionVM<T>(IEnumerable<T> prevPeriodDtos, IEnumerable<T> currentPeriodDtos)
            where T : DistributionDTO;

        IEnumerable<MeasurementComparisonVM> ToMeasurementComparisonVM<T>(IEnumerable<T> prevPeriodDtos, IEnumerable<T> currentPeriodDtos)
            where T : MeasurementComparisonDTO;
    }
}