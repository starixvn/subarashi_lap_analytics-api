using AutoFixture;
using Fanex.Athena.AnalyticsAPI.Common.Models.Configuration;
using Fanex.Athena.AnalyticsAPI.Common.Repositories;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.LGAPI.SSAS.Bridge.Services;
using Fanex.Data.Repository;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Common.UnitTest.Repositories
{
    internal class BaseRepositoryTestable : BaseRepository
    {
        public BaseRepositoryTestable(
            IOptions<SSASSetting> setting,
            IDaxQueryContentService daxQueryService,
            ISsasBridgeService bridgeService
            )
            : base(setting, daxQueryService, bridgeService)
        {
        }

        protected override string ModuleId => string.Empty;
    }

    internal class CriteriaTest : ICriteria
    {
        public string GetSettingKey()
        {
            throw new System.NotImplementedException();
        }

        public bool IsValid()
        {
            throw new System.NotImplementedException();
        }
    }

    public class BaseRepository_QueryAsync_Test
    {
        private readonly Fixture _fixture;

        private readonly Mock<IOptions<SSASSetting>> _mockSetting;
        private readonly Mock<IDaxQueryContentService> _mockDaxQueryService;
        private readonly Mock<ISsasBridgeService> _mockBridgeService;

        private BaseRepositoryTestable _baseRepository;

        public BaseRepository_QueryAsync_Test()
        {
            _fixture = new Fixture();

            _mockSetting = new Mock<IOptions<SSASSetting>>();
            _mockDaxQueryService = new Mock<IDaxQueryContentService>();
            _mockBridgeService = new Mock<ISsasBridgeService>();
        }

        [Fact]
        public async Task QueryAsync_QueryContentParam_WhenCall_BridgeServiceCallSameParams()
        {
            //Arrange
            var query = _fixture.Create<string>();
            var expected = _fixture.CreateMany<object>();

            _mockBridgeService.Setup(_ => _.QueryAsync<object>(query, It.IsAny<string>()))
                .Returns(() => Task.FromResult(expected));

            _baseRepository = new BaseRepositoryTestable(_mockSetting.Object, _mockDaxQueryService.Object, _mockBridgeService.Object);

            //Act
            var actual = await _baseRepository.QueryAsync<object>(query);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task QueryAsync_CriteriaParam_WhenCall_BridgeServiceCallWithCorrectQuery()
        {
            //Arrange
            var query = _fixture.Create<string>();
            var expected = _fixture.CreateMany<object>();

            _mockDaxQueryService.Setup(_ => _.GetDaxQuery(It.IsAny<ICriteria>())).Returns(() => Task.FromResult(query));

            _mockBridgeService.Setup(_ => _.QueryAsync<object>(query, It.IsAny<string>()))
                .Returns(() => Task.FromResult(expected));

            _baseRepository = new BaseRepositoryTestable(_mockSetting.Object, _mockDaxQueryService.Object, _mockBridgeService.Object);

            //Act
            var actual = await _baseRepository.QueryAsync<object>(_fixture.Create<CriteriaTest>());

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task QueryAsync_CriteriaParam_WhenCall_DaxQueryServiceCallWithCorrectCriteria()
        {
            //Arrange
            var criteria = _fixture.Create<CriteriaTest>();
            var called = false;

            _mockDaxQueryService.Setup(_ => _.GetDaxQuery(It.IsAny<ICriteria>()))
                .Callback<ICriteria>(_ =>
                {
                    if (_.Equals(criteria))
                    {
                        called = true;
                    }
                });

            _baseRepository = new BaseRepositoryTestable(_mockSetting.Object, _mockDaxQueryService.Object, _mockBridgeService.Object);

            //Act
            await _baseRepository.QueryAsync<object>(criteria);

            //Assert
            Assert.True(called);
        }
    }
}