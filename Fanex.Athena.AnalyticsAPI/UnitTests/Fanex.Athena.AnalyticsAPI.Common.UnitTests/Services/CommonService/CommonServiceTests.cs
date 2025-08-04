using System;
using System.Data;
using System.Threading.Tasks;
using Fanex.Athena.AnalyticsAPI.Common.Models.Configuration;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Moq;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Common.UnitTests.Services
{
    public class CommonServiceTests
    {
        private readonly Mock<ISqlService> _mockSqlService;
        private readonly CommonService _commonService;

        public CommonServiceTests()
        {
            _mockSqlService = new Mock<ISqlService>();
            _commonService = new CommonService(_mockSqlService.Object);
        }

        [Fact]
        public async Task GetBalanceUp_ShouldReturnBalanceUp_WhenDataReaderHasData()
        {
            // Arrange
            var expectedBalanceUpDay = DateTime.Now.Date;
            var expectedMoveBetsDay = DateTime.Now.Date.AddDays(-1);
            var spKey = "Lrf_SelBalanceUpDay";

            var mockDataReader = new Mock<IDataReader>();
            
            mockDataReader.SetupSequence(r => r.Read())
                .Returns(true) 
                .Returns(false);

            mockDataReader.Setup(r => r["balanceupday"]).Returns(expectedBalanceUpDay);
            mockDataReader.Setup(r => r["movebetsday"]).Returns(expectedMoveBetsDay);

            _mockSqlService.Setup(s => s.ExecuteQueryWithReadHandler(
                    It.Is<string>(key => key == spKey),
                    null,
                    It.IsAny<Func<IDataReader, BalanceUp>>()))
                .Returns<string, object, Func<IDataReader, BalanceUp>>(async (key, param, handler) =>
                {
                    var result = handler(mockDataReader.Object);
                    return await Task.FromResult(result);
                });

            // Act
            var result = await _commonService.GetBalanceUp();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedBalanceUpDay, result.BalanceUpDay);
            Assert.Equal(expectedMoveBetsDay, result.MoveBetsDay);
            _mockSqlService.Verify(s => s.ExecuteQueryWithReadHandler(spKey, null, It.IsAny<Func<IDataReader, BalanceUp>>()), Times.Once);
        }

        [Fact]
        public async Task GetBalanceUp_ShouldReturnEmptyBalanceUp_WhenDataReaderIsEmpty()
        {
            // Arrange
            var spKey = "Lrf_SelBalanceUpDay";
            var mockDataReader = new Mock<IDataReader>();
            
            mockDataReader.Setup(r => r.Read()).Returns(false);

            _mockSqlService.Setup(s => s.ExecuteQueryWithReadHandler(
                    It.Is<string>(key => key == spKey),
                    null,
                    It.IsAny<Func<IDataReader, BalanceUp>>()))
                .Returns<string, object, Func<IDataReader, BalanceUp>>(async (key, param, handler) =>
                {
                    var result = handler(mockDataReader.Object);
                    return await Task.FromResult(result);
                });

            // Act
            var result = await _commonService.GetBalanceUp();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(default(DateTime), result.BalanceUpDay);
            Assert.Equal(default(DateTime), result.MoveBetsDay);
             _mockSqlService.Verify(s => s.ExecuteQueryWithReadHandler(spKey, null, It.IsAny<Func<IDataReader, BalanceUp>>()), Times.Once);
        }
    }
} 