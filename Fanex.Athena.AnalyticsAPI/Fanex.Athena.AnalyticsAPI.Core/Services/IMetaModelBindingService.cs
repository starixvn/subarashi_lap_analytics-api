using System.Collections.Generic;
using Fanex.Athena.AnalyticsAPI.Core.Models.DataDomain;

namespace Fanex.Athena.AnalyticsAPI.Core.Services
{
    public interface IMetaModelBindingService
    {
        void BindSiteName(IDictionary<string, object> actionArguments, string siteName);

        void BindAgentXml(IDictionary<string, object> actionArguments, IEnumerable<CurrencyAgentInfo> currencyAgentList);

        void BindProductCodeList(IDictionary<string, object> actionArguments, IEnumerable<string> offeredSubProductList);
    }
}