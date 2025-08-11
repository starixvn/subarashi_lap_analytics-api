namespace Fanex.Athena.AnalyticsAPI.Core.UnitTests.Service
{
    using System.Collections.Generic;
    using System.Linq;
    using AnalyticsAPI.UnitTests.Utils.Helpers;
    using Models.DataDomain;
    using Models.MetaModels;
    using Moq;
    using Services;
    using Xunit;

    public class MetaModelBindingService_Test
    {
        private readonly MetaModelBindingService _target;

        public MetaModelBindingService_Test()
        {
            _target = new MetaModelBindingService();
        }

        [Fact]
        public void BindSiteName_WhenCall_MapCorrectly()
        {
            //Arrange
            var metaModel = Mock.Of<ISiteNameMetaModel>();
            var expectedSiteName = FakeHelper.Create<string>();

            //Action
            _target.BindSiteName(new Dictionary<string, object> { { "metaModel", metaModel } }, expectedSiteName);

            //Assert
            Assert.Equal(expectedSiteName, metaModel.SiteName);
        }

        [Theory]
        [MemberData(nameof(AgentListTestCase))]
        public void BindAgentXml_AgentListIsNullOrEmpty_AgentListXMLIsEmpty(IEnumerable<CurrencyAgentInfo> agentList)
        {
            //Arrange
            var metaModel = Mock.Of<IAgentMetaModel>();
            var expectedAgentXML = string.Empty;

            //Action
            _target.BindAgentXml(new Dictionary<string, object> { { "metaModel", metaModel } }, agentList);

            //Assert
            Assert.Equal(expectedAgentXML, metaModel.AgentListXml);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void BindAgentXml_AllCurrency_MapCorrectly(string currencyIdList)
        {
            //Arrange
            var metaModel = Mock.Of<IAgentMetaModel>();
            var currencyIdListMetaModel = Mock.Of<ICurrencyIdListMetaModel>();
            currencyIdListMetaModel.CurrencyIdList = currencyIdList;

            var agentList = new List<CurrencyAgentInfo>
            {
                new CurrencyAgentInfo
                {
                    AgentId = new[] { 1 ,2 , 3},
                    CurrencyId = 1
                }
            };

            var expectedAgentXML = "<ROOT>" +
                "<Agent AgentId=\"1\" Currency=\"1\"/>" +
                "<Agent AgentId=\"2\" Currency=\"1\"/>" +
                "<Agent AgentId=\"3\" Currency=\"1\"/>" +
                "</ROOT>";

            //Action
            _target.BindAgentXml(new Dictionary<string, object> { { "metaModel", metaModel }, { "currencyIdListMetaModel", currencyIdListMetaModel } }, agentList);

            //Assert
            Assert.Equal(expectedAgentXML, metaModel.AgentListXml);
        }

        [Fact]
        public void BindAgentXml_WithCurrencyIdList_MapCorrectly()
        {
            //Arrange
            var metaModel = Mock.Of<IAgentMetaModel>();
            var currencyIdListMetaModel = Mock.Of<ICurrencyIdListMetaModel>();
            currencyIdListMetaModel.CurrencyIdList = "1,2";

            var agentList = new List<CurrencyAgentInfo>
            {
                new CurrencyAgentInfo
                {
                    AgentId = new[] { 1 ,2 , 3},
                    CurrencyId = 1
                },
                new CurrencyAgentInfo
                {
                    AgentId = new[] { 4 },
                    CurrencyId = 2
                },
                new CurrencyAgentInfo
                {
                    AgentId = new[] { 5, 6, 7 },
                    CurrencyId = 3
                }
            };

            var expectedAgentXML = "<ROOT>" +
                "<Agent AgentId=\"1\" Currency=\"1\"/>" +
                "<Agent AgentId=\"2\" Currency=\"1\"/>" +
                "<Agent AgentId=\"3\" Currency=\"1\"/>" +
                "<Agent AgentId=\"4\" Currency=\"2\"/>" +
                "</ROOT>";

            //Action
            _target.BindAgentXml(new Dictionary<string, object> { { "metaModel", metaModel }, { "currencyIdListMetaModel", currencyIdListMetaModel } }, agentList);

            //Assert
            Assert.Equal(expectedAgentXML, metaModel.AgentListXml);
        }

        public static IEnumerable<object[]> AgentListTestCase() => new List<object[]>
        {
            new object[] { Enumerable.Empty<CurrencyAgentInfo>() },
            new object[] { null }
        };
    }
}