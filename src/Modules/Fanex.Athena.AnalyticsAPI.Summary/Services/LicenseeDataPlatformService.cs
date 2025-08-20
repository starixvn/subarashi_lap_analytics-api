namespace Fanex.Athena.AnalyticsAPI.Summary.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Common.Services;
    using Helpers;
    using Models.MetaModels;
    using Models.ViewModels;

    public class LicenseeDataPlatformService : ILicenseeDataPlatformService
    {
        private readonly ISqlService _sqlService;

        private readonly ICommonService _commonService;

        public LicenseeDataPlatformService(ISqlService sqlService, ICommonService commonService)
        {
            _sqlService = sqlService;
            _commonService = commonService;
        }

        public async Task<WidgetByPlatformViewModel> GetTurnover(BaseSummaryMetaModel metaModel)
        {
            const string spKey = "Lrf_GetLicenseeTurnoverByPlatform";
            const string columnName = "Turnover";
            return await GetData(metaModel, spKey, columnName);
        }

        public async Task<WidgetByPlatformViewModel> GetWinLoss(BaseSummaryMetaModel metaModel)
        {
            const string spKey = "Lrf_GetLicenseeWLByPlatform";
            const string columnName = "LicenseeWL";
            return await GetData(metaModel, spKey, columnName);
        }

        private async Task<WidgetByPlatformViewModel> GetData(BaseSummaryMetaModel metaModel, string spKey, string columnName)
        {
            if (string.IsNullOrEmpty(metaModel.AgentListXml))
            {
                return new WidgetByPlatformViewModel { Total = 0, Data = new List<KeyValuePair<string, decimal>>(0) };
            }

            var balanceUp = await _commonService.GetBalanceUp();

            var param = new
            {
                AgentList = metaModel.AgentListXml,
                ProductList = metaModel.ProductCodeList,
                metaModel.FromDate,
                metaModel.ToDate,
                metaModel.BaseCurrency,
                metaModel.SpecialPlatform,
                balanceUp.MoveBetsDay
            };
            var readHandler = ReadHandler(columnName);

            var result = await _sqlService.ExecuteQueryWithReadHandler(spKey, param, readHandler);
            return result;
        }

        [SuppressMessage("Critical Code Smell", "S3353:Unchanged local variables should be \"const\"", Justification = "<Pending>")]
        private static Func<string, Func<IDataReader, WidgetByPlatformViewModel>> ReadHandler = winlossColumnName => reader =>
                {
                    var winlossColumn = winlossColumnName;

                    var dataList = new List<KeyValuePair<string, decimal>>();
                    var total = 0m;
                    while (reader.Read())
                    {
                        var platformType = Convert.ToInt32(reader["PlatformType"]);
                        var key = PlatformHelper.GetPlatformKey(platformType);
                        var value = Convert.ToDecimal(reader[winlossColumn]);
                        total += value;
                        dataList.Add(new KeyValuePair<string, decimal>(key, value));
                    }

                    return new WidgetByPlatformViewModel
                    {
                        Data = dataList,
                        Total = total
                    };
                };
    }
}