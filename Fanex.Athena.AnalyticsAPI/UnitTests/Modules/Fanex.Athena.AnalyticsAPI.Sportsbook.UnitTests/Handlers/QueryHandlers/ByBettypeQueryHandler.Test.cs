using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Sportsbook.Handlers;
using Fanex.Athena.AnalyticsAPI.Sportsbook.Mappers;
using Fanex.Athena.AnalyticsAPI.Sportsbook.Models;
using Fanex.Athena.AnalyticsAPI.Sportsbook.Repositories;
using Fanex.Data.Repository;
using Moq;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Sportsbook.UnitTests.Handlers.QueryHandlers
{
    public class ByBettypeQueryHandler_Test
    {
        private readonly Fixture _fixture;
        private readonly ByBettypeQueryHandler _handler;
        private readonly Mock<ISportsbookRepository> _sportsbookRepository;
        private readonly Mock<ISportsbookMapper> _sportsbookMapper;

        public ByBettypeQueryHandler_Test()
        {
            _fixture = new Fixture();
            _sportsbookRepository = new Mock<ISportsbookRepository>();
            _sportsbookMapper = new Mock<ISportsbookMapper>();
            _handler = new ByBettypeQueryHandler(_sportsbookRepository.Object, _sportsbookMapper.Object, Mock.Of<ITranslationService>());
        }

        [Fact]
        public async Task HandleByBettypeSoccerRequest_WhenCall_MapperShouldCallCorrectParams()
        {
            //Arrange
            var metaModel = _fixture.Create<ByBettypeSoccerRequest>();
            var expected = _fixture.CreateMany<SportsbookDTO>().Where(_ => _.IsSaba == metaModel.IsSaba);
            _sportsbookRepository.Setup(_ => _.QueryAsync<SportsbookDTO>(It.IsAny<ICriteria>())).ReturnsAsync(expected);

            //Act
            await _handler.Handle(metaModel, CancellationToken.None);

            //Assert
            _sportsbookMapper.Verify(x => x.ToTurnoverResponse(expected, It.IsAny<bool>(), It.IsAny<IReadOnlyDictionary<int, IEnumerable<bool>>>(), It.IsAny<IEnumerable<int>>()), Times.Once());
            _sportsbookMapper.Verify(x => x.ToWinlossResponse(expected, It.IsAny<bool>(), It.IsAny<IReadOnlyDictionary<int, IEnumerable<bool>>>(), It.IsAny<IEnumerable<int>>()), Times.Once());
        }

        [Fact]
        public async Task HandleByBettypeBasketballRequest_WhenCall_MapperShouldCallCorrectParams()
        {
            //Arrange
            var metaModel = _fixture.Create<ByBettypeBasketballRequest>();
            var expected = _fixture.CreateMany<SportsbookDTO>().Where(_ => _.IsSaba == metaModel.IsSaba);
            _sportsbookRepository.Setup(_ => _.QueryAsync<SportsbookDTO>(It.IsAny<ICriteria>())).ReturnsAsync(expected);

            //Act
            await _handler.Handle(metaModel, CancellationToken.None);

            //Assert
            _sportsbookMapper.Verify(x => x.ToTurnoverResponse(expected, It.IsAny<bool>(), It.IsAny<IReadOnlyDictionary<int, IEnumerable<bool>>>(), It.IsAny<IEnumerable<int>>()), Times.Once());
            _sportsbookMapper.Verify(x => x.ToWinlossResponse(expected, It.IsAny<bool>(), It.IsAny<IReadOnlyDictionary<int, IEnumerable<bool>>>(), It.IsAny<IEnumerable<int>>()), Times.Once());
        }
    }
}