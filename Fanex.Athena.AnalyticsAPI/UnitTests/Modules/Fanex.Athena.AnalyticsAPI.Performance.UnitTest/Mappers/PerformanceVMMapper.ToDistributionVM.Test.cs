using AutoFixture;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Mappers;
using Moq;
using System;
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

            _calculationService.Setup(_ => _.ToRound(It.IsAny<double?>(), 2))
                .Returns((double? value, int numberOfRound) => value ?? 0);
            _calculationService.Setup(_ => _.ToPercentUnit(It.IsAny<double?>()))
               .Returns((double? value) => value ?? 0);
        }

        [Fact]
        public void ToDistributionVM_WhenCall_ReturnCorrect_TwoElement_NameElement()
        {
            //Arrange
            var prevDtos = _fixture.CreateMany<DistributionDtoTestClass>();
            var currentDtos = _fixture.CreateMany<DistributionDtoTestClass>();
            var topDistributions = Enumerable.Empty<ValueTuple<string, string>>();

            //Act
            var result = _target.ToDistributionVM(prevDtos, currentDtos, topDistributions);

            //Assert
            Assert.Equal(2, result.Count());
            Assert.Equal("Previous Period", result.ElementAtOrDefault(0)?.Name);
            Assert.Equal("Current Period", result.ElementAtOrDefault(1)?.Name);
        }

        [Fact]
        public void ToDistributionVM_TopDistributionIsEmpty_ReturnDistributionsAsEmpty()
        {
            //Arrange
            var prevDtos = FakePreviousDistributionDTOs();
            var currentDtos = FakeCurrentDistributionDTOs();
            var topDistributions = Enumerable.Empty<ValueTuple<string, string>>();

            //Act
            var result = _target.ToDistributionVM(prevDtos, currentDtos, topDistributions);

            //Assert
            Assert.Empty(result.ElementAtOrDefault(0).Distributions);
            Assert.Empty(result.ElementAtOrDefault(1).Distributions);
        }

        [Fact]
        public void ToDistributionVM_WhenCall_ReturnCorrectResult()
        {
            //Arrange
            var prevDtos = FakePreviousDistributionDTOs();
            var currentDtos = FakeCurrentDistributionDTOs();
            var topDistributions = FakeTopDistributions();

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

        internal static IEnumerable<DistributionDtoTestClass> FakePreviousDistributionDTOs()
        {
            return new List<DistributionDtoTestClass>
            {
                new DistributionDtoTestClass
                {
                    IsSum = true,
                    Total = 9500.50
                },
                new DistributionDtoTestClass
                {
                    Distribution = 0.35,
                    TestId = "1"
                },
                new DistributionDtoTestClass
                {
                    Distribution = 0.2,
                    TestId = "2"
                },
                new DistributionDtoTestClass
                {
                    Distribution = 0.1,
                    TestId = "3"
                },
                new DistributionDtoTestClass
                {
                    Distribution = 0.65,
                    TestId = "7"
                }
            };
        }

        internal static IEnumerable<DistributionDtoTestClass> FakeCurrentDistributionDTOs()
        {
            return new List<DistributionDtoTestClass>
            {
                new DistributionDtoTestClass
                {
                    IsSum = true,
                    Total = 9900.50
                },
                new DistributionDtoTestClass
                {
                    Distribution = 0.35,
                    TestId = "1"
                },
                new DistributionDtoTestClass
                {
                    Distribution = 0.2,
                    TestId = "2"
                },
                new DistributionDtoTestClass
                {
                    Distribution = 0.1,
                    TestId = "3"
                },
                new DistributionDtoTestClass
                {
                    Distribution = 0.3,
                    TestId = "4"
                },
                new DistributionDtoTestClass
                {
                    Distribution = 0.5,
                    TestId = "5"
                },
                new DistributionDtoTestClass
                {
                    Distribution = 0.3,
                    TestId = "6"
                },
                new DistributionDtoTestClass
                {
                    Distribution = 0.2,
                    TestId = "7"
                }
            };
        }
    }
}