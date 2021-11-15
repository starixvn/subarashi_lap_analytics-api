using Fanex.Athena.AnalyticsAPI.Common.Models.Configuration;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Summary.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Summary.Models.ViewModels;
using Fanex.Athena.AnalyticsAPI.Summary.Services;
using Fanex.Athena.AnalyticsAPI.UnitTests.Utils.Helpers;
using FluentAssertions;
using Moq;
using System;
using System.Data;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Summary.UnitTest.Services
{
    public class LicenseeDataPlatformService_Test
    {
        private readonly Mock<ISqlService> _mockSqlService;
        private readonly Mock<ICommonService> _mockCommonService;
        private readonly LicenseeDataPlatformService _target;

        public LicenseeDataPlatformService_Test()
        {
            _mockSqlService = new Mock<ISqlService>();
            _mockCommonService = new Mock<ICommonService>();
            _target = new LicenseeDataPlatformService(_mockSqlService.Object, _mockCommonService.Object);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task GetWinLoss_AgentListXmlIsNullOrEmpty_ReturnDefaultValue(string agentListXml)
        {
            //Arrange
            var metaModel = new BaseSummaryMetaModel
            {
                AgentListXml = agentListXml
            };

            //Act
            var actual = await _target.GetWinLoss(metaModel);

            //Assert
            Assert.Equal(0, actual.Total);
            Assert.Empty(actual.Data);
        }

        [Fact]
        public async Task GetWinLoss_WhenCall_CorrectParams()
        {
            //Arrange
            var metaModel = FakeHelper.Create<BaseSummaryMetaModel>();

            var expectedSpKey = "Lrf_GetLicenseeWLByPlatform";
            var expectedSpParams = ExpectedSPParams(metaModel);
            var expectedReadHandlerTarget = new { winlossColumnName = "LicenseeWL" };

            var actualKey = string.Empty;
            var actualSpParams = null as object;
            var actualReadHandlerTarget = null as object;

            SetupCallBackForSqlService((spKey, spParams, readHandler) =>
            {
                actualKey = spKey;
                actualSpParams = spParams;
                actualReadHandlerTarget = readHandler.Target;
            });

            BalanceUp expectedBalanceUp = new BalanceUp()
            {
                MoveBetsDay = new DateTime(2021, 06, 22),
                BalanceUpDay = new DateTime(2021, 06, 22)
            };

            _mockCommonService.Setup(_ => _.GetBalanceUp()).Returns(Task.FromResult(expectedBalanceUp));

            //Act
            await _target.GetWinLoss(metaModel);

            //Assert
            Assert.Equal(expectedSpKey, actualKey);
            expectedSpParams.Should().BeEquivalentTo(actualSpParams);
            expectedReadHandlerTarget.Should().BeEquivalentTo(actualReadHandlerTarget);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task GetTurnover_AgentListXmlIsNullOrEmpty_ReturnDefaultValue(string agentListXml)
        {
            //Arrange
            var metaModel = new BaseSummaryMetaModel
            {
                AgentListXml = agentListXml
            };

            //Act
            var actual = await _target.GetTurnover(metaModel);

            //Assert
            Assert.Equal(0, actual.Total);
            Assert.Empty(actual.Data);
        }

        [Fact]
        public async Task GetTurnover_WhenCall_CorrectParams()
        {
            //Arrange
            var metaModel = FakeHelper.Create<BaseSummaryMetaModel>();

            var expectedSpKey = "Lrf_GetLicenseeTurnoverByPlatform";
            var expectedSpParams = ExpectedSPParams(metaModel);
            var expectedReadHandlerTarget = new { winlossColumnName = "Turnover" };

            var actualKey = string.Empty;
            var actualSpParams = null as object;
            var actualReadHandlerTarget = null as object;

            SetupCallBackForSqlService((spKey, spParams, readHandler) =>
            {
                actualKey = spKey;
                actualSpParams = spParams;
                actualReadHandlerTarget = readHandler.Target;
            });

            BalanceUp expectedBalanceUp = new BalanceUp()
            {
                MoveBetsDay = new DateTime(2021, 06, 22),
                BalanceUpDay = new DateTime(2021, 06, 22)
            };

            _mockCommonService.Setup(_ => _.GetBalanceUp()).Returns(Task.FromResult(expectedBalanceUp));

            //Act
            await _target.GetTurnover(metaModel);

            //Assert
            Assert.Equal(expectedSpKey, actualKey);
            expectedSpParams.Should().BeEquivalentTo(actualSpParams);
            expectedReadHandlerTarget.Should().BeEquivalentTo(actualReadHandlerTarget);
        }

        private object ExpectedSPParams(BaseSummaryMetaModel metaModel)
        {
            var expectedSpParams = new
            {
                AgentList = metaModel.AgentListXml,
                metaModel.FromDate,
                metaModel.ToDate,
                metaModel.BaseCurrency,
                MoveBetsDay = new DateTime(2021, 06, 22),
                metaModel.ProductList
            };

            return expectedSpParams;
        }

        private void SetupCallBackForSqlService(Action<string, object, Func<IDataReader, WidgetByPlatformViewModel>> callBack)
        {
            _mockSqlService.Setup(_ => _.ExecuteQueryWithReadHandler(It.IsAny<string>(), It.IsAny<object>(),
                It.IsAny<Func<IDataReader, WidgetByPlatformViewModel>>()))
            .Callback<string, object, Func<IDataReader, WidgetByPlatformViewModel>>((spKey, spParams, readHandler) => callBack(spKey, spParams, readHandler));
        }
    }
}