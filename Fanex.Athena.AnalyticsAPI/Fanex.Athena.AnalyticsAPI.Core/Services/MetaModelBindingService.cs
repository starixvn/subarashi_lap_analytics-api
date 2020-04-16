using Fanex.Athena.AnalyticsAPI.Core.Models.DataDomain;
using Fanex.Athena.AnalyticsAPI.Core.Models.MetaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.Athena.AnalyticsAPI.Core.Services
{
    public class MetaModelBindingService : IMetaModelBindingService
    {
        public void BindSiteName(IDictionary<string, object> actionArguments, string siteName)
        {
            if (IsMetaModel(actionArguments, out ISiteNameMetaModel itemWithSiteName))
            {
                itemWithSiteName.SiteName = siteName;
            }
        }

        public void BindAgentXml(IDictionary<string, object> actionArguments, IEnumerable<CurrencyAgentInfo> currencyAgentList)
        {
            if (IsMetaModel(actionArguments, out IAgentMetaModel itemWithAgent))
            {
                var currencyIdList = string.Empty;
                if (IsMetaModel(actionArguments, out ICurrencyIdListMetaModel itemWithCurrency))
                {
                    currencyIdList = itemWithCurrency.CurrencyIdList;
                }

                itemWithAgent.AgentListXml = GenerateAgentListXml(currencyAgentList, currencyIdList);
            }
        }

        private static string GenerateAgentListXml(IEnumerable<CurrencyAgentInfo> authCurrencyList, string currencyIdList)
        {
            if (authCurrencyList == null || !authCurrencyList.Any())
            {
                return string.Empty;
            }

            var actualCurrencyAgentList = GetSelectedCurrencyAgentList(authCurrencyList, currencyIdList);

            var builder = new StringBuilder("<ROOT>");
            foreach (var currencyItem in actualCurrencyAgentList)
            {
                var xmlItemList = currencyItem.AgentId.Select(x => $"<Agent AgentId=\"{x}\" Currency=\"{currencyItem.CurrencyId}\"/>");
                builder.Append(string.Join("", xmlItemList));
            }

            builder.Append("</ROOT>");

            return builder.ToString();
        }

        private static IEnumerable<CurrencyAgentInfo> GetSelectedCurrencyAgentList(IEnumerable<CurrencyAgentInfo> authCurrencyList, string currencyIdList)
        {
            if (string.IsNullOrEmpty(currencyIdList))
            {
                return authCurrencyList;
            }

            var selectedCurrencyIdList = ParseCurrencyIdList(currencyIdList);

            return authCurrencyList.Where(x => selectedCurrencyIdList.Contains(x.CurrencyId));
        }

        private static IEnumerable<int> ParseCurrencyIdList(string currencyIdList)
        {
            var selectedCurrencyIdList = currencyIdList.Trim()
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => Convert.ToInt32(x));

            return selectedCurrencyIdList;
        }

        private static bool IsMetaModel<T>(IDictionary<string, object> actionArguments, out T model)
            where T : class
        {
            var metaModel = actionArguments.FirstOrDefault(x => x.Value is T);
            if (metaModel.Value is T value)
            {
                model = value;
                return true;
            }
            model = null;
            return false;
        }
    }
}