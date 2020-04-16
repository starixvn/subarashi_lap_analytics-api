using Fanex.Athena.AnalyticsAPI.Core.Models.DataDomain;
using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Core.Services
{
    public interface IMetaModelBindingService
    {
        void BindSiteName(IDictionary<string, object> actionArguments, string siteName);

        void BindAgentXml(IDictionary<string, object> actionArguments, IEnumerable<CurrencyAgentInfo> currencyAgentList);
    }
}