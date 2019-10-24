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
        private const string DISTRIBUTION_OTHERS_ID = "0";
        private const string DISTRIBUTION_OTHERS_NAME = "Others";

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
            var topDistribution = currentPeriodDtos
                .Where(_ => !_.IsSum)
                .OrderByDescending(_ => Math.Abs(_.Total))
                .Take(NUMBER_OF_TOP)
                .Select(_ => new Tuple<string, string>(_.Id, _.Name));

            if (currentPeriodDtos.Count() > NUMBER_OF_TOP)
            {
                topDistribution = topDistribution.Append(new Tuple<string, string>(DISTRIBUTION_OTHERS_ID, DISTRIBUTION_OTHERS_NAME));
            }

            return new List<DistributionVM>
            {
                ToDistributionVM<DistributionDTO>(prevPeriodDtos, topDistribution, "Previous Period"),
                ToDistributionVM<DistributionDTO>(currentPeriodDtos, topDistribution, "Current Period")
            };
        }

        private DistributionVM ToDistributionVM<T>(IEnumerable<T> dtos, IEnumerable<Tuple<string, string>> topDistributions, string name)
            where T : DistributionDTO
        {
            var list = dtos.ToList();
            var sumItem = list.FirstOrDefault(_ => _.IsSum);
            list.Remove(sumItem);

            var sumAbs = list.Sum(_ => Math.Abs(_.Total));
            var dtoDic = list.ToDictionary(_ => _.Id, _ => _);

            var result = new DistributionVM(topDistributions.Count() + 1)
            {
                Name = name,
                Total = _calculationService.ToRound(sumItem.Total)
            };

            foreach (var distribution in topDistributions)
            {
                if(distribution.Item1 == DISTRIBUTION_OTHERS_ID)
                {
                    continue;
                }

                var distributionItem = new DistributionItemVM
                {
                    Name = distribution.Item2
                };

                if (dtoDic.TryGetValue(distribution.Item1, out T distributionDto))
                {
                    distributionItem.Percent = _calculationService.CalculateDistribution(distributionDto.Total, sumAbs);
                    dtoDic.Remove(distribution.Item1);
                }
                else
                {
                    distributionItem.Percent = _calculationService.CalculateDistribution(0, sumAbs);
                }

                result.Distributions.Add(distributionItem);
            }

            if(topDistributions.Any(_ => _.Item1 == DISTRIBUTION_OTHERS_ID))
            {
                result.Distributions.Add(new DistributionItemVM
                {
                    Name = DISTRIBUTION_OTHERS_NAME,
                    Percent = _calculationService.CalculateDistribution(dtoDic.Sum(_ => _.Value.Total), sumAbs)
                });
            }

            return result;
        }

        public IEnumerable<MeasurementComparisonVM> ToMeasurementComparisonVM<T>(IEnumerable<T> prevPeriodDtos, IEnumerable<T> currentPeriodDtos)
            where T : MeasurementComparisonDTO
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MeasurementPeriodComparisonGrowthVM> ToMeasurementGrowthPeriodVM<T>(
            IEnumerable<T> prevPeriodDtos,
            IEnumerable<T> currentPeriodDtos,
            IEnumerable<Tuple<string, string>> topDistributions
            )
            where T : MeasurementPeriodDTO
        {
            var result = new List<MeasurementPeriodComparisonGrowthVM>(topDistributions.Count());
            var prevPeriodDtoDic = prevPeriodDtos.ToDictionary(_ => _.Id, _ => _);
            var currentPeriodDtoDic = currentPeriodDtos.ToDictionary(_ => _.Id, _ => _);

            var hasOthers = topDistributions.Any(_ => _.Item1 == DISTRIBUTION_OTHERS_ID);

            var prevPeriodDtoTotal = prevPeriodDtos.Sum(_ => _.Total);
            var currentPeriodDtoTotal = prevPeriodDtos.Sum(_ => _.Total);

            foreach (var topDistribution in topDistributions)
            {
                if (currentPeriodDtoDic.TryGetValue(topDistribution.Item1, out T currentPeriodDto))
                {
                    var vm = CreateMeasurementPeriodComparisonGrowthVM(
                        currentPeriodDto.Name,
                        prevPeriodDtoDic.ContainsKey(topDistribution.Item1) ? prevPeriodDtoDic[topDistribution.Item1].Total : 0,
                        currentPeriodDto.Total);

                    result.Add(vm);

                    if (hasOthers)
                    {
                        prevPeriodDtoTotal -= vm.PreviousPeriodTotal;
                        currentPeriodDtoTotal -= vm.CurrentPeriodTotal;
                    }
                }
            }

            if (hasOthers)
            {
                result.Add(CreateMeasurementPeriodComparisonGrowthVM(
                        DISTRIBUTION_OTHERS_NAME,
                        prevPeriodDtos.Count() > NUMBER_OF_TOP ? prevPeriodDtoTotal : 0,
                        currentPeriodDtoTotal));
            }

            return result;
        }

        private MeasurementPeriodComparisonGrowthVM CreateMeasurementPeriodComparisonGrowthVM(
            string name,
            double prevPeriodTotal,
            double currentPeriodTotal)
        {
            return new MeasurementPeriodComparisonGrowthVM
            {
                Name = name,
                PreviousPeriodTotal = _calculationService.ToRound(prevPeriodTotal),
                CurrentPeriodTotal = _calculationService.ToRound(currentPeriodTotal),
                GrowthPercent = _calculationService.CalculateGrowth(currentPeriodTotal, prevPeriodTotal)
            };
        }
    }
}