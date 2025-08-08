namespace Fanex.Athena.AnalyticsAPI.Common.UnitTests.Services
{
    using System.Globalization;
    using Common.Constants;
    using Common.Services;
    using Moq;
    using RemotingRef;
    using Xunit;

    public class LangResourceServiceTests
    {
        private readonly Mock<IRemotingRefService> _mockRemotingRefService;
        private readonly Mock<ICommonApiClient> _mockApiClient;
        private readonly LangResourceService _langResourceService;

        public LangResourceServiceTests()
        {
            _mockRemotingRefService = new Mock<IRemotingRefService>();
            _mockApiClient = new Mock<ICommonApiClient>();
            _langResourceService = new LangResourceService(_mockRemotingRefService.Object, _mockApiClient.Object);
        }

        [Theory]
        [InlineData("99", "Bet Type Name from Remoting")] // Not in EXCEPTION_BETTYPE_IDs
        public void GetName_ForBetType_ShouldCallRemotingService(string id, string expectedName)
        {
            // Arrange
            var langCode = CultureInfo.CurrentUICulture.Name;
            _mockRemotingRefService.Setup(x => x.GetDisplayedBetTypeName(id, langCode)).Returns(expectedName);

            // Act
            var result = _langResourceService.GetName(TranslationConst.BettypeType, id, "default");

            // Assert
            Assert.Equal(expectedName, result);
            _mockRemotingRefService.Verify(x => x.GetDisplayedBetTypeName(id, langCode), Times.Once);
        }

        [Fact]
        public void GetName_ForBetType_WhenRemotingReturnsEmpty_ShouldReturnDefault()
        {
            // Arrange
            var id = "99";
            var defaultValue = "Default Bet Type";
            var langCode = CultureInfo.CurrentUICulture.Name;
            _mockRemotingRefService.Setup(x => x.GetDisplayedBetTypeName(id, langCode)).Returns(string.Empty);

            // Act
            var result = _langResourceService.GetName(TranslationConst.BettypeType, id, defaultValue);

            // Assert
            Assert.Equal(defaultValue, result);
        }

        [Fact]
        public void GetName_ForBetType_WithExceptionId_ShouldUseLocalResource()
        {
            // Arrange
            var id = "0"; // In EXCEPTION_BETTYPE_IDs
            // We expect it to call the static ViewResource.ResourceManager
            // Since we can't mock it, we know the resource 'bettype_0' exists and is 'Others'
            var expectedValue = "Others";

            // Act
            var result = _langResourceService.GetName(TranslationConst.BettypeType, id, "default");

            // Assert
            Assert.Equal(expectedValue, result);
            _mockRemotingRefService.Verify(x => x.GetDisplayedBetTypeName(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Theory]
        [InlineData("1", "Sport Name from Remoting")] // Not in EXCEPTION_SPORT_IDs or HARD_CODE_SPORT_IDS
        public void GetName_ForSport_ShouldCallRemotingService(string id, string expectedName)
        {
            // Arrange
            var langCode = CultureInfo.CurrentUICulture.Name;
            _mockRemotingRefService.Setup(x => x.GetSportName(id, langCode)).Returns(expectedName);

            // Act
            var result = _langResourceService.GetName(TranslationConst.SportType, id, "default");

            // Assert
            Assert.Equal(expectedName, result);
            _mockRemotingRefService.Verify(x => x.GetSportName(id, langCode), Times.Once);
        }

        [Fact]
        public void GetName_ForSport_WhenRemotingReturnsEmpty_ShouldReturnDefault()
        {
            // Arrange
            var id = "1";
            var defaultValue = "Default Sport";
            var langCode = CultureInfo.CurrentUICulture.Name;
            _mockRemotingRefService.Setup(x => x.GetSportName(id, langCode)).Returns(string.Empty);

            // Act
            var result = _langResourceService.GetName(TranslationConst.SportType, id, defaultValue);

            // Assert
            Assert.Equal(defaultValue, result);
        }
        
        [Fact]
        public void GetName_ForSport_WithHardCodedId_ShouldUseLocalResource()
        {
            // Arrange
            var id = "-2"; // In HARD_CODE_SPORT_IDS
            var expectedValue = "Parlay"; // From ViewResource 'sport__2'

            // Act
            var result = _langResourceService.GetName(TranslationConst.SportType, id, "default");

            // Assert
            Assert.Equal(expectedValue, result);
            _mockRemotingRefService.Verify(x => x.GetSportName(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void GetName_ForSport_WithExceptionId_ShouldUseLocalResource()
        {
            // Arrange
            var id = "0"; // In EXCEPTION_SPORT_IDs
            var expectedValue = "Others"; // From ViewResource 'sport_0'

            // Act
            var result = _langResourceService.GetName(TranslationConst.SportType, id, "default");

            // Assert
            Assert.Equal(expectedValue, result);
            _mockRemotingRefService.Verify(x => x.GetSportName(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void GetName_ForOtherTypes_ShouldUseLocalResource()
        {
            // Arrange
            var id = "3"; // For league
            var expectedValue = "ENGLISH PREMIER LEAGUE"; // From ViewResource 'league_3'

            // Act
            var result = _langResourceService.GetName(TranslationConst.LeagueType, id, "default");

            // Assert
            Assert.Equal(expectedValue, result);
            _mockRemotingRefService.Verify(x => x.GetDisplayedBetTypeName(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _mockRemotingRefService.Verify(x => x.GetSportName(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
        
        [Fact]
        public void GetName_ForOtherTypes_WhenResourceMissing_ShouldReturnDefault()
        {
            // Arrange
            var id = "non_existent_id";
            var defaultValue = "Default Value";
            
            // Act
            var result = _langResourceService.GetName(TranslationConst.LeagueType, id, defaultValue);

            // Assert
            Assert.Equal(defaultValue, result);
        }
    }
} 