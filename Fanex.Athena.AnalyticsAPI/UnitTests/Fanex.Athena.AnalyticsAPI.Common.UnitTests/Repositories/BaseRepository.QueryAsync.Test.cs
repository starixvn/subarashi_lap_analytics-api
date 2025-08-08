namespace Fanex.Athena.AnalyticsAPI.Common.UnitTests.Repositories
{
    using System.Threading.Tasks;
    using AutoFixture;
    using Common.Services;
    using Moq;
    using Xunit;

    public class BaseRepository_QueryAsync_Test
    {
        private readonly Fixture _fixture;

        private readonly Mock<ISqlService> _mockSqlService;

        private BaseRepositoryTestable _baseRepository;

        public BaseRepository_QueryAsync_Test()
        {
            _fixture = new Fixture();

            _mockSqlService = new Mock<ISqlService>();
        }

        [Fact]
        public async Task QueryAsync_QueryContentParam_WhenCall_SqlServiceCallSameParams()
        {
            //Arrange
            var criteria = _fixture.Create<CriteriaTest>();
            var expected = _fixture.CreateMany<object>();

            _mockSqlService.Setup(_ => _.ExecuteQueryData<object>(criteria))
                .Returns(() => Task.FromResult(expected));

            _baseRepository = new BaseRepositoryTestable(_mockSqlService.Object);

            //Act
            var actual = await _baseRepository.QueryAsync<object>(criteria);

            //Assert
            Assert.Equal(expected, actual);
        }
    }
}