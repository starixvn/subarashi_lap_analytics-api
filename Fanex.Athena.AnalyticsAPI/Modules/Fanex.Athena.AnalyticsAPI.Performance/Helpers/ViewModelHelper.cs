using Fanex.Athena.AnalyticsAPI.Common.Helpers;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Models.DTO;
using Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.Athena.AnalyticsAPI.Performance.Helpers
{
    public class OverviewViewModelMapper : ViewModelHelper
    {
        public OverviewViewModelMapper(
            ICalculationService calculationService
            )
            : base(calculationService)
        {
        }

        public IEnumerable<TopChangeVM> ToViewModel<TDto>(Func<TDto, int> keySelector, params IEnumerable<TDto>[] dtos)
            where TDto : TopChangeDTO
        {
            if (IsInValidDto(dtos))
            {
                return Enumerable.Empty<TopChangeVM>();
            }

            var currentPeriodData = dtos[0];
            var prevPeriodData = dtos[1];

            var currentPeriodDataDic = currentPeriodData.ToDictionary(_ => keySelector(_));
            var prevPeriodDataDic = currentPeriodData.ToDictionary(_ => keySelector(_));

            var distinctList = Enumerable.Union(currentPeriodData.Select(_ => keySelector(_)), prevPeriodData.Select(_ => keySelector(_)));

            var result = distinctList
                .Select(_ => new TopChangeVM
                {
                    PercentChange = _calculationService.CalculateGrowth(
                        currentPeriodDataDic.TryGetValue(_, out TDto current) ? current.Total : 0,
                        prevPeriodDataDic.TryGetValue(_, out TDto prev) ? prev.Total : 0
                        ),
                    PerspectiveName = keySelector(current).ToString(),
                    Total = _calculationService.ToThousandUnit(current.Total)
                })
                .OrderByDescending(_ => Math.Abs(_.PercentChange))
                .Take(5);

            return result;
        }

        public IEnumerable<SummaryVM> ToViewModel(params IEnumerable<SummaryDTO>[] dtos)
        {
            if (IsInValidDto(dtos))
            {
                return Enumerable.Empty<SummaryVM>();
            }

            var currentPeriodData = dtos[0]?.FirstOrDefault();
            var prevPeriodData = dtos[1]?.FirstOrDefault();

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

        public IEnumerable<MeasurementGrowthVM> ToViewModel(params IEnumerable<MeasurementGrowthDTO>[] dtos)
        {
            if (IsInValidDto(dtos))
            {
                return Enumerable.Empty<MeasurementGrowthVM>();
            }

            var currentPeriodData = dtos[0]?.FirstOrDefault();
            var prevPeriodData = dtos[1]?.FirstOrDefault();

            var result = new List<MeasurementGrowthVM>
            {
                new MeasurementGrowthVM
                {
                    Time = "Prev Period",
                    Total = _calculationService.ToThousandUnit(currentPeriodData?.Total),
                    GrowthPercent = 0
                },
                new MeasurementGrowthVM
                {
                    Time = "Current Period",
                    Total = _calculationService.ToThousandUnit(prevPeriodData?.Total),
                    GrowthPercent = _calculationService.CalculateGrowth(currentPeriodData?.Total, prevPeriodData?.Total)
                }
            };

            return result;
        }
    }
}