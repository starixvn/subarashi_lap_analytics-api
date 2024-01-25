using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Fanex.Athena.AnalyticsAPI.Common.Attributes;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Data;
using Fanex.Data.Repository;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Common.UnitTests.Services
{
    public class SqlService_ExecuteMultipleQueryData_Test
    {
        private readonly SqlService _targetService;
        private readonly Fixture _fixture;
        private readonly Mock<IObjectDb> _mockObjDB;
        private readonly Mock<CreateObjectDbFunc> _createObjectDbFuncMock;

        public SqlService_ExecuteMultipleQueryData_Test()
        {
            _targetService = new SqlService();
            _mockObjDB = new Mock<IObjectDb>();
            _createObjectDbFuncMock = new Mock<CreateObjectDbFunc>();
            _fixture = new Fixture();

            var mockReader = new Mock<IDataReader>();
            mockReader.Setup(_ => _.Read()).Returns(false);
            _mockObjDB.Setup(_ => _.ExecuteReaderAsync(It.IsAny<ICriteria>(), null, default)).ReturnsAsync(mockReader.Object);

            DbSettingProviderManager.StartTestingSession();
            ObjectDbFactory.SwitchToTestingMode();
        }

        [Fact]
        public async Task ExecuteMultipleQueryData_WhenCall_CreateDBInstanceWithCorrectSettingKey()
        {
            //Arrange
            var settingKey = _fixture.Create<string>();
            var number = _fixture.Create<int>() % 100;
            var criteriaList = CreateMultipleFakeCritera(number, settingKey);
            _createObjectDbFuncMock.Setup(_ => _.Invoke(It.IsAny<string>(), null)).Returns(_mockObjDB.Object);

            ObjectDbFactory.Mock(_createObjectDbFuncMock.Object);

            //Act
            await _targetService.ExecuteMultipleQueryData<DTOClassTest>(criteriaList);

            //Assert
            _createObjectDbFuncMock.Verify(_ => _.Invoke(settingKey, null), Times.Exactly(criteriaList.Length));
        }

        [Fact]
        public async Task ExecuteMultipleQueryData_WhenCall_ExecuteReaderAsyncWithCorrectCriteria()
        {
            //Arrange
            var settingKey = _fixture.Create<string>();
            var number = _fixture.Create<int>() % 100;
            var criteriaList = CreateMultipleFakeCritera(number, settingKey);

            _createObjectDbFuncMock.Setup(_ => _.Invoke(It.IsAny<string>(), null)).Returns(_mockObjDB.Object);

            ObjectDbFactory.Mock(_createObjectDbFuncMock.Object);

            //Act
            await _targetService.ExecuteMultipleQueryData<DTOClassTest>(criteriaList);

            //Assert
            foreach (var critera in criteriaList)
            {
                _mockObjDB.Verify(_ => _.ExecuteReaderAsync(critera, null, default), Times.Once);
            }
        }

        [Fact]
        public async Task ExecuteMultipleQueryData_ReaderIsNull_ReturnEmptyList()
        {
            //Arrange
            var settingKey = _fixture.Create<string>();
            var number = _fixture.Create<int>() % 100;
            var criteriaList = CreateMultipleFakeCritera(number, settingKey);
            var expected = new List<LinkedList<DTOClassTest>>();
            for (int i = 0; i < criteriaList.Length; i++) { expected.Add(new LinkedList<DTOClassTest>()); }

            _createObjectDbFuncMock.Setup(_ => _.Invoke(It.IsAny<string>(), null)).Returns(_mockObjDB.Object);

            ObjectDbFactory.Mock(_createObjectDbFuncMock.Object);

            //Act
            var result = await _targetService.ExecuteMultipleQueryData<DTOClassTest>(criteriaList);

            //Assert
            Assert.Equal(criteriaList.Length, result.ToList().Count);
            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(result));
        }

        private static ICriteria[] CreateMultipleFakeCritera(int number, string settingKey)
        {
            var fakeCriteriaList = new List<ICriteria>();
            for (int i = 0; i < number; i++)
            {
                var criteria = new Mock<ICriteria>();
                criteria.Setup(_ => _.IsValid()).Returns(true);
                criteria.Setup(_ => _.GetSettingKey()).Returns(settingKey);
                fakeCriteriaList.Add(criteria.Object);
            }

            return fakeCriteriaList.ToArray();
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