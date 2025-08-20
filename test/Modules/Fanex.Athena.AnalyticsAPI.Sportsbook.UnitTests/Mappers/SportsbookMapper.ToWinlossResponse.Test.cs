namespace Fanex.Athena.AnalyticsAPI.Sportsbook.UnitTests.Mappers
{
    using System.Collections.Generic;
    using AutoFixture;
    using Common.Services;
    using Moq;
    using Newtonsoft.Json;
    using Sportsbook.Mappers;
    using Sportsbook.Models;
    using Xunit;

    public class SportsbookMapper_ToWinlossResponse_Test
    {
        private readonly Fixture _fixture;
        private readonly SportsbookMapper _mapper;

        public SportsbookMapper_ToWinlossResponse_Test()
        {
            _fixture = new Fixture();
            _mapper = new SportsbookMapper(Mock.Of<ICalculationService>());
        }

        [Fact]
        public void ToWinlossResponse_WhenCall_ReturnCorrectResult()
        {
            //Arrange
            var data = new List<SportsbookDTO> {
                new SportsbookDTO
                {
                    Id = 1,
                    Name= "Test1",
                    Winloss = 1.0
                },
                new SportsbookDTO
                {
                    Id = 2,
                    Name= "Test2",
                    Winloss = 2.0
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
            var result = _mapper.ToWinlossResponse(data, false);

            //Assert
            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(result));
        }
    }
}