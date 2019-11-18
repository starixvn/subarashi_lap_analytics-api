using Fanex.Athena.AnalyticsAPI.Performance.Models.DTO;
using Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels;
using System;
using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Performance.Mappers
{
    public interface IOverviewPerformanceVMMapper : IPerformanceVMMapper
    {
        IEnumerable<SummaryVM> ToSummaryVM(IEnumerable<SummaryDTO> prevPeriodDtos, IEnumerable<SummaryDTO> currentPeriodDtos);

        IEnumerable<TopChangeVM> ToTopChangeVM<TDto, TKey>(IEnumerable<TDto> prevPeriodDtos, IEnumerable<TDto> currentPeriodDtos, Func<TDto, TKey> keySelector, Func<TDto, string> displayer)
            where TDto : TopChangeDTO;

        IEnumerable<MeasurementGrowthVM> ToMeasurementGrowthPeriodVM(IEnumerable<TotalDTO> prevPeriodOfPrevDtos, IEnumerable<TotalDTO> prevPeriodDtos, IEnumerable<TotalDTO> currentPeriodDtos);
    }
}