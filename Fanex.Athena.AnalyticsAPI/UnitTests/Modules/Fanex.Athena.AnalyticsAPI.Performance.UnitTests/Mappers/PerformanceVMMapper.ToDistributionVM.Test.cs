namespace Fanex.Athena.AnalyticsAPI.Performance.UnitTests.Mappers
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoFixture;
    using Common.Services;
    using Moq;
    using Performance.Mappers;
    using Performance.Models.MetaModels;
    using Xunit;

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
            var topDistributions = Enumerable.Empty<DistributionMetaModel>();

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
            var topDistributions = Enumerable.Empty<DistributionMetaModel>();

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