using AutoFixture;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Data;
using Fanex.Data.Repository;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Common.UnitTest.Services
{
    public class DaxQueryContentService_GetMultipleDaxQuery_Test
    {
        private readonly DaxQueryContentService _targetService;
        private readonly Fixture _fixture;
        private readonly Mock<ISqlService> _sqlService;

        public DaxQueryContentService_GetMultipleDaxQuery_Test()
        {
            _sqlService = new Mock<ISqlService>();
            _targetService = new DaxQueryContentService(_sqlService.Object);

            _fixture = new Fixture();

            DbSettingProviderManager.StartTestingSession();
            ObjectDbFactory.SwitchToTestingMode();
        }

        [Fact]
        public async Task GetMultipleDaxQuery_ListCriteriaIsEmpty_ReturnEmpty()
        {
            //Arrange
            var listCriteria = Enumerable.Empty<ICriteria>();

            //Act
            var result = await _targetService.GetMultipleDaxQuery(listCriteria);

            //Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetMultipleDaxQuery_WhenCall_ReturnCorrectResult()
        {
            //Arrange
            var listCriteria = new List<ICriteria>(2)
            {
                new Mock<ICriteria>().Object,
                new Mock<ICriteria>().Object,
            };

            var listOutput = new[] { "output1", "output2" };

            for (int i = 0; i < listCriteria.Count; i++)
            {
                _sqlService.Setup(_ => _.ExecuteNonQueryWithOutput(listCriteria[i], "DAX_Query"))
                    .Returns(Task.FromResult<object>(listOutput[i]));
            }

            //Act
            var result = await _targetService.GetMultipleDaxQuery(listCriteria);

            //Assert
            for (int i = 0; i < result.Count(); i++)
            {
                Assert.Equal(listOutput[i], result.ElementAt(i));
            }
         }
    }
}