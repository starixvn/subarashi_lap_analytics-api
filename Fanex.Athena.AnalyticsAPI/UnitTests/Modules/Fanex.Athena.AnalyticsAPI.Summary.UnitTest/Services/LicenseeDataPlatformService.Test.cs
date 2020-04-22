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
        private readonly Mock<ISqlService> _sqlServiceMock;
        private readonly ILicenseeDataPlatformService _target;

        public LicenseeDataPlatformService_Test()
        {
            _sqlServiceMock = new Mock<ISqlService>();
            _target = new LicenseeDataPlatformService(_sqlServiceMock.Object);
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

            //Action
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
            var actualKey = string.Empty;

            var expectedSpParams = new
            {
                AgentList = metaModel.AgentListXml,
                metaModel.FromDate,
                metaModel.ToDate,
                metaModel.BaseCurrency
            };

            var actualSpParams = null as object;

            _sqlServiceMock.Setup(_ => _.ExecuteQueryWithReadHandler(It.IsAny<string>(), It.IsAny<object>(),
                It.IsAny<Func<IDataReader, WidgetByPlatformViewModel>>()))
            .Callback<string, object, Func<IDataReader, WidgetByPlatformViewModel>>((spKey, spParams, readHandler) =>
            {
                actualKey = spKey;
                actualSpParams = spParams;
            });

            //Action
            await _target.GetWinLoss(metaModel);

            //Assert
            Assert.Equal(expectedSpKey, actualKey);
            expectedSpParams.Should().Equals(actualSpParams);
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

            //Action
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
            var actualKey = string.Empty;

            var expectedSpParams = new
            {
                AgentList = metaModel.AgentListXml,
                metaModel.FromDate,
                metaModel.ToDate,
                metaModel.BaseCurrency
            };

            var actualSpParams = null as object;

            _sqlServiceMock.Setup(_ => _.ExecuteQueryWithReadHandler(It.IsAny<string>(), It.IsAny<object>(),
                It.IsAny<Func<IDataReader, WidgetByPlatformViewModel>>()))
            .Callback<string, object, Func<IDataReader, WidgetByPlatformViewModel>>((spKey, spParams, readHandler) =>
            {
                actualKey = spKey;
                actualSpParams = spParams;
            });

            //Action
            await _target.GetTurnover(metaModel);

            //Assert
            Assert.Equal(expectedSpKey, actualKey);
            expectedSpParams.Should().Equals(actualSpParams);
        }
    }
}