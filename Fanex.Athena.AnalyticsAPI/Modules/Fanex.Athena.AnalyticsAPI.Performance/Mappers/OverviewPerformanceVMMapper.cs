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
        private const int NUMBER_OF_TOP_CHANGE = 5;

        public OverviewPerformanceVMMapper(
            ICalculationService calculationService
            )
            : base(calculationService)
        {
        }

        public IEnumerable<TopChangeVM> ToTopChangeVM<TDto, TKey>(
            IEnumerable<TDto> prevPeriodDtos,
            IEnumerable<TDto> currentPeriodDtos,
            Func<TDto, TKey> keySelector,
            Func<TDto, string> displayer)
            where TDto : TopChangeDTO
        {
            var prevPeriodData = prevPeriodDtos.ToDictionary(_ => keySelector(_));
            var currentPeriodData = currentPeriodDtos.ToDictionary(_ => keySelector(_));

            var distinctList = Enumerable.Union(currentPeriodDtos.Select(_ => keySelector(_)), prevPeriodDtos.Select(_ => keySelector(_)));

            var result = distinctList
                .Select(_ =>
                {
                    var percentChange = _calculationService.CalculateGrowth(
                            currentPeriodData.TryGetValue(_, out TDto current) ? current.Total : 0,
                            prevPeriodData.TryGetValue(_, out TDto prev) ? prev.Total : 0
                        );
                    var perspectiveName = displayer(current ?? prev);
                    var total = _calculationService.ToRound(current?.Total);

                    return new TopChangeVM
                    {
                        ChangePercent = percentChange,
                        PerspectiveName = perspectiveName,
                        Total = total
                    };
                })
                .OrderByDescending(_ => Math.Abs(_.ChangePercent))
                .Take(NUMBER_OF_TOP_CHANGE);

            return result;
        }

        public IEnumerable<SummaryVM> ToSummaryVM(IEnumerable<SummaryDTO> prevPeriodDtos, IEnumerable<SummaryDTO> currentPeriodDtos)
        {
            var prevPeriodData = prevPeriodDtos.FirstOrDefault();
            var currentPeriodData = currentPeriodDtos.FirstOrDefault();

            var result = new List<SummaryVM>
            {
                new SummaryVM
                {
                    MeasurementId = 1,
                    Total = _calculationService.ToRound(currentPeriodData?.Turnover),
                    ChangePercent = _calculationService.CalculateGrowth(currentPeriodData?.Turnover, prevPeriodData?.Turnover)
                },
                new SummaryVM
                {
                    MeasurementId = 2,
                    Total = _calculationService.ToRound(currentPeriodData?.GrossProfit),
                    ChangePercent = _calculationService.CalculateGrowth(currentPeriodData?.GrossProfit, prevPeriodData?.GrossProfit)
                },
                new SummaryVM
                {
                    MeasurementId = 3,
                    Total = _calculationService.ToRound(currentPeriodData?.Margin),
                    ChangePercent = _calculationService.CalculateGrowth(currentPeriodData?.Margin, prevPeriodData?.Margin)
                }
            };

            return result;
        }
    }
}