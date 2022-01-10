using AutoFixture;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Data;
using Fanex.Data.Repository;
using Moq;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Common.UnitTests.Services
{
    public class SqlService_ExecuteNonQueryWithOutput_Test
    {
        private readonly SqlService _targetService;
        private readonly Fixture _fixture;
        private readonly Mock<IObjectDb> _mockObjDB;
        private readonly Mock<CreateObjectDbFunc> _createObjectDbFuncMock;

        public SqlService_ExecuteNonQueryWithOutput_Test()
        {
            _targetService = new SqlService();
            _mockObjDB = new Mock<IObjectDb>();
            _createObjectDbFuncMock = new Mock<CreateObjectDbFunc>();

            _fixture = new Fixture();

            DbSettingProviderManager.StartTestingSession();
            ObjectDbFactory.SwitchToTestingMode();
        }

        [Fact]
        public async Task ExecuteNonQueryWithOutput_WhenCall_CreateDBInstanceWithCorrectSettingKey()
        {
            //Arrange
            var settingKey = _fixture.Create<string>();

            var criteria = new Mock<ICriteria>();
            criteria.Setup(_ => _.IsValid()).Returns(true);
            criteria.Setup(_ => _.GetSettingKey()).Returns(settingKey);

            _createObjectDbFuncMock.Setup(_ => _.Invoke(settingKey, null)).Returns(_mockObjDB.Object);

            ObjectDbFactory.Mock(_createObjectDbFuncMock.Object);

            //Act
            await _targetService.ExecuteNonQueryWithOutput(criteria.Object, _fixture.Create<string>());

            //Assert
            _createObjectDbFuncMock.Verify(_ => _.Invoke(settingKey, null), Times.Once);
        }

        [Fact]
        public async Task ExecuteNonQueryWithOutput_WhenCall_ExecuteNonQueryAsyncWithCorrectCriteria()
        {
            //Arrange
            var criteria = new Mock<ICriteria>();
            criteria.Setup(_ => _.IsValid()).Returns(true);

            _createObjectDbFuncMock.Setup(_ => _.Invoke(It.IsAny<string>(), null)).Returns(_mockObjDB.Object);

            ObjectDbFactory.Mock(_createObjectDbFuncMock.Object);

            //Act
            await _targetService.ExecuteNonQueryWithOutput(criteria.Object, _fixture.Create<string>());

            //Assert
            _mockObjDB.Verify(_ => _.ExecuteNonQueryAsync(criteria.Object, null, default(CancellationToken)), Times.Once);
        }

        [Fact]
        public async Task ExecuteNonQueryWithOutput_OutputNull_ReturnNull()
        {
            //Arrange
            var criteria = new Mock<ICriteria>();
            criteria.Setup(_ => _.IsValid()).Returns(true);

            _mockObjDB.Setup(_ => _.GetOutputParams()).Returns(null as Hashtable);

            _createObjectDbFuncMock.Setup(_ => _.Invoke(It.IsAny<string>(), null)).Returns(_mockObjDB.Object);

            ObjectDbFactory.Mock(_createObjectDbFuncMock.Object);

            //Act
            var result = await _targetService.ExecuteNonQueryWithOutput(criteria.Object, _fixture.Create<string>());

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task ExecuteNonQueryWithOutput_OutputIsNotNullAndContainsKey_ReturnCorrectResult()
        {
            //Arrange
            var criteria = new Mock<ICriteria>();
            criteria.Setup(_ => _.IsValid()).Returns(true);

            var outputKey = "DAX_Query";

            var output = new Hashtable
            {
                { outputKey, "output result" }
            };

            _mockObjDB.Setup(_ => _.GetOutputParams()).Returns(output);

            _createObjectDbFuncMock.Setup(_ => _.Invoke(It.IsAny<string>(), null)).Returns(_mockObjDB.Object);

            ObjectDbFactory.Mock(_createObjectDbFuncMock.Object);

            //Act
            var result = await _targetService.ExecuteNonQueryWithOutput(criteria.Object, outputKey);

            //Assert
            Assert.Equal(output[outputKey], result);
        }

        [Fact]
        public async Task GetDaxQuery_OutputIsNotNullAndNoContainsKey_ReturnNull()
        {
            //Arrange
            var criteria = new Mock<ICriteria>();
            criteria.Setup(_ => _.IsValid()).Returns(true);

            var output = new Hashtable
            {
                { "DAX_Query_No_Key", "abc" }
            };

            _mockObjDB.Setup(_ => _.GetOutputParams()).Returns(output);

            _createObjectDbFuncMock.Setup(_ => _.Invoke(It.IsAny<string>(), null)).Returns(_mockObjDB.Object);

            ObjectDbFactory.Mock(_createObjectDbFuncMock.Object);

            //Act
            var result = await _targetService.ExecuteNonQueryWithOutput(criteria.Object, "DAX_Query");

            //Assert
            Assert.Null(result);
        }
    }
}