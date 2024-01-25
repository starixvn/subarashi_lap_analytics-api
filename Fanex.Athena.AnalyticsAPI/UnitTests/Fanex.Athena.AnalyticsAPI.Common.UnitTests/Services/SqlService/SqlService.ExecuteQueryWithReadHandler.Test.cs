using System.Collections;
using System.Data;
using System.Threading.Tasks;
using AutoFixture;
using Fanex.Athena.AnalyticsAPI.Common.Attributes;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Data;
using Fanex.Data.Repository;
using Moq;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Common.UnitTests.Services
{
    public class SqlService_ExecuteQueryWithReadHandler_Test
    {
        private readonly SqlService _targetService;
        private readonly Fixture _fixture;
        private readonly Mock<IObjectDb> _mockObjDB;
        private readonly Mock<CreateObjectDbFunc> _createObjectDbFuncMock;

        public SqlService_ExecuteQueryWithReadHandler_Test()
        {
            _targetService = new SqlService();
            _mockObjDB = new Mock<IObjectDb>();
            _createObjectDbFuncMock = new Mock<CreateObjectDbFunc>();
            _fixture = new Fixture();

            var mockReader = new Mock<IDataReader>();
            mockReader.Setup(_ => _.Read()).Returns(false);
            _mockObjDB.Setup(_ => _.ExecuteReaderAsync(It.IsAny<string>(), null, default)).ReturnsAsync(mockReader.Object);

            DbSettingProviderManager.StartTestingSession();
            ObjectDbFactory.SwitchToTestingMode();
        }

        [Fact]
        public async Task ExecuteQueryWithReadHandler_WhenCall_CreateDBInstanceWithCorrectSettingKey()
        {
            //Arrange
            var settingKey = _fixture.Create<string>();
            var criteria = new Mock<ICriteria>();
            criteria.Setup(_ => _.IsValid()).Returns(true);
            criteria.Setup(_ => _.GetSettingKey()).Returns(settingKey);

            _createObjectDbFuncMock.Setup(_ => _.Invoke(settingKey, null)).Returns(_mockObjDB.Object);

            ObjectDbFactory.Mock(_createObjectDbFuncMock.Object);

            //Act
            await _targetService.ExecuteQueryWithReadHandler<DTOClassTest>(settingKey, criteria.Object, (reader) => null);

            //Assert
            _createObjectDbFuncMock.Verify(_ => _.Invoke(settingKey, null), Times.Once);
        }

        [Fact]
        public async Task ExecuteQueryWithReadHandler_WhenCall_ExecuteReaderAsyncWithCorrectCriteria()
        {
            //Arrange
            var settingKey = _fixture.Create<string>();
            var criteria = new Mock<ICriteria>();
            criteria.Setup(_ => _.IsValid()).Returns(true);

            _createObjectDbFuncMock.Setup(_ => _.Invoke(It.IsAny<string>(), null)).Returns(_mockObjDB.Object);

            ObjectDbFactory.Mock(_createObjectDbFuncMock.Object);

            //Act
            await _targetService.ExecuteQueryWithReadHandler<DTOClassTest>(settingKey, criteria.Object, (reader) => null);

            //Assert
            _mockObjDB.Verify(_ => _.ExecuteReaderAsync(criteria.Object, null, default), Times.Once);
        }

        [Fact]
        public async Task ExecuteQueryWithReadHandler_ReaderHandlerReturnNull_ReturnNull()
        {
            //Arrange
            var settingKey = _fixture.Create<string>();
            var criteria = new Mock<ICriteria>();
            criteria.Setup(_ => _.IsValid()).Returns(true);

            _createObjectDbFuncMock.Setup(_ => _.Invoke(It.IsAny<string>(), null)).Returns(_mockObjDB.Object);

            ObjectDbFactory.Mock(_createObjectDbFuncMock.Object);

            //Act
            var result = await _targetService.ExecuteQueryWithReadHandler<DTOClassTest>(settingKey, criteria.Object, (reader) => null);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task ExecuteQueryWithReadHandler_ReaderHandlerReturnData_ReturnCorrectData()
        {
            //Arrange
            var expected = _fixture.CreateMany<DTOClassTest>();
            var settingKey = _fixture.Create<string>();
            var criteria = new Mock<ICriteria>();
            criteria.Setup(_ => _.IsValid()).Returns(true);

            _mockObjDB.Setup(_ => _.GetOutputParams()).Returns(null as Hashtable);

            _createObjectDbFuncMock.Setup(_ => _.Invoke(It.IsAny<string>(), null)).Returns(_mockObjDB.Object);

            ObjectDbFactory.Mock(_createObjectDbFuncMock.Object);

            //Act
            var result = await _targetService.ExecuteQueryWithReadHandler(settingKey, criteria.Object, (reader) => expected);

            //Assert
            Assert.Equal(expected, result);
        }

        private class DTOClassTest
        {
            [SsasColumn("prop1")]
            public int Prop1 { get; set; }

            [SsasColumn("prop2")]
            public string Prop2 { get; set; }

            [SsasColumn("prop3")]
            public string Prop3 { get; set; }

            public string Prop4 { get; set; } = "prop4";
        }
    }
}