using AutoFixture;
using Fanex.Athena.AnalyticsAPI.Common.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Performance.Repositories;
using Fanex.Athena.AnalyticsAPI.Performance.Services;
using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Performance.UnitTest.Services
{
    public class PerformanceService_Test
    {
        private readonly PerformanceServiceTestable _target;
        private readonly Mock<IContextService> _mockContext;
        private readonly Mock<IPerformanceRepository> _repoMock;
        private readonly Mock<IHandleRawData> _handleRawDataMock;
        private readonly Fixture _fixture;

        public PerformanceService_Test()
        {
            _mockContext = new Mock<IContextService>();
            _repoMock = new Mock<IPerformanceRepository>();
            _fixture = new Fixture();
            _handleRawDataMock = new Mock<IHandleRawData>();
            _target = new PerformanceServiceTestable(_mockContext.Object, _repoMock.Object, _handleRawDataMock.Object);
        }

        [Theory]
        [InlineData("key1", "01/2019", "12/2019", "key1_01/01/2019_12/31/2019")]
        [InlineData("key2", "10/08/2018", "07/05/2019", "key2_10/01/2018_07/31/2019")]
        public void GetFieldNameOfMonthGrowthObject_WhenCall_ReturnCorrectResult(string key, string fromMonth, string toMonth, string expected)
        {
            //Arrange
            var period = new MonthRangeMetaModel
            {
                FromMonth = fromMonth,
                ToMonth = toMonth
            };

            //Act
            var result = _target.GetFieldNameOfMonthGrowthObjectTestable(key, period);

            //Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void MapToTopDistributions_InputIsNull_ReturnEmpty()
        {
            //Arrange

            //Act
            var result = _target.MapToTopDistributionsTestable(null);

            //Assert
            Assert.Empty(result);
        }

        [Theory]
        [MemberData(nameof(FakeTopDistributions))]
        public void MapToTopDistributions_WhenCall_ReturnCorrectResult(IEnumerable<DistributionMetaModel> topDistributions, IEnumerable<(string topDistributionId, string topDistributionName)> expected)
        {
            //Arrange
            //Act
            var result = _target.MapToTopDistributionsTestable(topDistributions);

            //Assert
            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void GetTopDistributions_InputIsEmpty_ReturnEmpty()
        {
            //Arrange
            //Act
            var result = _target.GetTopDistributionsTestable(Enumerable.Empty<DistributionDtoTestClass>(), _fixture.Create<int>());

            //Assert
            Assert.Empty(result);
        }

        [Theory]
        [MemberData(nameof(FakeDistributionDtos))]
        public void GetTopDistributions_WhenCall_ReturnCorrectResult(IEnumerable<DistributionDtoTestClass> distributions, int numberOfTop, IEnumerable<(string topDistributionId, string topDistributionName)> expected)
        {
            //Arrange
            //Act
            var result = _target.GetTopDistributionsTestable(distributions, numberOfTop);

            //Assert
            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(result));
        }

        public static IEnumerable<object[]> FakeDistributionDtos()
        {
            yield return new object[]
            {
                new List<DistributionDtoTestClass>
                {
                     new DistributionDtoTestClass { IsSum = true },
                     new DistributionDtoTestClass {  TestId = "1", TestName = "name1", Distribution = -0.65 },
                     new DistributionDtoTestClass {  TestId = "2", TestName = "name2", Distribution = -0.1 },
                     new DistributionDtoTestClass {  TestId = "3", TestName = "name3", Distribution =  0.3 },
                },
                2,
                new List<(string topDistributionId, string topDistributionName)>
                {
                    ("1", "name1"),
                    ("3", "name3"),
                    ("0", "Others")
                }
            };

            yield return new object[]
            {
                new List<DistributionDtoTestClass>
                {
                     new DistributionDtoTestClass { IsSum = true },
                     new DistributionDtoTestClass {  TestId = "1", TestName = "name1", Distribution = -0.1 },
                     new DistributionDtoTestClass {  TestId = "2", TestName = "name2", Distribution = -0.5 },
                     new DistributionDtoTestClass {  TestId = "3", TestName = "name3", Distribution =  0.3 },
                     new DistributionDtoTestClass {  TestId = "4", TestName = "name4", Distribution =  0.1 },
                     new DistributionDtoTestClass {  TestId = "5", TestName = "name5", Distribution =  0.1 },
                },
                5,
                new List<(string topDistributionId, string topDistributionName)>
                {
                    ("2", "name2"),
                    ("3", "name3"),
                    ("1", "name1"),
                    ("4", "name4"),
                    ("5", "name5"),
                }
            };
        }

        public static IEnumerable<object[]> FakeTopDistributions()
        {
            yield return new object[]
            {
                new List<DistributionMetaModel>
                {
                     new DistributionMetaModel
                    {
                        DisplayId = "1",
                        DisplayName = "name1"
                    },
                    new DistributionMetaModel
                    {
                        DisplayId = "2",
                        DisplayName = "name2"
                    },
                    new DistributionMetaModel
                    {
                        DisplayId = "3",
                        DisplayName = "name3"
                    }
                },

                new List<(string topDistributionId, string topDistributionName)>
                {
                    ("1", "name1"),
                    ("2", "name2"),
                    ("3", "name3"),
                }
            };

            yield return new object[]
            {
                new List<DistributionMetaModel>
                {
                    new DistributionMetaModel
                    {
                        DisplayId = "1",
                        DisplayName = "name1"
                    },
                    new DistributionMetaModel
                    {
                        DisplayId = "2",
                        DisplayName = "name2"
                    },
                    new DistributionMetaModel
                    {
                        DisplayId = "3",
                        DisplayName = "name3"
                    },
                    new DistributionMetaModel
                    {
                        DisplayId = "4",
                        DisplayName = "name4"
                    },
                    new DistributionMetaModel
                    {
                        DisplayId = "5",
                        DisplayName = "name5"
                    }
                },
                new List<(string topDistributionId, string topDistributionName)>
                {
                    ("1", "name1"),
                    ("2", "name2"),
                    ("3", "name3"),
                    ("4", "name4"),
                    ("5", "name5"),
                }
            };
        }
    }
}