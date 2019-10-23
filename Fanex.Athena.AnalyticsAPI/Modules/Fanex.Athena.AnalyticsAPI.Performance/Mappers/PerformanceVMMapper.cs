using Fanex.Athena.AnalyticsAPI.Common.Helpers;
using Fanex.Athena.AnalyticsAPI.Common.Mappers;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Models.DTO;
using Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.Athena.AnalyticsAPI.Performance.Mappers
{
    public class PerformanceVMMapper : ViewModelMapper, IPerformanceVMMapper
    {
        private const int NUMBER_OF_TOP = 5;

        public PerformanceVMMapper(
            ICalculationService calculationService
            )
            : base(calculationService)
        {
        }

        public IEnumerable<MeasurementGrowthVM> ToMeasurementGrowthTimeVM(
            IEnumerable<MeasurementTimeDTO> periodDtos,
            DateTime fromMonth, DateTime toMonth)
        {
            fromMonth = new DateTime(fromMonth.Year, fromMonth.Month, 1);
            toMonth = new DateTime(toMonth.Year, toMonth.Month, 1);

            var result = new List<MeasurementGrowthVM>();
            var currentPeriodData = periodDtos.ToDictionary(_ => _.Time.ToLocalTime().ToString(DateTimeHelper.MONTH_FORMAT));
            var prevMonthData = currentPeriodData.TryGetValue(fromMonth.AddMonths(-1).ToString(DateTimeHelper.MONTH_FORMAT), out MeasurementTimeDTO prevDto) ? prevDto : null;

            while (fromMonth <= toMonth)
            {
                var currentMonthData = currentPeriodData.TryGetValue(fromMonth.ToString(DateTimeHelper.MONTH_FORMAT), out MeasurementTimeDTO currentDto) ? currentDto : null;
                result.Add(new MeasurementGrowthVM
                {
                    Name = fromMonth.ToString(DateTimeHelper.MONTH_FORMAT),
                    Total = _calculationService.ToRound(currentMonthData?.Total),
                    GrowthPercent = _calculationService.CalculateGrowth(currentMonthData?.Total, prevMonthData?.Total)
                });

                prevMonthData = currentMonthData;
                fromMonth = fromMonth.AddMonths(1);
            }

            return result;
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
                    Name = "Previous Period",
                    Total = _calculationService.ToRound(prevPeriodData?.Total),
                    GrowthPercent = _calculationService.CalculateGrowth(prevPeriodData?.Total, prevPeriodOfPrevData?.Total)
                },
                new MeasurementGrowthVM
                {
                    Name = "Current Period",
                    Total = _calculationService.ToRound(currentPeriodData?.Total),
                    GrowthPercent = _calculationService.CalculateGrowth(currentPeriodData?.Total, prevPeriodData?.Total)
                }
            };

            return result;
        }

        public IEnumerable<DistributionVM> ToDistributionVM<T>(IEnumerable<T> prevPeriodDtos, IEnumerable<T> currentPeriodDtos)
            where T : DistributionDTO
        {
            return new List<DistributionVM>
            {
                ToViewModel<DistributionDTO>(prevPeriodDtos, "Previous Period"),
                ToViewModel<DistributionDTO>(currentPeriodDtos, "Current Period")
            };
        }

        private DistributionVM ToViewModel<T>(IEnumerable<T> dtos, string name)
            where T : DistributionDTO
        {
            var sumItem = dtos.FirstOrDefault(_ => _.IsSum);
            var sum = _calculationService.ToRound(sumItem.Total);

            var list = dtos.ToList();
            list.Remove(sumItem);
            var topDistribution = list.OrderByDescending(_ => Math.Abs(_.Total)).Take(NUMBER_OF_TOP).ToList();

            var result = new DistributionVM(topDistribution.Count + 1)
            {
                Name = name,
                Total = sum
            };

            var percentSum = 0.00;

            topDistribution.ForEach(_ =>
            {
                var distributionItem = new DistributionItemVM
                {
                    Name = _.Name,
                    Percent = _calculationService.CalculatePercent(_.Total, sum)
                };

                result.Distributions.Add(distributionItem);
                percentSum += distributionItem.Percent;
            });

            if (result.Distributions.Count == NUMBER_OF_TOP)
            {
                result.Distributions.Add(new DistributionItemVM
                {
                    Name = "Others",
                    Percent = _calculationService.ToRound(100 - percentSum)
                });
            }

            return result;
        }

        public IEnumerable<MeasurementComparisonVM> ToMeasurementComparisonVM<T>(IEnumerable<T> prevPeriodDtos, IEnumerable<T> currentPeriodDtos)
            where T : MeasurementComparisonDTO
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MeasurementPeriodComparisonGrowthVM> ToMeasurementGrowthPeriodVM<T>(IEnumerable<T> prevPeriodDtos, IEnumerable<T> currentPeriodDtos) where T : MeasurementPeriodDTO
        {
            throw new NotImplementedException();
        }
    }
}