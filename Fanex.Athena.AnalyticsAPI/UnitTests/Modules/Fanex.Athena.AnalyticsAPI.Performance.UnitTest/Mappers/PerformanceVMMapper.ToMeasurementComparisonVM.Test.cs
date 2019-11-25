using AutoFixture;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Mappers;
using Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Performance.UnitTest.Mappers
{
    public class PerformanceVMMapper_ToMeasurementComparisonVM_Test
    {
        private readonly Fixture _fixture;
        private readonly Mock<ICalculationService> _calculationService;
        private readonly PerformanceVMMapper _target;

        public PerformanceVMMapper_ToMeasurementComparisonVM_Test()
        {
            _fixture = new Fixture();
            _calculationService = new Mock<ICalculationService>();
            _target = new PerformanceVMMapper(_calculationService.Object);

            _calculationService.Setup(_ => _.ToRound(It.IsAny<double?>(), 2))
                .Returns((double? value, int numberOfRound) => value ?? 0);
            _calculationService.Setup(_ => _.ToPercentUnit(It.IsAny<double?>()))
               .Returns((double? value) => value ?? 0);
        }

        [Fact]
        public void ToMeasurementComparisonVM_TopDistributionIsEmpty_ReturnEmpty()
        {
            //Arrange
            var prevDtos = FakePreviousMeasurementComparisonDtos();
            var currentDtos = FakeCurrentMeasurementComparisonDtos();
            var topDistributions = Enumerable.Empty<ValueTuple<string, string>>();

            //Act
            var result = _target.ToMeasurementComparisonVM(prevDtos, currentDtos, topDistributions);

            //Assert
            Assert.Empty(result);
        }

        [Fact]
        public void ToMeasurementComparisonVM_WhenCall_ReturnCorrectResult()
        {
            //Arrange
            var prevDtos = FakePreviousMeasurementComparisonDtos();
            var currentDtos = FakeCurrentMeasurementComparisonDtos();
            var topDistributions = FakeTopDistributions();

            var expected = new List<MeasurementComparisonVM>
            {
                new MeasurementComparisonVM
                {
                    Name = "distribution1",
                    DifferenceMargin = new DifferenceMeasurement { DifferenceTotal = 4.7, DifferenceDistribution = 0 },
                    DifferenceGrossProfit = new DifferenceMeasurement { DifferenceTotal = 983730, DifferenceDistribution = 0 },
                    DifferenceTurnover = new DifferenceMeasurement { DifferenceTotal = -8200, DifferenceDistribution = -30.000000000000004 }
                },
                new MeasurementComparisonVM
                {
                    Name = "distribution2",
                    DifferenceMargin = new DifferenceMeasurement { DifferenceTotal = 0, DifferenceDistribution = 0 },
                    DifferenceGrossProfit = new DifferenceMeasurement { DifferenceTotal = -79100, DifferenceDistribution = -14.399999999999999 },
                    DifferenceTurnover = new DifferenceMeasurement { DifferenceTotal = -5921000, DifferenceDistribution = 20.000000000000004 }
                },
                new MeasurementComparisonVM
                {   Name = "distribution3",
                    DifferenceMargin = new DifferenceMeasurement { DifferenceTotal = 0.5, DifferenceDistribution = 0 },
                    DifferenceGrossProfit = new DifferenceMeasurement { DifferenceTotal = 8823.255, DifferenceDistribution = 12.2 },
                    DifferenceTurnover = new DifferenceMeasurement { DifferenceTotal = 91230.23, DifferenceDistribution = 39.2 }
                },
                new MeasurementComparisonVM
                {   Name = "distribution4",
                    DifferenceMargin = new DifferenceMeasurement { DifferenceTotal = 1.5, DifferenceDistribution = 0 },
                    DifferenceGrossProfit = new DifferenceMeasurement { DifferenceTotal = 3501254.255, DifferenceDistribution = 12.2 },
                    DifferenceTurnover = new DifferenceMeasurement { DifferenceTotal = 91230.23, DifferenceDistribution = 39.2 }
                },
                new MeasurementComparisonVM
                {   Name = "Others",
                    DifferenceMargin = new DifferenceMeasurement { DifferenceTotal = -5.35 },
                    DifferenceGrossProfit = new DifferenceMeasurement { DifferenceTotal = 39923.255000000005, DifferenceDistribution = 30.200000000000006 },
                    DifferenceTurnover = new DifferenceMeasurement { DifferenceTotal = 124330.23, DifferenceDistribution = 39.2 }
                }
            };

            //Act
            var result = _target.ToMeasurementComparisonVM(prevDtos, currentDtos, topDistributions);

            //Assert
            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(result));
        }

        internal static IEnumerable<(string topDistributionId, string topDistributionName)> FakeTopDistributions()
        {
            return new List<(string topDistributionId, string topDistributionName)>
            {
                ("1", "distribution1"),
                ("2", "distribution2"),
                ("3", "distribution3"),
                ("4", "distribution4"),
                ("5", "distribution5"),
                ("0", "Others")
            };
        }

        internal static IEnumerable<MeasurementComparisonDtoTestClass> FakePreviousMeasurementComparisonDtos()
        {
            return new List<MeasurementComparisonDtoTestClass>
            {
                new MeasurementComparisonDtoTestClass
                {
                    TestId = "1",
                    TestName = "distribution1",
                    Margin = -1.2,
                    Turnover = 9130.23,
                    TurnoverDistribution = 39.2,
                    GrossProfit = 8823.255,
                    GrossProfitDistribution = 12.2
                },
                new MeasurementComparisonDtoTestClass
                {
                    TestId = "2",
                    TestName = "distribution2",
                    Margin = 0.5,
                    Turnover = 6012230.23,
                    TurnoverDistribution = 19.2,
                    GrossProfit = 88023.255,
                    GrossProfitDistribution = 2.2
                },
                new MeasurementComparisonDtoTestClass
                {
                    TestId = "7",
                    TestName = "distribution7",
                    Margin = 3.5,
                    Turnover = 230.23,
                    TurnoverDistribution = 39.2,
                    GrossProfit = 68823.255,
                    GrossProfitDistribution = 22.2
                }
            };
        }

        internal static IEnumerable<MeasurementComparisonDtoTestClass> FakeCurrentMeasurementComparisonDtos()
        {
            return new List<MeasurementComparisonDtoTestClass>
            {
                new MeasurementComparisonDtoTestClass
                {
                    TestId = "1",
                    TestName = "distribution1",
                    Margin = 3.5,
                    Turnover = 930.23,
                    TurnoverDistribution = 9.2,
                    GrossProfit = 992553.255,
                    GrossProfitDistribution = 12.2
                },
                new MeasurementComparisonDtoTestClass
                {
                    TestId = "2",
                    TestName = "distribution2",
                    Margin = 0.5,
                    Turnover = 91230.23,
                    TurnoverDistribution = 39.2,
                    GrossProfit = 8923.255,
                    GrossProfitDistribution = -12.2
                },
                new MeasurementComparisonDtoTestClass
                {
                    TestId = "3",
                    TestName = "distribution3",
                    Margin = 0.5,
                    Turnover = 91230.23,
                    TurnoverDistribution = 39.2,
                    GrossProfit = 8823.255,
                    GrossProfitDistribution = 12.2
                },
                new MeasurementComparisonDtoTestClass
                {
                    TestId = "4",
                    TestName = "distribution4",
                    Margin = 1.5,
                    Turnover = 91230.23,
                    TurnoverDistribution = 39.2,
                    GrossProfit = 3501254.255,
                    GrossProfitDistribution = 12.2
                },
                new MeasurementComparisonDtoTestClass
                {
                    TestId = "6",
                    TestName = "distribution6",
                    Margin = -2.35,
                    Turnover = 33330.23,
                    TurnoverDistribution = 39.2,
                    GrossProfit = 99923.255,
                    GrossProfitDistribution = 40.2
                },
                new MeasurementComparisonDtoTestClass
                {
                    TestId = "7",
                    TestName = "distribution7",
                    Margin = 0.5,
                    Turnover = 91230.23,
                    TurnoverDistribution = 39.2,
                    GrossProfit = 8823.255,
                    GrossProfitDistribution = 12.2
                }
            };
        }
    }
}