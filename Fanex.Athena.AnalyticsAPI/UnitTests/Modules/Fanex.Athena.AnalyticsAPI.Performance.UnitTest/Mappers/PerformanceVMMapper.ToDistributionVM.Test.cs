using AutoFixture;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Mappers;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Performance.UnitTest.Mappers
{
    public class PerformanceVMMapper_ToDistributionVM_Test
    {
        private readonly Fixture _fixture;
        private readonly Mock<ICalculationService> _calculationService;
        private readonly PerformanceVMMapper _target;

        public PerformanceVMMapper_ToDistributionVM_Test()
        {
            _fixture = new Fixture();
            _calculationService = new Mock<ICalculationService>();
            _target = new PerformanceVMMapper(_calculationService.Object);
        }

        [Fact]
        public void ToDistributionVM_WhenCall_ReturnTwoElement()
        {
            //Arrange
            //Act
            var result = _target.ToDistributionVM(_fixture.CreateMany<DistributionTestClass>(),
                _fixture.CreateMany<DistributionTestClass>(),
                  _fixture.CreateMany<(string topDistributionId, string topDistributionName)>());

            //Assert
            Assert.Equal(2, result.Count());
            Assert.Equal("Previous Period", result.ElementAtOrDefault(0)?.Name);
            Assert.Equal("Current Period", result.ElementAtOrDefault(1)?.Name);
        }

        [Fact]
        public void ToDistributionVM_WhenCall_ReturnCorrectResult()
        {
            //Arrange
            var prevDtos = FakePreviousDistributionDTOs();
            var currentDtos = FakeCurrentDistributionDTOs();
            var topDistributions = FakeTopDistributions();

            _calculationService.Setup(_ => _.ToRound(It.IsAny<double>(), 2))
                .Returns((double value, int numberOfRound) => value);
            _calculationService.Setup(_ => _.ToPercentUnit(It.IsAny<double>()))
               .Returns((double? value) => value ?? 0);

            //Act
            var result = _target.ToDistributionVM(prevDtos, currentDtos, topDistributions);
            var prevVM = result.ElementAtOrDefault(0);
            var currentVM = result.ElementAtOrDefault(1);

            //Assert
            Assert.Equal(9500.50, prevVM.Total);
            Assert.Equal(9900.50, currentVM.Total);

            Assert.Equal(topDistributions.Count(), currentVM.Distributions.Count);
            Assert.Equal(topDistributions.Count(), prevVM.Distributions.Count);

            Assert.Equal(0.65, prevVM.Distributions.FirstOrDefault(_ => _.Id == "0")?.Percent);
            Assert.Equal(0.5, currentVM.Distributions.FirstOrDefault(_ => _.Id == "0")?.Percent);
        }

        public static IEnumerable<(string topDistributionId, string topDistributionName)> FakeTopDistributions()
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

        public static IEnumerable<DistributionTestClass> FakePreviousDistributionDTOs()
        {
            return new List<DistributionTestClass>
            {
                new DistributionTestClass
                {
                    IsSum = true,
                    Total = 9500.50
                },
                new DistributionTestClass
                {
                    Distribution = 0.35,
                    TestId = "1"
                },
                new DistributionTestClass
                {
                    Distribution = 0.2,
                    TestId = "2"
                },
                new DistributionTestClass
                {
                    Distribution = 0.1,
                    TestId = "3"
                },
                new DistributionTestClass
                {
                    Distribution = 0.65,
                    TestId = "7"
                }
            };
        }

        public static IEnumerable<DistributionTestClass> FakeCurrentDistributionDTOs()
        {
            return new List<DistributionTestClass>
            {
                new DistributionTestClass
                {
                    IsSum = true,
                    Total = 9900.50
                },
                new DistributionTestClass
                {
                    Distribution = 0.35,
                    TestId = "1"
                },
                new DistributionTestClass
                {
                    Distribution = 0.2,
                    TestId = "2"
                },
                new DistributionTestClass
                {
                    Distribution = 0.1,
                    TestId = "3"
                },
                new DistributionTestClass
                {
                    Distribution = 0.3,
                    TestId = "4"
                },
                new DistributionTestClass
                {
                    Distribution = 0.5,
                    TestId = "5"
                },
                new DistributionTestClass
                {
                    Distribution = 0.3,
                    TestId = "6"
                },
                new DistributionTestClass
                {
                    Distribution = 0.2,
                    TestId = "7"
                }
            };
        }
    }
}