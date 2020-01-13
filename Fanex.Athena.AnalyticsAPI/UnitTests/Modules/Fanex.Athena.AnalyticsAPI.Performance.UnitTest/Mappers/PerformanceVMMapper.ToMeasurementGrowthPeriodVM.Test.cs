using AutoFixture;
using Fanex.Athena.AnalyticsAPI.Common.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Mappers;
using Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Performance.UnitTest.Mappers
{
    public class PerformanceVMMapper_ToMeasurementGrowthPeriodVM_Test
    {
        private readonly Fixture _fixture;
        private readonly Mock<ICalculationService> _calculationService;
        private readonly PerformanceVMMapper _target;

        public PerformanceVMMapper_ToMeasurementGrowthPeriodVM_Test()
        {
            _fixture = new Fixture();
            _calculationService = new Mock<ICalculationService>();
            _target = new PerformanceVMMapper(_calculationService.Object);

            _calculationService.Setup(_ => _.ToRound(It.IsAny<double>(), 2))
               .Returns((double value, int numberOfRound) => value);
            _calculationService.Setup(_ => _.CalculateGrowth(It.IsAny<double?>(), It.IsAny<double?>()))
               .Returns((double? current, double? previous) => (current ?? 0) + (previous ?? 0));
        }

        [Fact]
        public void ToMeasurementGrowthPeriodVM_TopDistributionIsEmpty_ReturnEmptyResult()
        {
            //Arrange
            var prevDtos = FakePreviouMeasurementPeriodDTOs();
            var currentDtos = FakeCurrentMeasurementPeriodDTOs();
            var topDistributions = Enumerable.Empty<DistributionMetaModel>();

            //Act
            var result = _target.ToMeasurementGrowthPeriodVM(prevDtos, currentDtos, topDistributions);

            //Assert
            Assert.Empty(result);
        }

        [Fact]
        public void ToMeasurementGrowthPeriodVM_ElementOfListOfCurrentPeriodNoMatchWithTopDistribution_ThrowException()
        {
            //Arrange
            var prevDtos = FakePreviouMeasurementPeriodDTOs();
            var currentDtos = new List<MeasurementPerioddDtoTestClass>
            {
                new MeasurementPerioddDtoTestClass
                {
                      Total = _fixture.Create<double>(),
                      TestId = "2"
                },
            };

            var topDistributions = new List<DistributionMetaModel>
            {
                new DistributionMetaModel { DisplayId = "1", DisplayName = "distribution1" }
            };

            //Act
            Action act = () => _target.ToMeasurementGrowthPeriodVM(prevDtos, currentDtos, topDistributions);

            //Assert
            Assert.Throws<KeyNotFoundException>(act);
        }

        [Fact]
        public void ToMeasurementGrowthPeriodVM_WhenCall_ReturnCorrectResult()
        {
            //Arrange
            var prevDtos = FakePreviouMeasurementPeriodDTOs();
            var currentDtos = FakeCurrentMeasurementPeriodDTOs();
            var topDistributions = FakeTopDistributions();

            //Act
            var result = _target.ToMeasurementGrowthPeriodVM(prevDtos, currentDtos, topDistributions);

            //Assert
            Assert.Equal(6, result.Count());

            foreach (var topDistribution in topDistributions)
            {
                if (topDistribution.DisplayId.Equals("0"))
                {
                    continue;
                }

                var element = result.FirstOrDefault(_ => _.Id.Equals(topDistribution.DisplayId));
                var prevPeriodDto = prevDtos.FirstOrDefault(_ => _.Id.Equals(topDistribution.DisplayId));
                var currentPeriodDto = currentDtos.FirstOrDefault(_ => _.Id.Equals(topDistribution.DisplayId));

                Assert.Equal(prevPeriodDto?.Total ?? 0, element.PreviousPeriodTotal);
                Assert.Equal(currentPeriodDto.Total, element.CurrentPeriodTotal);

                Assert.Equal(currentPeriodDto.Total + (prevPeriodDto?.Total ?? 0), element.GrowthPercent);

                Assert.Equal(element.CurrentPeriodDistributions.ElementAt(0).Id, topDistribution.DisplayId);

                Assert.Equal(element.PreviousPeriodDistributions.ElementAt(0).Id, topDistribution.DisplayId);
            }
        }

        internal static IEnumerable<DistributionMetaModel> FakeTopDistributions()
        {
            return new List<DistributionMetaModel>
            {
                new DistributionMetaModel { DisplayId = "1", DisplayName = "distribution1", CurrentPeriodDistributions = new List<IdentityMetaModel> { new IdentityMetaModel { Id = "1", Name = "distribution1" } } },
                new DistributionMetaModel { DisplayId = "2", DisplayName = "distribution2", CurrentPeriodDistributions =  new List<IdentityMetaModel> { new IdentityMetaModel { Id = "2", Name = "distribution2" } } },
                new DistributionMetaModel { DisplayId = "3", DisplayName = "distribution3", CurrentPeriodDistributions =  new List<IdentityMetaModel> { new IdentityMetaModel { Id = "3", Name = "distribution3" } }},
                new DistributionMetaModel { DisplayId = "4", DisplayName = "distribution4", CurrentPeriodDistributions =  new List<IdentityMetaModel> { new IdentityMetaModel { Id = "4", Name = "distribution4" } } },
                new DistributionMetaModel { DisplayId = "5", DisplayName = "distribution5", CurrentPeriodDistributions =  new List<IdentityMetaModel> { new IdentityMetaModel { Id = "5", Name = "distribution5" } } },
                new DistributionMetaModel { DisplayId = "0", DisplayName = "Others", CurrentPeriodDistributions = new List<IdentityMetaModel> { new IdentityMetaModel { Id = "0", Name = "Others" } } }
            };
        }

        internal static IEnumerable<MeasurementPerioddDtoTestClass> FakePreviouMeasurementPeriodDTOs()
        {
            return new List<MeasurementPerioddDtoTestClass>
            {
                new MeasurementPerioddDtoTestClass
                {
                      Total = 1500,
                      TestId = "1"
                },
                new MeasurementPerioddDtoTestClass
                {
                    Total = 14000.9423,
                    TestId = "2"
                },
                new MeasurementPerioddDtoTestClass
                {
                    Total = -1235.100,
                    TestId = "3"
                },
                new MeasurementPerioddDtoTestClass
                {
                    Total = 500,
                    TestId = "7"
                },
                new MeasurementPerioddDtoTestClass
                {
                    Total = 500,
                    TestId = "10"
                },
                new MeasurementPerioddDtoTestClass
                {
                    Total = 12,
                    TestId = "0"
                }
            };
        }

        internal static IEnumerable<MeasurementPerioddDtoTestClass> FakeCurrentMeasurementPeriodDTOs()
        {
            return new List<MeasurementPerioddDtoTestClass>
            {
                new MeasurementPerioddDtoTestClass
                {
                      Total = 9123.12,
                      TestId = "1"
                },
                new MeasurementPerioddDtoTestClass
                {
                    Total = 14000,
                    TestId = "2"
                },
                new MeasurementPerioddDtoTestClass
                {
                    Total = 0.2,
                    TestId = "3"
                },
                new MeasurementPerioddDtoTestClass
                {
                    Total = 99999.99,
                    TestId = "4"
                },
                new MeasurementPerioddDtoTestClass
                {
                    Total = 750,
                    TestId = "5"
                },
                new MeasurementPerioddDtoTestClass
                {
                    Total = -40,
                    TestId = "6"
                },
                new MeasurementPerioddDtoTestClass
                {
                    Total = 1900,
                    TestId = "7"
                },
                 new MeasurementPerioddDtoTestClass
                {
                    Total = 875.528,
                    TestId = "8"
                },
                new MeasurementPerioddDtoTestClass
                {
                    Total = 20,
                    TestId = "0"
                }
            };
        }
    }
}