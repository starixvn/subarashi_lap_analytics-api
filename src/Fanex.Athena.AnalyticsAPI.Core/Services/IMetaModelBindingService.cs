namespace Fanex.Athena.AnalyticsAPI.Core.Services
{
    using System.Collections.Generic;
    using Models.DataDomain;

    public interface IMetaModelBindingService
    {
        void BindSiteName(IDictionary<string, object> actionArguments, string siteName);

        void BindAgentXml(IDictionary<string, object> actionArguments, IEnumerable<CurrencyAgentInfo> currencyAgentList);

        void BindProductCodeList(IDictionary<string, object> actionArguments, IEnumerable<string> offeredSubProductList);
    }
}