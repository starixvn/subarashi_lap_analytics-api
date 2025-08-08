namespace Fanex.Athena.AnalyticsAPI.Summary.Services
{
    using System;
    using System.Collections;
    using System.Data;
    using System.Threading.Tasks;
    using Common.Services;
    using Models.MetaModels;

    public class SingleValueService : ISingleValueService
    {
        private readonly ISqlService _sqlService;

        public SingleValueService(ISqlService sqlService)
        {
            _sqlService = sqlService;
        }

        public async Task<decimal> GetCustomerBalance(BaseSummaryMetaModel metaModel)
        {
            return await GetData(metaModel, "Lrf_GetCustomerBalance", "ClosingBalance").ConfigureAwait(false);
        }

        public async Task<decimal> GetPendingFunds(BaseSummaryMetaModel metaModel)
        {
            return await GetData(metaModel, "Lrf_GetLicenseePendingFunds", "PendingFund", false).ConfigureAwait(false);
        }

        public async Task<decimal> GetTodayDeposit(BaseSummaryMetaModel metaModel)
        {
            return await GetData(metaModel, "Lrf_GetLicenseeTodayDeposit", "Deposit").ConfigureAwait(false);
        }

        public async Task<decimal> GetTodayWithdrawal(BaseSummaryMetaModel metaModel)
        {
            return await GetData(metaModel, "Lrf_GetLicenseeTodayWithdrawal", "Withdrawal").ConfigureAwait(false);
        }

        private async Task<decimal> GetData(BaseSummaryMetaModel metaModel, string spKey, string columnName, bool hasReportDate = true)
        {
            if (string.IsNullOrEmpty(metaModel.AgentListXml))
            {
                return 0;
            }

            var param = new Hashtable();
            param.Add("AgentList", metaModel.AgentListXml);
            param.Add("BaseCurrency", metaModel.BaseCurrency);
            if (hasReportDate)
            {
                param.Add("ReportDate", metaModel.FromDate);
            }

            var readHandler = ReadHandler(columnName);

            var result = await _sqlService.ExecuteQueryWithReadHandler(spKey, param, readHandler);
            return result;
        }

        private static Func<string, Func<IDataReader, decimal>> ReadHandler = columnName => reader =>
        {
            return reader.Read() && !reader.IsDBNull(reader.GetOrdinal(columnName)) ? Convert.ToDecimal(reader[columnName]) : 0;
        };
    }
}