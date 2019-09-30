using Fanex.Athena.AnalyticsAPI.Performance.Models.DTO;
using Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels;
using System;
using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Performance.Mappers
{
    public interface IOverviewPerformanceVMMapper : IPerformanceVMMapper
    {
        IEnumerable<SummaryVM> ToViewModel(IEnumerable<SummaryDTO> prevPeriodDtos, IEnumerable<SummaryDTO> currentPeriodDtos);

        IEnumerable<TopChangeVM> ToViewModel<TDto, TKey>(IEnumerable<TDto> prevPeriodDtos, IEnumerable<TDto> currentPeriodDtos, Func<TDto, TKey> keySelector, Func<TDto, string> displayer)
            where TDto : TopChangeDTO;
    }
}