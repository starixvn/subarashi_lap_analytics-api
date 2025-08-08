namespace Fanex.Athena.AnalyticsAPI.Performance.UnitTests.Mappers
{
    using System;
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

    public class PerformanceVMMapper_ToMeasurementGrowthTimeVM_Test
    {
        private readonly Fixture _fixture;
        private readonly Mock<ICalculationService> _calculationService;
        private readonly PerformanceVMMapper _target;

        public PerformanceVMMapper_ToMeasurementGrowthTimeVM_Test()
        {
            _fixture = new Fixture();
            _calculationService = new Mock<ICalculationService>();
            _target = new PerformanceVMMapper(_calculationService.Object);

            _calculationService.Setup(_ => _.ToRound(It.IsAny<double?>(), 2))
                .Returns((double? value, int numberOfRound) => value ?? 0);
            _calculationService.Setup(_ => _.CalculateGrowth(It.IsAny<double?>(), It.IsAny<double?>()))
               .Returns((double? current, double? previous) => (current ?? 0) + (previous ?? 0));
        }

        [Theory]
        [MemberData(nameof(FakePeriods))]
        public void ToMeasurementGrowthTimeVM_WhenCall_ReturnCorrectNumberOfMonthsAndMonthKeys(DateTime fromDate, DateTime toDate, List<string> monthKeys)
        {
            //Arrange
            var data = FakeMeasurementPeriodDTOs();

            //Act
            var result = _target.ToMeasurementGrowthTimeVM(data, fromDate, toDate);

            //Assert
            Assert.Equal(monthKeys.Count, result.Count());
            for (int i = 0; i < result.Count(); i++)
            {
                Assert.Equal(monthKeys[i], result.ElementAt(i).Name);
            }
        }

        [Fact]
        public void ToMeasurementGrowthTimeVM_DataIsEmpty_ResultIsDefaultValues()
        {
            //Arrange
            var data = Enumerable.Empty<MeasurementTimeDTO>();
            var fromDate = new DateTime(2019, 1, 20);
            var toDate = new DateTime(2019, 12, 5);

            //Act
            var result = _target.ToMeasurementGrowthTimeVM(data, fromDate, toDate);

            //Assert
            Assert.True(result.All(_ => _.Total == default(double) && _.GrowthPercent == default(double)));
        }

        [Fact]
        public void ToMeasurementGrowthTimeVM_WhenCall_ReturnCorrectMonthKeys()
        {
            //Arrange
            var data = FakeMeasurementPeriodDTOs();
            var fromDate = new DateTime(2019, 1, 20);
            var toDate = new DateTime(2019, 5, 5);

            var expected = new List<MeasurementGrowthVM>
            {
                new MeasurementGrowthVM { Name = "01/2019", Total = 1500, GrowthPercent = 2000 },
                new MeasurementGrowthVM { Name = "02/2019", Total = 500, GrowthPercent = 2000 },
                new MeasurementGrowthVM { Name = "03/2019", Total = -500, GrowthPercent = 0 },
                new MeasurementGrowthVM { Name = "04/2019", Total = 0, GrowthPercent = -500 },
                new MeasurementGrowthVM { Name = "05/2019", Total = 120.55, GrowthPercent = 120.55 },
            };

            //Act
            var result = _target.ToMeasurementGrowthTimeVM(data, fromDate, toDate);

            //Assert
            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(result));
        }

        internal static IEnumerable<MeasurementTimeDTO> FakeMeasurementPeriodDTOs()
        {
            return new List<MeasurementTimeDTO>
            {
                new MeasurementTimeDTO
                {
                    Time = new DateTime(2018, 12, 1),
                    Total = 500
                },
                new MeasurementTimeDTO
                {
                    Time = new DateTime(2019, 1, 1),
                    Total = 1500
                },
                new MeasurementTimeDTO
                {
                    Time = new DateTime(2019, 2, 1),
                    Total = 500
                },
                new MeasurementTimeDTO
                {
                    Time = new DateTime(2019, 3, 1),
                    Total = -500
                },
                new MeasurementTimeDTO
                {
                    Time = new DateTime(2019, 5, 1),
                    Total = 120.55
                }
            };
        }

        public static IEnumerable<object[]> FakePeriods()
        {
            yield return new object[]
            {
                new DateTime(2018, 12, 20),
                new DateTime(2018, 12, 20),
                new List<string> { "12/2018" }
            };
            yield return new object[]
            {
                new DateTime(2018, 12, 20),
                new DateTime(2018, 11, 20),
                new List<string>()
            };
            yield return new object[]
            {
                new DateTime(2019, 1, 20),
                new DateTime(2019, 12, 5),
                new List<string> { "01/2019", "02/2019", "03/2019", "04/2019", "05/2019", "06/2019", "07/2019", "08/2019", "09/2019", "10/2019", "11/2019", "12/2019" }
            };
            yield return new object[]
            {
                new DateTime(2018, 3, 20),
                new DateTime(2019, 1, 11),
                new List<string> { "03/2018", "04/2018", "05/2018", "06/2018", "07/2018", "08/2018", "09/2018", "10/2018", "11/2018", "12/2018", "01/2019" }
            };
        }
    }
}