using System;
using System.Linq;
using Fanex.Athena.AnalyticsAPI.Common.Consts;
using Fanex.Athena.AnalyticsAPI.Core.Constants;
using Fanex.Athena.AnalyticsAPI.Core.Models.MetaModels;

namespace Fanex.Athena.AnalyticsAPI.Summary.Models.MetaModels
{
    public class BaseSummaryMetaModel : IAgentMetaModel, IValidationMetaModel, ICurrencyIdListMetaModel, IProductCodeListMetaModel
    {
        private int _currencyId;

        public int CurrencyId
        {
            get
            {
                return _currencyId;
            }

            set
            {
                _currencyId = value;
                if (value > 0)
                {
                    CurrencyIdList = value.ToString();
                }
            }
        }

        public string CurrencyIdList { get; set; }

        public string BaseCurrency { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string SelectedWidgetString { get; set; }

        public virtual bool IsValid
        {
            get
            {
                return string.IsNullOrEmpty(BaseCurrency)
                    || CurrencyConst.SupportedBaseCurrencies.Contains(BaseCurrency.ToUpperInvariant());
            }
        }

        public string AgentListXml { get; set; }

        public int SpecialPlatform { get; set; }

        public string ProductCodeList { get; set; } = ProductCode.SB;
    }
}