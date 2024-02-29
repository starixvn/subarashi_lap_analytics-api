using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Moq;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Common.UnitTests.Repositories
{
    public class BaseRepository_MultipleQueryAsync_Test
    {
        private readonly Fixture _fixture;

        private readonly Mock<ISqlService> _mockSqlService;

        private BaseRepositoryTestable _baseRepository;

        public BaseRepository_MultipleQueryAsync_Test()
        {
            _fixture = new Fixture();

            _mockSqlService = new Mock<ISqlService>();
        }

        [Fact]
        public async Task QueryMultipleAsync_QueryContentParam_WhenCall_SqlServiceCallSameParamsAndReturnCorrectResult()
        {
            //Arrange
            var listCriteria = _fixture.CreateMany<CriteriaTest>().ToArray();
            var expected = _fixture.CreateMany<IEnumerable<object>>();

            _mockSqlService.Setup(_ => _.ExecuteMultipleQueryData<object>(listCriteria))
                .Returns(() => Task.FromResult(expected));

            _baseRepository = new BaseRepositoryTestable(_mockSqlService.Object);

            //Act
            var actual = await _baseRepository.QueryMultipleAsync<object>(listCriteria);

            //Assert
            Assert.Equal(expected, actual);
        }
    }
}