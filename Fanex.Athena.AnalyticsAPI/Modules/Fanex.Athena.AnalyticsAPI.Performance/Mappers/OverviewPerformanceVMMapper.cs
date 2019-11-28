using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Constants;
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

        public IEnumerable<MeasurementGrowthVM> ToMeasurementGrowthPeriodVM(
            IEnumerable<TotalDTO> prevPeriodOfPrevDtos,
            IEnumerable<TotalDTO> prevPeriodDtos,
            IEnumerable<TotalDTO> currentPeriodDtos)
        {
            var prevPeriodOfPrevData = prevPeriodOfPrevDtos.FirstOrDefault();
            var prevPeriodData = prevPeriodDtos.FirstOrDefault();
            var currentPeriodData = currentPeriodDtos.FirstOrDefault();

            var result = new List<MeasurementGrowthVM>
            {
                new MeasurementGrowthVM
                {
                    Name = PREVIOUS_PERIOD_NAME,
                    Total = _calculationService.ToRound(prevPeriodData?.Total),
                    GrowthPercent = _calculationService.CalculateGrowth(prevPeriodData?.Total, prevPeriodOfPrevData?.Total)
                },
                new MeasurementGrowthVM
                {
                    Name = CURRENT_PERIOD_NAME,
                    Total = _calculationService.ToRound(currentPeriodData?.Total),
                    GrowthPercent = _calculationService.CalculateGrowth(currentPeriodData?.Total, prevPeriodData?.Total)
                }
            };

            return result;
        }

        public IEnumerable<TopChangeVM> ToTopChangeVM<TDto>(
            IEnumerable<TDto> prevPeriodDtos,
            IEnumerable<TDto> currentPeriodDtos)
            where TDto : TopChangeDTO
        {
            var prevPeriodData = prevPeriodDtos.ToDictionary(_ => _.Id);
            var currentPeriodData = currentPeriodDtos.ToDictionary(_ => _.Id);

            var distinctList = Enumerable.Union(currentPeriodDtos.Select(_ => _.Id), prevPeriodDtos.Select(_ => _.Id));

            var result = distinctList
                .Select(_ =>
                {
                    var percentChange = _calculationService.CalculateGrowth(
                            currentPeriodData.TryGetValue(_, out TDto current) ? current.Total : 0,
                            prevPeriodData.TryGetValue(_, out TDto prev) ? prev.Total : 0
                        );

                    var perspectiveId = (current ?? prev)?.Id;
                    var perspectiveName = (current ?? prev)?.Name;
                    var total = _calculationService.ToRound(current?.Total);

                    return new TopChangeVM
                    {
                        PerspectiveId = perspectiveId,
                        PerspectiveName = perspectiveName,
                        Total = total,
                        ChangePercent = percentChange,
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
                    MeasurementId = MeasurementConst.TURNOVER,
                    Total = _calculationService.ToRound(currentPeriodData?.Turnover),
                    ChangePercent = _calculationService.CalculateGrowth(currentPeriodData?.Turnover, prevPeriodData?.Turnover)
                },
                new SummaryVM
                {
                    MeasurementId = MeasurementConst.GROSS_PROFIT,
                    Total = _calculationService.ToRound(currentPeriodData?.GrossProfit),
                    ChangePercent = _calculationService.CalculateGrowth(currentPeriodData?.GrossProfit, prevPeriodData?.GrossProfit)
                },
                new SummaryVM
                {
                    MeasurementId = MeasurementConst.MARGIN,
                    Total = _calculationService.ToRound(currentPeriodData?.Margin),
                    ChangePercent = _calculationService.CalculateGrowth(currentPeriodData?.Margin, prevPeriodData?.Margin)
                }
            };

            return result;
        }
    }
}