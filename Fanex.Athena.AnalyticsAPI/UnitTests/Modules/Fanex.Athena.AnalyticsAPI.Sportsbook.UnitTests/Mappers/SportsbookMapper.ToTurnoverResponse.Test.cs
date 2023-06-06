using AutoFixture;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Sportsbook.Mappers;
using Fanex.Athena.AnalyticsAPI.Sportsbook.Models;
using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Sportsbook.UnitTests.Mappers
{
    public class SportsbookMapper_ToTurnoverResponse_Test
    {
        private readonly Fixture _fixture;
        private readonly SportsbookMapper _mapper;

        public SportsbookMapper_ToTurnoverResponse_Test()
        {
            _fixture = new Fixture();
            _mapper = new SportsbookMapper(Mock.Of<ICalculationService>());
        }

        [Fact]
        public void ToTurnoverResponse_WhenCall_ReturnCorrectResult()
        {
            //Arrange
            var data = new List<SportsbookDTO> {
                new SportsbookDTO
                {
                    Id = 1,
                    Name= "Test1",
                    Turnover = 1.0
                },
                new SportsbookDTO
                {
                    Id = 2,
                    Name= "Test2",
                    Turnover = 2.0
                },
            };
            var expected = new List<DimDistributionResponse>
            {
                new DimDistributionResponse
                {
                    Id = 2,
                    Name = "Test2",
                    Value = 2.0,
                },
                new DimDistributionResponse
                {
                    Id = 1,
                    Name = "Test1",
                    Value = 1.0,
                },
            };

            //Action
            var result = _mapper.ToTurnoverResponse(data, false);

            //Assert
            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(result));
        }
    }
}