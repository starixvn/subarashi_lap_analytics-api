using Fanex.Athena.AnalyticsAPI.Common.Constants;
using Fanex.Athena.AnalyticsAPI.Common.Resources.Lang;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.RemotingRef;
using Moq;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Common.UnitTest.Services.TranslationSerivce
{ 
    public class TranslationService_Test
    {
        [Fact]
        public void Test()
        {
            var result = LangEnUS.ResourceManager.GetString("product_3");
            Assert.Equal("Number Game", result);
        }

        [Fact]
        public void Test1()
        {
            var mock = new Mock<IRemotingRefService>();

            var x = new TranslationService(mock.Object);
            var result = x.GetProductName("1", LangCodeConst.EnUS, null);

            Assert.Equal("Sportsbook", result);
        }
    }
}