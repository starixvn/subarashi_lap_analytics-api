using Fanex.Athena.LGAPI.SSAS.Bridge.Attributes;
using Fanex.Athena.LGAPI.SSAS.Bridge.Utils;

namespace Fanex.Athena.AnalyticsAPI.Common.Models.MetaModels
{
    public class GeneralFilterParam : BaseSsasCriteria
    {
        [SsasParamAttribute("siteId")]
        public int LicenseeId { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public int BaseCurrency { get; set; }

        [SsasParamAttribute(NoneParam = true)]
        public ComparisonPeriodParam ComparisonPeriod { get; set; }

        public override string Query
        {
            //just for test
            get => @"   
                              DEFINE
                VAR param_SiteID = @siteId
        
                VAR param_Basecurrency = @baseCurrency
    
                VAR param_FromDate = DATEVALUE(@fromdate)    
                VAR param_ToDate = DATEVALUE(@todate)


                EVALUATE
                  SUMMARIZE(
                     FILTER(
                        'FactSitePerformance',
                        FactSitePerformance[SiteID] = param_SiteID
                        && FactSitePerformance[DateKey] >= param_FromDate
                        && FactSitePerformance[DateKey] <= param_ToDate
                        ),
                     DimSite[Licensee Name],
                         DimCurrency[CurrencyID],
                     DimCurrency[CurrencyName],
                    ""Licensee Turnover"", CALCULATE(
                                        SUMX(RELATEDTABLE(FactCurrencyDaily),[Licensee Turnover])
                                        , DimBaseCurrency[CurrencyID] = param_Basecurrency

                                )
                        )";
        }
    }
}