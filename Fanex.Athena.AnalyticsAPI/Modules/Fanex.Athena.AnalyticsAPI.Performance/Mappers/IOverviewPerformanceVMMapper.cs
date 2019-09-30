using Fanex.Athena.AnalyticsAPI.Performance.Models.DTO;
using Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels;
using System;
using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Performance.Mappers
{
    public interface IOverviewPerformanceVMMapper : IPerformanceVMMapper
    {
        IEnumerable<SummaryVM> ToViewModel(IEnumerable<SummaryDTO> prevPeriodDtos, IEnumerable<SummaryDTO> currentPeriodDtos);

        IEnumerable<TopChangeVM> ToViewModel<TDto>(IEnumerable<TDto> prevPeriodDtos, IEnumerable<TDto> currentPeriodDtos, Func<TDto, int> keySelector)
            where TDto : TopChangeDTO;
    }
}