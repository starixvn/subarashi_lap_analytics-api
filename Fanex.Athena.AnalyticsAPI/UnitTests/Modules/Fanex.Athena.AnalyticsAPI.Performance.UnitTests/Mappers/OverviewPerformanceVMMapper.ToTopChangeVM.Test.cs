using AutoFixture;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Mappers;
using Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels;
using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Performance.UnitTests.Mappers
{
    public class OverviewPerformanceVMMapper_ToTopChangeVM_Test
    {
        private readonly Fixture _fixture;
        private readonly Mock<ICalculationService> _calculationService;
        private readonly OverviewPerformanceVMMapper _target;

        public OverviewPerformanceVMMapper_ToTopChangeVM_Test()
        {
            _fixture = new Fixture();
            _calculationService = new Mock<ICalculationService>();
            _target = new OverviewPerformanceVMMapper(_calculationService.Object);

            _calculationService.Setup(_ => _.ToRound(It.IsAny<double?>(), 2))
                .Returns((double? value, int numberOfRound) => value ?? 0);

            _calculationService.Setup(_ => _.CalculateGrowth(It.IsAny<double?>(), It.IsAny<double?>()))
              .Returns((double? current, double? previous) => (current ?? 0) - (previous ?? 0));
        }

        [Fact]
        public void ToTopChangeVM_DataIsEmpty_ReturnEmpty()
        {
            //Arrange
            var prevPeriodData = Enumerable.Empty<TopChangeDtoTestClass>();
            var currentPeriodData = Enumerable.Empty<TopChangeDtoTestClass>();

            //Act
            var result = _target.ToTopChangeVM(prevPeriodData, currentPeriodData);

            //Assert
            Assert.Empty(result);
        }

        [Fact]
        public void ToTopChangeVM_WhenCall_ReturnCorrectResult()
        {
            //Arrange
            var prevPeriodData = FakePreviousTopChangeDtos();
            var currentPeriodData = FakeCurrentTopChangeDtos();

            var expected = new List<TopChangeVM>
            {
                new TopChangeVM
                {
                    PerspectiveId = "4",
                    PerspectiveName = "name4",
                    Total = -80000,
                    ChangePercent = -80000
                },
                new TopChangeVM
                {
                    PerspectiveId = "6",
                    PerspectiveName = "name6",
                    Total = 19999,
                    ChangePercent = 19999
                },
                new TopChangeVM
                {
                    PerspectiveId = "5",
                    PerspectiveName = "name5",
                    Total = 1000,
                    ChangePercent = 9100
                },
                new TopChangeVM
                {
                    PerspectiveId = "1",
                    PerspectiveName = "name1",
                    Total = 400.25,
                    ChangePercent = -9000
                },
                new TopChangeVM
                {
                    PerspectiveId = "2",
                    PerspectiveName = "name2",
                    Total = 0,
                    ChangePercent = 0
                }
            };

            //Act
            var result = _target.ToTopChangeVM(prevPeriodData, currentPeriodData);

            //Assert
            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(result));
        }

        private static IEnumerable<TopChangeDtoTestClass> FakePreviousTopChangeDtos()
        {
            return new List<TopChangeDtoTestClass>
            {
                new TopChangeDtoTestClass { TestId = "1", TestName = "name1", Total = 9400.25 },
                new TopChangeDtoTestClass { TestId = "2", TestName = "name2", Total = 0 },
                new TopChangeDtoTestClass { TestId = "3", TestName = "name3", Total = 15000 },
                new TopChangeDtoTestClass { TestId = "5", TestName = "name4", Total = -8100 },
            };
        }

        private static IEnumerable<TopChangeDtoTestClass> FakeCurrentTopChangeDtos()
        {
            return new List<TopChangeDtoTestClass>
            {
                new TopChangeDtoTestClass { TestId = "1", TestName = "name1", Total = 400.25 },
                new TopChangeDtoTestClass { TestId = "2", TestName = "name2", Total = 0 },
                new TopChangeDtoTestClass { TestId = "3", TestName = "name3", Total = 15000},
                new TopChangeDtoTestClass { TestId = "4", TestName = "name4", Total = -80000 },
                new TopChangeDtoTestClass { TestId = "5", TestName = "name5", Total = 1000 },
                new TopChangeDtoTestClass { TestId = "6", TestName = "name6", Total = 19999}
            };
        }
    }
}