using Fanex.Athena.AnalyticsAPI.Common.Models.Configuration;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Common.Services
{
    public class CommonService : ICommonService
    {
        private readonly ISqlService _sqlService;

        public CommonService(ISqlService sqlService)
        {
            _sqlService = sqlService;
        }

        public async Task<BalanceUp> GetBalanceUp()
        {
            const string spKey = "Lrf_SelBalanceUpDay";

            var result = await _sqlService.ExecuteQueryWithReadHandler(spKey, null, ReadBalanceUpHandler);

            return result;
        }

        private static Func<IDataReader, BalanceUp> ReadBalanceUpHandler = reader =>
        {
            BalanceUp balanceup = new BalanceUp();

            if (reader.Read())
            {
                balanceup.BalanceUpDay = Convert.ToDateTime(reader["balanceupday"]);
                balanceup.MoveBetsDay = Convert.ToDateTime(reader["movebetsday"]);
            }
            return balanceup;
        };


    }
}