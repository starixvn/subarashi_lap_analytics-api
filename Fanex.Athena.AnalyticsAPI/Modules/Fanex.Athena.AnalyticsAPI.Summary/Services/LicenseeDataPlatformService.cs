using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Summary.Helpers;
using Fanex.Athena.AnalyticsAPI.Summary.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Summary.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Summary.Services
{
    public class LicenseeDataPlatformService : ILicenseeDataPlatformService
    {
        private readonly ISqlService _sqlService;

        public LicenseeDataPlatformService(ISqlService sqlService)
        {
            _sqlService = sqlService;
        }

        public async Task<WidgetByPlatformViewModel> GetTurnover(SummaryBaseMetaModel metaModel)
        {
            const string spKey = "Lrf_GetLicenseeTurnoverByPlatform";
            const string columnName = "Turnover";
            return await GetData(metaModel, spKey, columnName);
        }

        public async Task<WidgetByPlatformViewModel> GetWinLoss(SummaryBaseMetaModel metaModel)
        {
            const string spKey = "Lrf_GetLicenseeWLByPlatform";
            const string columnName = "LicenseeWL";
            return await GetData(metaModel, spKey, columnName);
        }

        private async Task<WidgetByPlatformViewModel> GetData(SummaryBaseMetaModel metaModel, string spKey, string columnName)
        {
            if (string.IsNullOrEmpty(metaModel.AgentListXml))
            {
                return new WidgetByPlatformViewModel { Total = 0, Data = new List<KeyValuePair<string, decimal>>(0) };
            }

            var param = new
            {
                AgentList = metaModel.AgentListXml,
                metaModel.FromDate,
                metaModel.ToDate,
                metaModel.BaseCurrency
            };
            var readHandler = ReadHandler(columnName);

            var result = await _sqlService.ExecuteQueryWithReadHandler(spKey, param, readHandler);
            return result;
        }

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