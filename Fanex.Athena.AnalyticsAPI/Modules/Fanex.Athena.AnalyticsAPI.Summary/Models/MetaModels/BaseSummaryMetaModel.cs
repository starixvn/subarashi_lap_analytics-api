namespace Fanex.Athena.AnalyticsAPI.Summary.Models.MetaModels
{
    using System;
    using System.Linq;
    using Common.Consts;
    using Core.Constants;
    using Core.Models.MetaModels;

    public class BaseSummaryMetaModel : IAgentMetaModel, IValidationMetaModel, ICurrencyIdListMetaModel, IProductCodeListMetaModel
    {
        private int _currencyId;

        public int CurrencyId
        {
            get { return _currencyId; }

            set
            {
                _currencyId = value;
                if (value > 0)
                {
                    CurrencyIdList = value.ToString();
                }
            }
        }

        public string BaseCurrency { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string SelectedWidgetString { get; set; }

        public int SpecialPlatform { get; set; }

        public string AgentListXml { get; set; }

        public string CurrencyIdList { get; set; }

        public string ProductCodeList { get; set; } = ProductCode.SB;

        public virtual bool IsValid => string.IsNullOrEmpty(BaseCurrency) || CurrencyConst.SupportedBaseCurrencies.Contains(BaseCurrency.ToUpperInvariant());
    }
}