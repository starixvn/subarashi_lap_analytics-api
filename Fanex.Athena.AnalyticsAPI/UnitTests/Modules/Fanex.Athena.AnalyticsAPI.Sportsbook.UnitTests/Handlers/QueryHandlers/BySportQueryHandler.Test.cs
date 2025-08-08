namespace Fanex.Athena.AnalyticsAPI.Sportsbook.UnitTests.Handlers.QueryHandlers
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoFixture;
    using Common.Services;
    using Core.Services;
    using Data.Repository;
    using Moq;
    using Repositories;
    using Sportsbook.Handlers;
    using Sportsbook.Mappers;
    using Sportsbook.Models;
    using Xunit;

    public class BySportQueryHandler_Test
    {
        private readonly Fixture _fixture;
        private readonly BySportQueryHandler _handler;
        private readonly Mock<ISportsbookRepository> _sportsbookRepository;
        private readonly Mock<ISportsbookMapper> _sportsbookMapper;

        public BySportQueryHandler_Test()
        {
            _fixture = new Fixture();
            _sportsbookRepository = new Mock<ISportsbookRepository>();
            _sportsbookMapper = new Mock<ISportsbookMapper>();
            _handler = new BySportQueryHandler(_sportsbookRepository.Object, _sportsbookMapper.Object, Mock.Of<ITranslationService>(), Mock.Of<IAuthInfoService>());
        }

        [Fact]
        public async Task HandleBySportRequest_WhenCall_MapperShouldCallCorrectParams()
        {
            //Arrange
            var metaModel = _fixture.Create<BySportRequest>();
            var expected = _fixture.CreateMany<SportsbookDTO>();
            _sportsbookRepository.Setup(_ => _.QueryAsync<SportsbookDTO>(It.IsAny<ICriteria>())).ReturnsAsync(expected);

            //Act
            await _handler.Handle(metaModel, CancellationToken.None);

            //Assert
            _sportsbookMapper.Verify(x => x.ToTurnoverResponse(expected, It.IsAny<bool>(), It.IsAny<IReadOnlyDictionary<int, IEnumerable<bool>>>(), It.IsAny<IEnumerable<int>>()), Times.Once());
            _sportsbookMapper.Verify(x => x.ToWinlossResponse(expected, It.IsAny<bool>(), It.IsAny<IReadOnlyDictionary<int, IEnumerable<bool>>>(), It.IsAny<IEnumerable<int>>()), Times.Once());
        }
    }
}