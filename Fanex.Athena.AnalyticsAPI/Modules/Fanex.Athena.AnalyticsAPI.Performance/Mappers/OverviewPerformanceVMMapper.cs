using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Models.DTO;
using Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.Athena.AnalyticsAPI.Performance.Mappers
{
    public class OverviewPerformanceVMMapper : PerformanceVMMapper, IOverviewPerformanceVMMapper
    {
        public OverviewPerformanceVMMapper(
            ICalculationService calculationService
            )
            : base(calculationService)
        {
        }

        public IEnumerable<TopChangeVM> ToViewModel<TDto>(IEnumerable<TDto> prevPeriodDtos, IEnumerable<TDto> currentPeriodDtos, Func<TDto, int> keySelector)
            where TDto : TopChangeDTO
        {
            var currentPeriodData = prevPeriodDtos.ToDictionary(_ => keySelector(_));
            var prevPeriodData = currentPeriodDtos.ToDictionary(_ => keySelector(_));

            var distinctList = Enumerable.Union(currentPeriodDtos.Select(_ => keySelector(_)), prevPeriodDtos.Select(_ => keySelector(_)));

            var result = distinctList
                .Select(_ => new TopChangeVM
                {
                    PercentChange = _calculationService.CalculateGrowth(
                        currentPeriodData.TryGetValue(_, out TDto current) ? current.Total : 0,
                        prevPeriodData.TryGetValue(_, out TDto prev) ? prev.Total : 0
                        ),
                    PerspectiveName = keySelector(current).ToString(),
                    Total = _calculationService.ToThousandUnit(current.Total)
                })
                .OrderByDescending(_ => Math.Abs(_.PercentChange))
                .Take(5);

            return result;
        }

        public IEnumerable<SummaryVM> ToViewModel(IEnumerable<SummaryDTO> prevPeriodDtos, IEnumerable<SummaryDTO> currentPeriodDtos)
        {
            var prevPeriodData = prevPeriodDtos.FirstOrDefault();
            var currentPeriodData = currentPeriodDtos.FirstOrDefault();

            var result = new List<SummaryVM>
            {
                new SummaryVM
                {
                    MeasurementId = 0,
                    Total = _calculationService.ToThousandUnit(currentPeriodData?.Turnover),
                    ChangePercent = _calculationService.CalculateGrowth(currentPeriodData?.Turnover, prevPeriodData?.Turnover)
                },
                new SummaryVM
                {
                    MeasurementId = 1,
                    Total = _calculationService.ToThousandUnit(currentPeriodData?.GrossProfit),
                    ChangePercent = _calculationService.CalculateGrowth(currentPeriodData?.GrossProfit, prevPeriodData?.GrossProfit)
                },
                new SummaryVM
                {
                    MeasurementId = 2,
                    Total = _calculationService.ToThousandUnit(currentPeriodData?.Margin),
                    ChangePercent = _calculationService.CalculateGrowth(currentPeriodData?.Margin, prevPeriodData?.Margin)
                }
            };

            return result;
        }
    }
}