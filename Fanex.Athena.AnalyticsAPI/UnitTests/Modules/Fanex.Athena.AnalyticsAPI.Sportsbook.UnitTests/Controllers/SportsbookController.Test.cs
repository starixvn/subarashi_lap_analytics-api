using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Sportsbook.Controllers;
using Fanex.Athena.AnalyticsAPI.Sportsbook.Models;
using MediatR;
using Moq;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Sportsbook.UnitTests.Controllers
{
    public class SportsbookController_Test
    {
        private readonly SportsbookController _target;
        private readonly Mock<ISender> _mockSender;
        private readonly Fixture _fixture;

        public SportsbookController_Test()
        {
            _mockSender = new Mock<ISender>();
            _fixture = new Fixture();
            _target = new SportsbookController(Mock.Of<IContextService>(), _mockSender.Object);
        }

        [Fact]
        public async Task BySport_WithMetaModel_SenderShouldCallCorrectParams()
        {
            //Arrange
            var metaModel = _fixture.Create<BySportRequest>();

            //Act
            await _target.BySport(metaModel);

            //Assert
            _mockSender.Verify(x => x.Send(metaModel, It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task ByPlatform_WithMetaModel_SenderShouldCallCorrectParams()
        {
            //Arrange
            var metaModel = _fixture.Create<ByPlatformRequest>();

            //Act
            await _target.ByPlatform(metaModel);

            //Assert
            _mockSender.Verify(x => x.Send(metaModel, It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task ByLeagueSoccer_WithMetaModel_SenderShouldCallCorrectParams()
        {
            //Arrange
            var metaModel = _fixture.Create<ByLeagueSoccerRequest>();

            //Act
            await _target.ByLeagueSoccer(metaModel);

            //Assert
            _mockSender.Verify(x => x.Send(metaModel, It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task ByLeagueBasketball_WithMetaModel_SenderShouldCallCorrectParams()
        {
            //Arrange
            var metaModel = _fixture.Create<ByLeagueBasketballRequest>();

            //Act
            await _target.ByLeagueBasketball(metaModel);

            //Assert
            _mockSender.Verify(x => x.Send(metaModel, It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task ByLeagueCricket_WithMetaModel_SenderShouldCallCorrectParams()
        {
            //Arrange
            var metaModel = _fixture.Create<ByLeagueCricketRequest>();

            //Act
            await _target.ByLeagueCricket(metaModel);

            //Assert
            _mockSender.Verify(x => x.Send(metaModel, It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task ByCurrency_WithMetaModel_SenderShouldCallCorrectParams()
        {
            //Arrange
            var metaModel = _fixture.Create<ByCurrencyRequest>();

            //Act
            await _target.ByCurrency(metaModel);

            //Assert
            _mockSender.Verify(x => x.Send(metaModel, It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task ByBettypeSoccer_WithMetaModel_SenderShouldCallCorrectParams()
        {
            //Arrange
            var metaModel = _fixture.Create<ByBettypeSoccerRequest>();

            //Act
            await _target.ByBettypeSoccer(metaModel);

            //Assert
            _mockSender.Verify(x => x.Send(metaModel, It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task ByBettypeBaseketball_WithMetaModel_SenderShouldCallCorrectParams()
        {
            //Arrange
            var metaModel = _fixture.Create<ByBettypeBasketballRequest>();

            //Act
            await _target.ByBettypeBaseketball(metaModel);

            //Assert
            _mockSender.Verify(x => x.Send(metaModel, It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}