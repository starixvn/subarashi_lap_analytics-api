namespace Fanex.Athena.AnalyticsAPI.Performance.Models.Criteria
{
    public class OverviewTotalCriteria : PerformanceCriteria
    {
        public override string Query
        {
            get
            {
                return @"
                        DEFINE
                            VAR param_SiteID = @siteId

                            VAR param_Basecurrency = @baseCurrency

                            VAR param_FromDate = DATEVALUE(@fromDate)
                            VAR param_ToDate = DATEVALUE(@toDate)

                            EVALUATE
                              SUMMARIZE(
                                 FILTER(
                                    'FactSitePerformance',
                                    FactSitePerformance[SiteID] = param_SiteID
                                    && FactSitePerformance[DateKey] >= param_FromDate
                                    && FactSitePerformance[DateKey] <= param_ToDate
                                    ),
                                 DimSite[Licensee Name],
                                 ""Licensee Turnover"", CALCULATE(
                                                    SUMX(RELATEDTABLE(FactCurrencyDaily),[Licensee Turnover] * FactCurrencyDaily[EndOfMonthRate])
                                                    , DimBaseCurrency[CurrencyID] = param_Basecurrency
                                            ),
                                ""Licensee Winloss"", CALCULATE(
                                                    SUMX(RELATEDTABLE(FactCurrencyDaily),[Licensee Winloss] * FactCurrencyDaily[EndOfMonthRate])
                                                    , DimBaseCurrency[CurrencyID] = param_Basecurrency
                                            ),
                                ""Licensee Margin"", [Licensee Winloss] /[Licensee Turnover]
                                )
                                            ";
            }
        }
    }
}