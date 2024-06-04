using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Core.Services;
using Fanex.Athena.AnalyticsAPI.Sportsbook.Handlers;
using Fanex.Athena.AnalyticsAPI.Sportsbook.Mappers;
using Fanex.Athena.AnalyticsAPI.Sportsbook.Models;
using Fanex.Athena.AnalyticsAPI.Sportsbook.Repositories;
using Fanex.Data.Repository;
using Moq;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Sportsbook.UnitTests.Handlers.QueryHandlers
{
    public class ByLeagueQueryHandler_Test
    {
        private readonly Fixture _fixture;
        private readonly ByLeagueQueryHandler _handler;
        private readonly Mock<ISportsbookRepository> _sportsbookRepository;
        private readonly Mock<ISportsbookMapper> _sportsbookMapper;

        public ByLeagueQueryHandler_Test()
        {
            _fixture = new Fixture();
            _sportsbookRepository = new Mock<ISportsbookRepository>();
            _sportsbookMapper = new Mock<ISportsbookMapper>();
            _handler = new ByLeagueQueryHandler(_sportsbookRepository.Object, _sportsbookMapper.Object, Mock.Of<ITranslationService>(), Mock.Of<IAuthInfoService>());
        }

        [Fact]
        public async Task HandleByLeagueSoccerRequest_WhenCall_MapperShouldCallCorrectParams()
        {
            //Arrange
            var metaModel = _fixture.Create<ByLeagueSoccerRequest>();
            var expected = _fixture.CreateMany<SportsbookDTO>();
            _sportsbookRepository.Setup(_ => _.QueryAsync<SportsbookDTO>(It.IsAny<ICriteria>())).ReturnsAsync(expected);

            //Act
            await _handler.Handle(metaModel, CancellationToken.None);

            //Assert
            _sportsbookMapper.Verify(x => x.ToTurnoverResponse(expected, It.IsAny<bool>(), It.IsAny<IReadOnlyDictionary<int, IEnumerable<bool>>>(), It.IsAny<IEnumerable<int>>()), Times.Once());
            _sportsbookMapper.Verify(x => x.ToWinlossResponse(expected, It.IsAny<bool>(), It.IsAny<IReadOnlyDictionary<int, IEnumerable<bool>>>(), It.IsAny<IEnumerable<int>>()), Times.Once());
        }

        [Fact]
        public async Task HandleByLeagueBasketballRequest_WhenCall_MapperShouldCallCorrectParams()
        {
            //Arrange
            var metaModel = _fixture.Create<ByLeagueBasketballRequest>();
            var expected = _fixture.CreateMany<SportsbookDTO>();
            _sportsbookRepository.Setup(_ => _.QueryAsync<SportsbookDTO>(It.IsAny<ICriteria>())).ReturnsAsync(expected);

            //Act
            await _handler.Handle(metaModel, CancellationToken.None);

            //Assert
            _sportsbookMapper.Verify(x => x.ToTurnoverResponse(expected, It.IsAny<bool>(), It.IsAny<IReadOnlyDictionary<int, IEnumerable<bool>>>(), It.IsAny<IEnumerable<int>>()), Times.Once());
            _sportsbookMapper.Verify(x => x.ToWinlossResponse(expected, It.IsAny<bool>(), It.IsAny<IReadOnlyDictionary<int, IEnumerable<bool>>>(), It.IsAny<IEnumerable<int>>()), Times.Once());
        }

        [Fact]
        public async Task HandleByLeagueCricketRequest_WhenCall_MapperShouldCallCorrectParams()
        {
            //Arrange
            var metaModel = _fixture.Create<ByLeagueCricketRequest>();
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