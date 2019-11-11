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

        public IEnumerable<DistributionVM> ToDistributionVM<T>(
            IEnumerable<T> prevPeriodDtos,
            IEnumerable<T> currentPeriodDtos,
            IEnumerable<(string topDistributionId, string topDistributionName)> topDistributions)
            where T : DistributionDTO
        {
            return new List<DistributionVM>
            {
                ToDistributionVM<DistributionDTO>(prevPeriodDtos, topDistributions, PREVIOUS_PERIOD_NAME),
                ToDistributionVM<DistributionDTO>(currentPeriodDtos, topDistributions, CURRENT_PERIOD_NAME)
            };
        }

        public IEnumerable<OverallMeasurementVM> ToOverallMeasurement<T>(T prevPeriodDto, T currentPeriodDto)
            where T : TotalDTO
        {
            return new List<OverallMeasurementVM>
            {
                new OverallMeasurementVM
                {
                    Name = PREVIOUS_PERIOD_NAME,
                    Total = _calculationService.ToRound(prevPeriodDto?.Total)
                },
                new OverallMeasurementVM
                {
                    Name = CURRENT_PERIOD_NAME,
                    Total = _calculationService.ToRound(currentPeriodDto?.Total)
                }
            };
        }

        public IEnumerable<MeasurementComparisonVM> ToMeasurementComparisonVM<T>(
            IEnumerable<T> prevPeriodDtos,
            IEnumerable<T> currentPeriodDtos,
            IEnumerable<(string topDistributionId, string topDistributionName)> topDistributions)
            where T : MeasurementComparisonDTO
        {
            var result = new List<MeasurementComparisonVM>(topDistributions.Count());

            var prevPeriodDtoDic = prevPeriodDtos.ToDictionary(_ => _.Id, _ => _);
            var currentPeriodDtoDic = currentPeriodDtos.ToDictionary(_ => _.Id, _ => _);

            var sumAbsPeriodTurnover = ValueTuple.Create(prevPeriodDtos.Sum(_ => Math.Abs(_.Turnover)), currentPeriodDtos.Sum(_ => Math.Abs(_.Turnover)));
            var sumAbsPeriodGrossProfit = ValueTuple.Create(prevPeriodDtos.Sum(_ => Math.Abs(_.GrossProfit)), currentPeriodDtos.Sum(_ => Math.Abs(_.GrossProfit)));

            var sumCurrentPeriod = ValueTuple.Create(0.00, 0.00, 0.00);
            var sumPrevPeriod = ValueTuple.Create(0.00, 0.00, 0.00);

            var hasOthers = HasOthers(topDistributions);

            foreach (var topDistribution in topDistributions)
            {
                (string topDistributionId, string topDistributionName) = topDistribution;

                if (topDistributionId == DISTRIBUTION_OTHERS_ID)
                {
                    continue;
                }

                if (currentPeriodDtoDic.TryGetValue(topDistributionId, out T currentPeriodDto))
                {
                    var prevPeriodDto = prevPeriodDtoDic.ContainsKey(topDistributionId) ? prevPeriodDtoDic[topDistributionId] : null;
                    var vmItem = CreateMeasurementComparisonVM(topDistributionName, prevPeriodDto, currentPeriodDto, sumAbsPeriodTurnover, sumAbsPeriodGrossProfit);
                    if (hasOthers)
                    {
                        sumPrevPeriod = UpdateSumCurrentMeasurement(sumPrevPeriod, prevPeriodDto);
                        sumCurrentPeriod = UpdateSumCurrentMeasurement(sumCurrentPeriod, currentPeriodDto);
                    }

                    result.Add(vmItem);
                }
            }

            if (hasOthers)
            {
                (double sumPrevPeriodTurnover, double sumPrevPeriodGrossProfit, double sumPrevPeriodMargin) = sumPrevPeriod;
                (double sumCurrentPeriodTurnover, double sumCurrentPeriodGrossProfit, double sumCurrentPeriodMargin) = sumCurrentPeriod;

                sumCurrentPeriodTurnover = currentPeriodDtos.Sum(_ => _.Turnover) - sumCurrentPeriodTurnover;
                sumCurrentPeriodGrossProfit = currentPeriodDtos.Sum(_ => _.GrossProfit) - sumCurrentPeriodGrossProfit;
                sumCurrentPeriodMargin = currentPeriodDtos.Sum(_ => _.Margin) - sumCurrentPeriodMargin;

                sumPrevPeriodTurnover = prevPeriodDtos.Sum(_ => _.Turnover) - sumPrevPeriodTurnover;
                sumPrevPeriodGrossProfit = prevPeriodDtos.Sum(_ => _.GrossProfit) - sumPrevPeriodGrossProfit;
                sumPrevPeriodMargin = prevPeriodDtos.Sum(_ => _.Margin) - sumPrevPeriodMargin;

                var vmItem = new MeasurementComparisonVM
                {
                    Name = DISTRIBUTION_OTHERS_NAME,
                    DifferenceTurnover = CreateDifferenceMeasurement(sumPrevPeriodTurnover, sumCurrentPeriodTurnover, sumAbsPeriodTurnover),
                    DifferenceGrossProfit = CreateDifferenceMeasurement(sumPrevPeriodGrossProfit, sumCurrentPeriodGrossProfit, sumAbsPeriodGrossProfit),
                    DifferenceMargin = CreateDifferenceMeasurement(sumPrevPeriodMargin, sumCurrentPeriodMargin, (0, 0))
                };

                result.Add(vmItem);
            }

            return result;
        }

        public IEnumerable<MeasurementPeriodComparisonGrowthVM> ToMeasurementGrowthPeriodVM<T>(
            IEnumerable<T> prevPeriodDtos,
            IEnumerable<T> currentPeriodDtos,
            IEnumerable<(string topDistributionId, string topDistributionName)> topDistributions
            )
            where T : MeasurementPeriodDTO
        {
            var result = new List<MeasurementPeriodComparisonGrowthVM>(topDistributions.Count());
            var prevPeriodDtoDic = prevPeriodDtos.ToDictionary(_ => _.Id, _ => _);
            var currentPeriodDtoDic = currentPeriodDtos.ToDictionary(_ => _.Id, _ => _);

            var hasOthers = HasOthers(topDistributions);

            foreach (var topDistribution in topDistributions)
            {
                (string topDistributionId, string topDistributionName) = topDistribution;

                if (topDistributionId == DISTRIBUTION_OTHERS_ID)
                {
                    continue;
                }

                var currentPeriodDto = currentPeriodDtoDic[topDistributionId];

                var previousPeriodTotal = prevPeriodDtoDic.ContainsKey(topDistributionId) ? prevPeriodDtoDic[topDistributionId].Total : 0;
                var currentPeriodTotal = currentPeriodDto.Total;

                var vm = CreateMeasurementPeriodComparisonGrowthVM(topDistributionId, topDistributionName, previousPeriodTotal, currentPeriodTotal);
                result.Add(vm);

                if (hasOthers)
                {
                    currentPeriodDtoDic.Remove(topDistributionId);
                    prevPeriodDtoDic.Remove(topDistributionId);
                }
            }

            if (hasOthers)
            {
                result.Add(CreateMeasurementPeriodComparisonGrowthVM(
                        DISTRIBUTION_OTHERS_ID,
                        DISTRIBUTION_OTHERS_NAME,
                        prevPeriodDtoDic.Sum(_ => _.Value.Total),
                        currentPeriodDtoDic.Sum(_ => _.Value.Total),
                        prevPeriodDtoDic.Select(_ => new IdentifyVM { Id = _.Value.Id, Name = _.Value.Name }),
                        currentPeriodDtoDic.Select(_ => new IdentifyVM { Id = _.Value.Id, Name = _.Value.Name })
                        ));
            }

            return result;
        }

        private DistributionVM ToDistributionVM<T>(IEnumerable<T> dtos, IEnumerable<(string topDistributionId, string topDistributionName)> topDistributions, string name)
            where T : DistributionDTO
        {
            var listDto = dtos.ToList();
            var sumItem = listDto.FirstOrDefault(_ => _.IsSum);
            listDto.Remove(sumItem);

            var sumAbs = listDto.Sum(_ => Math.Abs(_.Total));
            var dtoDic = listDto.ToDictionary(_ => _.Id, _ => _);
            var calculatedDistribution = listDto.Exists(_ => !_.Distribution.Equals(default));

            var result = new DistributionVM(topDistributions.Count() + 1)
            {
                Name = name,
                Total = _calculationService.ToRound(sumItem?.Total)
            };

            foreach (var topDistribution in topDistributions)
            {
                (string topDistributionId, string topDistributionName) = topDistribution;

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
                    distributionItem.Percent = calculatedDistribution ?
                        _calculationService.ToPercentUnit(distributionDto.Distribution) : _calculationService.CalculateDistribution(distributionDto.Total, sumAbs);
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
                    Percent = calculatedDistribution ? _calculationService.ToPercentUnit(dtoDic.Sum(_ => _.Value.Distribution))
                    : _calculationService.CalculateDistribution(dtoDic.Sum(_ => _.Value.Total), sumAbs)
                });
            }

            return result;
        }

        private MeasurementPeriodComparisonGrowthVM CreateMeasurementPeriodComparisonGrowthVM(
            in string id,
            in string name,
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

        private MeasurementComparisonVM CreateMeasurementComparisonVM(
            in string name,
            MeasurementComparisonDTO prevPeriodDto,
            MeasurementComparisonDTO currentPeriodDto,
            in ValueTuple<double, double> sumAbsPeriodTurnover,
            in ValueTuple<double, double> sumAbsPeriodGrossProfit
            )
        {
            (double sumAbsPrevPeriodTurnover, double sumAbsCurrentPeriodTurnover) = sumAbsPeriodTurnover;
            (double sumAbsPrevPeriodGrossProfit, double sumAbsCurrentPeriodGrossProfit) = sumAbsPeriodGrossProfit;

            return new MeasurementComparisonVM
            {
                Name = name,
                DifferenceTurnover = CreateDifferenceMeasurement(prevPeriodDto?.Turnover, currentPeriodDto.Turnover, (sumAbsPrevPeriodTurnover, sumAbsCurrentPeriodTurnover)),
                DifferenceGrossProfit = CreateDifferenceMeasurement(prevPeriodDto?.GrossProfit, currentPeriodDto.GrossProfit, (sumAbsPrevPeriodGrossProfit, sumAbsCurrentPeriodGrossProfit)),
                DifferenceMargin = CreateDifferenceMeasurement(prevPeriodDto?.Margin, currentPeriodDto.Margin, (0, 0))
            };
        }

        private DifferenceMeasurement CreateDifferenceMeasurement(
            double? prevPeriodMeasurement,
            double currentPeriodMeasurement,
            in ValueTuple<double, double> sumAbsPeriodMeasurement
            )
        {
            (double sumAbsPrevPeriodMeasurement, double sumAbsCurrentPeriodMeasurement) = sumAbsPeriodMeasurement;

            return new DifferenceMeasurement
            {
                DifferenceTotal = _calculationService.ToRound(currentPeriodMeasurement - (prevPeriodMeasurement ?? 0)),
                DifferenceDistribution = _calculationService.ToRound(_calculationService.CalculateDistribution(currentPeriodMeasurement, sumAbsCurrentPeriodMeasurement)
                        - _calculationService.CalculateDistribution(prevPeriodMeasurement, sumAbsPrevPeriodMeasurement)),
            };
        }

        private static ValueTuple<double, double, double> UpdateSumCurrentMeasurement(in ValueTuple<double, double, double> measurements, MeasurementComparisonDTO dto)
        {
            (double sumTurnover, double sumGrossProfit, double sumMargin) = measurements;
            sumTurnover += dto?.Turnover ?? 0;
            sumGrossProfit += dto?.GrossProfit ?? 0;
            sumMargin += dto?.Margin ?? 0;

            return (sumTurnover, sumGrossProfit, sumMargin);
        }
    }
}