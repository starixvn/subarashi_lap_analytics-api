using Fanex.Athena.AnalyticsAPI.Common.Helpers;
using Fanex.Athena.AnalyticsAPI.Common.Mappers;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Models.DTO;
using Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.Athena.AnalyticsAPI.Performance.Mappers
{
    public class PerformanceVMMapper : ViewModelMapper, IPerformanceVMMapper
    {
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

        public IEnumerable<DistributionVM> ToDistributionVM<T>(
            IEnumerable<T> prevPeriodDtos,
            IEnumerable<T> currentPeriodDtos,
            IEnumerable<DistributionMetaModel> topDistributions)
            where T : DistributionDTO
        {
            return new List<DistributionVM>
            {
                ToDistributionVM<DistributionDTO>(prevPeriodDtos, topDistributions, PREVIOUS_PERIOD_ID, PREVIOUS_PERIOD_NAME),
                ToDistributionVM<DistributionDTO>(currentPeriodDtos, topDistributions, CURRENT_PERIOD_ID, CURRENT_PERIOD_NAME)
            };
        }

        public IEnumerable<OverallMeasurementVM> ToOverallMeasurementVM<T>(T prevPeriodDto, T currentPeriodDto)
            where T : TotalDTO
        {
            return new List<OverallMeasurementVM>
            {
                new OverallMeasurementVM
                {
                    Id = PREVIOUS_PERIOD_ID,
                    Name = PREVIOUS_PERIOD_NAME,
                    Total = _calculationService.ToRound(prevPeriodDto?.Total)
                },
                new OverallMeasurementVM
                {
                    Id = CURRENT_PERIOD_ID,
                    Name = CURRENT_PERIOD_NAME,
                    Total = _calculationService.ToRound(currentPeriodDto?.Total)
                }
            };
        }

        public IEnumerable<MeasurementPeriodComparisonGrowthVM> ToMeasurementGrowthPeriodVM<T>(
            IEnumerable<T> prevPeriodDtos,
            IEnumerable<T> currentPeriodDtos,
            IEnumerable<DistributionMetaModel> topDistributions
            )
            where T : MeasurementPeriodDTO
        {
            var result = new List<MeasurementPeriodComparisonGrowthVM>(topDistributions.Count());
            var prevPeriodDtoDic = prevPeriodDtos.ToDictionary(_ => _.Id, _ => _);
            var currentPeriodDtoDic = currentPeriodDtos.ToDictionary(_ => _.Id, _ => _);

            foreach (var topDistribution in topDistributions)
            {
                var (topDistributionId, topDistributionName) = (topDistribution.DisplayId, topDistribution.DisplayName);
                var currentPeriodDto = currentPeriodDtoDic[topDistribution.DisplayId];

                var previousPeriodTotal = prevPeriodDtoDic.ContainsKey(topDistributionId) ? prevPeriodDtoDic[topDistributionId].Total : 0;
                var currentPeriodTotal = currentPeriodDto.Total;

                var vmItem = CreateMeasurementPeriodComparisonGrowthVM(topDistributionId, topDistributionName,
                    previousPeriodTotal, currentPeriodTotal,
                    topDistribution.PreviousPeriodDistributions?.Select(_ => new IdentifyVM { Id = _.Id, Name = _.Name }),
                    topDistribution.CurrentPeriodDistributions.Select(_ => new IdentifyVM { Id = _.Id, Name = _.Name })
                    );

                result.Add(vmItem);
            }

            return result;
        }

        private DistributionVM ToDistributionVM<T>(IEnumerable<T> dtos, IEnumerable<DistributionMetaModel> topDistributions, string id, string name)
            where T : DistributionDTO
        {
            var listDto = dtos.ToList();
            var sumItem = listDto.FirstOrDefault(_ => _.IsSum);
            listDto.Remove(sumItem);

            var dtoDic = listDto.ToDictionary(_ => _.Id, _ => _);

            var result = new DistributionVM(topDistributions.Count() + 1)
            {
                Id = id,
                Name = name,
                Total = _calculationService.ToRound(sumItem?.Total)
            };

            foreach (var topDistribution in topDistributions)
            {
                (string topDistributionId, string topDistributionName) = (topDistribution.DisplayId, topDistribution.DisplayName);

                if (topDistributionId == DISTRIBUTION_OTHERS_ID)
                {
                    continue;
                }

                var distributionItem = new DistributionItemVM
                {
                    Id = topDistributionId,
                    Name = topDistributionName
                };

                if (dtoDic.TryGetValue(topDistributionId, out T distributionDto))
                {
                    distributionItem.Percent = _calculationService.ToPercentUnit(distributionDto.Distribution);
                    dtoDic.Remove(topDistributionId);
                }
                else
                {
                    distributionItem.Percent = 0;
                }

                result.Distributions.Add(distributionItem);
            }

            if (HasOthers(topDistributions))
            {
                result.Distributions.Add(new DistributionItemVM
                {
                    Id = DISTRIBUTION_OTHERS_ID,
                    Name = DISTRIBUTION_OTHERS_NAME,
                    Percent = _calculationService.ToPercentUnit(dtoDic.Sum(_ => _.Value.Distribution))
                });
            }

            return result;
        }

        public IEnumerable<MeasurementComparisonVM> ToMeasurementComparisonVM<T>(
            IEnumerable<T> prevPeriodDtos,
            IEnumerable<T> currentPeriodDtos,
            IEnumerable<DistributionMetaModel> topDistributions)
            where T : MeasurementComparisonDTO
        {
            var result = new List<MeasurementComparisonVM>(topDistributions.Count());

            var prevPeriodDtoDic = prevPeriodDtos.ToDictionary(_ => _.Id, _ => _);
            var currentPeriodDtoDic = currentPeriodDtos.ToDictionary(_ => _.Id, _ => _);

            foreach (var topDistribution in topDistributions)
            {
                (string topDistributionId, string topDistributionName) = (topDistribution.DisplayId, topDistribution.DisplayName);

                if (topDistributionId == DISTRIBUTION_OTHERS_ID)
                {
                    continue;
                }

                if (currentPeriodDtoDic.TryGetValue(topDistributionId, out T currentPeriodDto))
                {
                    var prevPeriodDto = prevPeriodDtoDic.ContainsKey(topDistributionId) ? prevPeriodDtoDic[topDistributionId] : null;
                    var vmItem = CreateMeasurementComparisonVM(topDistributionId, topDistributionName, prevPeriodDto, currentPeriodDto);

                    result.Add(vmItem);
                }

                prevPeriodDtoDic.Remove(topDistributionId);
                currentPeriodDtoDic.Remove(topDistributionId);
            }

            var distributionOthers = topDistributions.FirstOrDefault(_ => _.DisplayId == DISTRIBUTION_OTHERS_ID);
            if (distributionOthers != null)
            {
                prevPeriodDtoDic.Remove(DISTRIBUTION_OTHERS_ID, out T prevPeriodOthers);
                currentPeriodDtoDic.Remove(DISTRIBUTION_OTHERS_ID, out T currentPeriodOthers);
                var prevPeriodMargin = prevPeriodOthers?.Margin;
                var currentPeriodMargin = currentPeriodOthers.Margin;

                var (prevPeriodTunover, prevPeriodTunoverDistribution,
                    prevPeriodGrossProfit, prevPeriodGrossProfitDistribution) = SumFromListOf<MeasurementComparisonDTO>(prevPeriodDtoDic.Select(_ => _.Value));

                var (currentPeriodTunover, currentPeriodTunoverDistribution,
                   currentPeriodGrossProfit, currentPeriodGrossProfitDistribution) = SumFromListOf<MeasurementComparisonDTO>(currentPeriodDtoDic.Select(_ => _.Value));

                var vmItem = new MeasurementComparisonVM
                {
                    Id = DISTRIBUTION_OTHERS_ID,
                    Name = distributionOthers.DisplayName,
                    DifferenceTurnover = CreateDifferenceMeasurement(prevPeriodTunover, currentPeriodTunover, prevPeriodTunoverDistribution, currentPeriodTunoverDistribution),
                    DifferenceGrossProfit = CreateDifferenceMeasurement(prevPeriodGrossProfit, currentPeriodGrossProfit, prevPeriodGrossProfitDistribution, currentPeriodGrossProfitDistribution),
                    DifferenceMargin = CreateDifferenceMeasurement(prevPeriodMargin, currentPeriodMargin, 0, 0)
                };

                result.Add(vmItem);
            }

            return result;
        }

        private MeasurementPeriodComparisonGrowthVM CreateMeasurementPeriodComparisonGrowthVM(
           string id,
           string name,
           double prevPeriodTotal,
           double currentPeriodTotal,
           IEnumerable<IdentifyVM> previousPeriodIdentifies = null,
           IEnumerable<IdentifyVM> currentPeriodIdentifies = null)
        {
            return new MeasurementPeriodComparisonGrowthVM
            {
                Id = id,
                Name = name,
                PreviousPeriodTotal = _calculationService.ToRound(prevPeriodTotal),
                CurrentPeriodTotal = _calculationService.ToRound(currentPeriodTotal),
                GrowthPercent = _calculationService.CalculateGrowth(currentPeriodTotal, prevPeriodTotal),
                PreviousPeriodDistributions = previousPeriodIdentifies ?? new List<IdentifyVM> { new IdentifyVM { Id = id, Name = name } },
                CurrentPeriodDistributions = currentPeriodIdentifies ?? new List<IdentifyVM> { new IdentifyVM { Id = id, Name = name } }
            };
        }

        private MeasurementComparisonVM CreateMeasurementComparisonVM(string id, string name, MeasurementComparisonDTO prevPeriodDto, MeasurementComparisonDTO currentPeriodDto)
        {
            return new MeasurementComparisonVM
            {
                Id = id,
                Name = name,
                DifferenceTurnover = CreateDifferenceMeasurement(prevPeriodDto?.Turnover, currentPeriodDto.Turnover, prevPeriodDto?.TurnoverDistribution, currentPeriodDto.TurnoverDistribution),
                DifferenceGrossProfit = CreateDifferenceMeasurement(prevPeriodDto?.GrossProfit, currentPeriodDto.GrossProfit,
                    prevPeriodDto?.GrossProfitDistribution, currentPeriodDto.GrossProfitDistribution),
                DifferenceMargin = CreateDifferenceMeasurement(prevPeriodDto?.Margin, currentPeriodDto.Margin, 0, 0)
            };
        }

        private DifferenceMeasurement CreateDifferenceMeasurement(
            double? prevPeriodMeasurement,
            double currentPeriodMeasurement,
            double? prevPeriodDistribution,
            double currentPeriodDistribution)
        {
            return new DifferenceMeasurement
            {
                DifferenceTotal = _calculationService.ToRound(currentPeriodMeasurement - (prevPeriodMeasurement ?? 0)),
                DifferenceDistribution = _calculationService.ToPercentUnit(currentPeriodDistribution - (prevPeriodDistribution ?? 0))
            };
        }

        private static (double turnover, double turnoverDistribution, double grossProfit, double grossProfitDistribution) SumFromListOf<T>(IEnumerable<T> items)
            where T : MeasurementComparisonDTO
        {
            var turnover = 0.00;
            var turnoverDistribution = 0.00;
            var grossProfit = 0.00;
            var grossProfitDistribution = 0.00;

            foreach (var item in items)
            {
                turnover += item.Turnover;
                turnoverDistribution += item.TurnoverDistribution;
                grossProfit += item.GrossProfit;
                grossProfitDistribution += item.GrossProfitDistribution;
            }

            return (turnover, turnoverDistribution, grossProfit, grossProfitDistribution);
        }

        private static bool HasOthers(IEnumerable<DistributionMetaModel> topDistributions)
        {
            if (topDistributions == null)
            {
                throw new ArgumentNullException(nameof(topDistributions));
            }
            return topDistributions.Any(_ => _.DisplayId == DISTRIBUTION_OTHERS_ID);
        }
    }
}