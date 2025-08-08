namespace Fanex.Athena.AnalyticsAPI.Performance.UnitTests.Mappers
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoFixture;
    using Common.Services;
    using Moq;
    using Newtonsoft.Json;
    using Performance.Mappers;
    using Performance.Models.MetaModels;
    using Performance.Models.ViewModels;
    using Xunit;

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
            var topDistributions = Enumerable.Empty<DistributionMetaModel>();

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
                    Id = "1",
                    Name = "distribution1",
                    DifferenceMargin = new DifferenceMeasurement { DifferenceTotal = 470, DifferenceDistribution = 0 },
                    DifferenceGrossProfit = new DifferenceMeasurement { DifferenceTotal = 983730, DifferenceDistribution = 0 },
                    DifferenceTurnover = new DifferenceMeasurement { DifferenceTotal = -8200, DifferenceDistribution = -30.000000000000004 }
                },
                new MeasurementComparisonVM
                {
                    Id = "2",
                    Name = "distribution2",
                    DifferenceMargin = new DifferenceMeasurement { DifferenceTotal = 0, DifferenceDistribution = 0 },
                    DifferenceGrossProfit = new DifferenceMeasurement { DifferenceTotal = -79100, DifferenceDistribution = -14.399999999999999 },
                    DifferenceTurnover = new DifferenceMeasurement { DifferenceTotal = -5921000, DifferenceDistribution = 20.000000000000004 }
                },
                new MeasurementComparisonVM
                {
                    Id = "3",
                    Name = "distribution3",
                    DifferenceMargin = new DifferenceMeasurement { DifferenceTotal = 50, DifferenceDistribution = 0 },
                    DifferenceGrossProfit = new DifferenceMeasurement { DifferenceTotal = 8823.255, DifferenceDistribution = 12.2 },
                    DifferenceTurnover = new DifferenceMeasurement { DifferenceTotal = 91230.23, DifferenceDistribution = 39.2 }
                },
                new MeasurementComparisonVM
                {
                    Id = "4",
                    Name = "distribution4",
                    DifferenceMargin = new DifferenceMeasurement { DifferenceTotal = 150, DifferenceDistribution = 0 },
                    DifferenceGrossProfit = new DifferenceMeasurement { DifferenceTotal  = 3501254.255, DifferenceDistribution = 12.2 },
                    DifferenceTurnover = new DifferenceMeasurement { DifferenceTotal = 91230.23, DifferenceDistribution = 39.2 }
                },
                new MeasurementComparisonVM
                {
                    Id = "0",
                    Name = "Others",
                    DifferenceMargin = new DifferenceMeasurement { DifferenceTotal = 10 },
                    DifferenceGrossProfit = new DifferenceMeasurement { DifferenceTotal = 39923.255000000005, DifferenceDistribution = 30.200000000000006 },
                    DifferenceTurnover = new DifferenceMeasurement { DifferenceTotal = 124330.23, DifferenceDistribution = 39.2 }
                }
            };

            //Act
            var result = _target.ToMeasurementComparisonVM(prevDtos, currentDtos, topDistributions);

            //Assert
            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(result));
        }

        internal static IEnumerable<DistributionMetaModel> FakeTopDistributions()
        {
            return new List<DistributionMetaModel>
            {
                new DistributionMetaModel { DisplayId = "1", DisplayName = "distribution1" },
                new DistributionMetaModel { DisplayId = "2", DisplayName = "distribution2" },
                new DistributionMetaModel { DisplayId = "3", DisplayName = "distribution3" },
                new DistributionMetaModel { DisplayId = "4", DisplayName = "distribution4" },
                new DistributionMetaModel { DisplayId = "5", DisplayName = "distribution5" },
                new DistributionMetaModel { DisplayId = "0", DisplayName = "Others" }
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
                },
                new MeasurementComparisonDtoTestClass
                {
                    TestId = "0",
                    TestName = "Others",
                    Margin = 0.05,
                    Turnover = 0
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
                },
                new MeasurementComparisonDtoTestClass
                {
                    TestId = "0",
                    TestName = "Others",
                    Margin = 0.15
                }
            };
        }
    }
}