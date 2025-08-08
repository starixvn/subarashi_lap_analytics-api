namespace Fanex.Athena.AnalyticsAPI.Performance.UnitTests.Mappers
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoFixture;
    using Common.Services;
    using Moq;
    using Newtonsoft.Json;
    using Performance.Mappers;
    using Performance.Models.DTO;
    using Performance.Models.ViewModels;
    using Xunit;

    public class OverviewPerformanceVMMapper_ToSummaryVM_Test
    {
        private readonly Fixture _fixture;
        private readonly Mock<ICalculationService> _calculationService;
        private readonly OverviewPerformanceVMMapper _target;

        public OverviewPerformanceVMMapper_ToSummaryVM_Test()
        {
            _fixture = new Fixture();
            _calculationService = new Mock<ICalculationService>();
            _target = new OverviewPerformanceVMMapper(_calculationService.Object);

            _calculationService.Setup(_ => _.ToRound(It.IsAny<double?>(), 2))
                .Returns((double? value, int numberOfRound) => value ?? 0);

            _calculationService.Setup(_ => _.CalculateGrowth(It.IsAny<double?>(), It.IsAny<double?>()))
              .Returns((double? current, double? previous) => (current ?? 0) + (previous ?? 0));
        }

        [Fact]
        public void ToSummaryVM_WhenCall_ReturnCorrectResult()
        {
            //Arrange
            var prevPeriodData = new List<SummaryDTO> { new SummaryDTO { Turnover = 99.1235, GrossProfit = -1235.23, Margin = 1.23 } };
            var currentPeriodData = new List<SummaryDTO> { new SummaryDTO { Turnover = 81239.15545, GrossProfit = -1000.23, Margin = -0.5 } };

            var expected = new List<SummaryVM>
            {
                new SummaryVM
                {
                    MeasurementId = 1,
                    Total = 81239.15545,
                    ChangePercent = 81239.15545 + 99.1235
                },
                new SummaryVM
                {
                    MeasurementId = 2,
                    Total = -1000.23,
                    ChangePercent =  -1235.23 - 1000.23
                },
                new SummaryVM
                {
                    MeasurementId = 3,
                    Total = -50,
                    ChangePercent = -50 + 123
                }
            };

            //Act
            var result = _target.ToSummaryVM(prevPeriodData, currentPeriodData);

            //Assert
            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void ToSummaryVM_DataIsEmpty_ReturnCorrectResult()
        {
            //Arrange
            var prevPeriodData = Enumerable.Empty<SummaryDTO>();
            var currentPeriodData = Enumerable.Empty<SummaryDTO>();

            var expected = new List<SummaryVM>
            {
                new SummaryVM
                {
                    MeasurementId = 1,
                    Total = 0,
                    ChangePercent = 0
                },
                new SummaryVM
                {
                    MeasurementId = 2,
                    Total = 0,
                    ChangePercent = 0
                },
                new SummaryVM
                {
                    MeasurementId = 3,
                    Total = 0,
                    ChangePercent = 0
                }
            };

            //Act
            var result = _target.ToSummaryVM(prevPeriodData, currentPeriodData);

            //Assert
            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(result));
        }
    }
}