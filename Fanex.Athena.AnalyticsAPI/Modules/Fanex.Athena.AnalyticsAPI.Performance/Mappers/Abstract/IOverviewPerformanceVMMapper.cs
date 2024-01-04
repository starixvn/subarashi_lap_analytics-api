using System.Collections.Generic;
using Fanex.Athena.AnalyticsAPI.Performance.Models.DTO;
using Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels;

namespace Fanex.Athena.AnalyticsAPI.Performance.Mappers
{
    public interface IOverviewPerformanceVMMapper : IPerformanceVMMapper
    {
        IEnumerable<SummaryVM> ToSummaryVM(IEnumerable<SummaryDTO> prevPeriodDtos, IEnumerable<SummaryDTO> currentPeriodDtos);

        IEnumerable<TopChangeVM> ToTopChangeVM<TDto>(IEnumerable<TDto> prevPeriodDtos, IEnumerable<TDto> currentPeriodDtos)
            where TDto : TopChangeDTO;

        IEnumerable<MeasurementGrowthVM> ToMeasurementGrowthPeriodVM(IEnumerable<TotalDTO> prevPeriodOfPrevDtos, IEnumerable<TotalDTO> prevPeriodDtos, IEnumerable<TotalDTO> currentPeriodDtos);
    }
}