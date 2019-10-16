using Fanex.Athena.AnalyticsAPI.Performance.Models.DTO;
using Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels;
using System;
using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Performance.Mappers
{
    public interface IPerformanceVMMapper
    {
        IEnumerable<MeasurementGrowthVM> ToViewModel(IEnumerable<MeasurementGrowthDTO> periodDtos, DateTime fromMonth, DateTime toMonth);

        IEnumerable<MeasurementGrowthVM> ToViewModel(IEnumerable<TotalDTO> prevPeriodOfPrevDtos, IEnumerable<TotalDTO> prevPeriodDtos, IEnumerable<TotalDTO> currentPeriodDtos);

        IEnumerable<DistributionVM> ToViewModel<T>(IEnumerable<T> prevPeriodDtos, IEnumerable<T> currentPeriodDtos)
            where T : DistributionDTO;
    }
}