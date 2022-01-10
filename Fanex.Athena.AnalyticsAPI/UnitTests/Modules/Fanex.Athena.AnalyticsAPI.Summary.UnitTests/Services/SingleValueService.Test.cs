using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Summary.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Summary.Services;
using Fanex.Athena.AnalyticsAPI.UnitTests.Utils.Helpers;
using FluentAssertions;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Summary.UnitTests.Services
{
    public class SingleValueService_Test
    {
        private readonly Mock<ISqlService> _mockSqlService;
        private readonly SingleValueService _target;

        public SingleValueService_Test()
        {
            _mockSqlService = new Mock<ISqlService>();
            _target = new SingleValueService(_mockSqlService.Object);
        }

        [Fact]
        public async Task GetCustomerBalance_WhenCall_CorrectParams()
        {
            //Arrange
            var metaModel = FakeHelper.Create<BaseSummaryMetaModel>();

            var expectedSpKey = "Lrf_GetCustomerBalance";
            var expectedSpParams = ExpectedSPParams(metaModel);
            var expectedReadHandlerTarget = new { columnName = "ClosingBalance" };

            var actualReadHandlerTarget = null as object;
            var actualKey = string.Empty;
            var actualSpParams = null as object;

            SetupCallBackForSqlService((spKey, spParams, readHandler) =>
            {
                actualKey = spKey;
                actualSpParams = spParams;
                actualReadHandlerTarget = readHandler.Target;
            });

            //Act
            await _target.GetCustomerBalance(metaModel);

            //Assert
            Assert.Equal(expectedSpKey, actualKey);
            expectedSpParams.Should().BeEquivalentTo(actualSpParams as Hashtable);
            expectedReadHandlerTarget.Should().BeEquivalentTo(actualReadHandlerTarget);
        }

        [Fact]
        public async Task GetPendingFunds_WhenCall_CorrectParams()
        {
            //Arrange
            var metaModel = FakeHelper.Create<BaseSummaryMetaModel>();

            var expectedSpKey = "Lrf_GetLicenseePendingFunds";
            var expectedSpParams = ExpectedSPParams(metaModel, false);
            var expectedReadHandlerTarget = new { columnName = "PendingFund" };

            var actualReadHandlerTarget = null as object;
            var actualKey = string.Empty;
            var actualSpParams = null as object;

            SetupCallBackForSqlService((spKey, spParams, readHandler) =>
            {
                actualKey = spKey;
                actualSpParams = spParams;
                actualReadHandlerTarget = readHandler.Target;
            });

            //Act
            await _target.GetPendingFunds(metaModel);

            //Assert
            Assert.Equal(expectedSpKey, actualKey);
            expectedSpParams.Should().BeEquivalentTo(actualSpParams as Hashtable);
            expectedReadHandlerTarget.Should().BeEquivalentTo(actualReadHandlerTarget);
        }

        [Fact]
        public async Task GetTodayDeposit_WhenCall_CorrectParams()
        {
            //Arrange
            var metaModel = FakeHelper.Create<BaseSummaryMetaModel>();

            var expectedSpKey = "Lrf_GetLicenseeTodayDeposit";
            var expectedSpParams = ExpectedSPParams(metaModel);
            var expectedReadHandlerTarget = new { columnName = "Deposit" };

            var actualReadHandlerTarget = null as object;
            var actualKey = string.Empty;
            var actualSpParams = null as object;

            SetupCallBackForSqlService((spKey, spParams, readHandler) =>
            {
                actualKey = spKey;
                actualSpParams = spParams;
                actualReadHandlerTarget = readHandler.Target;
            });

            //Act
            await _target.GetTodayDeposit(metaModel);

            //Assert
            Assert.Equal(expectedSpKey, actualKey);
            expectedSpParams.Should().BeEquivalentTo(actualSpParams as Hashtable);
            expectedReadHandlerTarget.Should().BeEquivalentTo(actualReadHandlerTarget);
        }

        [Fact]
        public async Task GetTodayWithdrawal_WhenCall_CorrectParams()
        {
            //Arrange
            var metaModel = FakeHelper.Create<BaseSummaryMetaModel>();

            var expectedSpKey = "Lrf_GetLicenseeTodayWithdrawal";
            var expectedSpParams = ExpectedSPParams(metaModel);
            var expectedReadHandlerTarget = new { columnName = "Withdrawal" };

            var actualReadHandlerTarget = null as object;
            var actualKey = string.Empty;
            var actualSpParams = null as object;

            SetupCallBackForSqlService((spKey, spParams, readHandler) =>
            {
                actualKey = spKey;
                actualSpParams = spParams;
                actualReadHandlerTarget = readHandler.Target;
            });

            //Act
            await _target.GetTodayWithdrawal(metaModel);

            //Assert
            Assert.Equal(expectedSpKey, actualKey);
            expectedSpParams.Should().BeEquivalentTo(actualSpParams as Hashtable);
            expectedReadHandlerTarget.Should().BeEquivalentTo(actualReadHandlerTarget);
        }

        private Hashtable ExpectedSPParams(BaseSummaryMetaModel metaModel, bool hasReport = true)
        {
            var expectedSpParams = new Hashtable();
            expectedSpParams.Add("AgentList", metaModel.AgentListXml);
            expectedSpParams.Add(nameof(metaModel.BaseCurrency), metaModel.BaseCurrency);
            if (hasReport)
            {
                expectedSpParams.Add("ReportDate", metaModel.FromDate);
            }

            return expectedSpParams;
        }

        private void SetupCallBackForSqlService(Action<string, object, Func<IDataReader, decimal>> callBack)
        {
            _mockSqlService.Setup(_ => _.ExecuteQueryWithReadHandler(It.IsAny<string>(), It.IsAny<object>(),
                It.IsAny<Func<IDataReader, decimal>>()))
            .Callback<string, object, Func<IDataReader, decimal>>((spKey, spParams, readHandler) => callBack(spKey, spParams, readHandler));
        }
    }
}