using System;
using System.Data;
using System.Threading.Tasks;
using Fanex.Data;
using Fanex.Data.Repository;

namespace Fanex.Athena.AnalyticsAPI.Common.Services
{
    public class SqlService : ISqlService
    {
        public async Task<object> ExecuteNonQueryWithOutput(ICriteria criteria, string outputName)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                await db.ExecuteNonQueryAsync(criteria);
                var output = db.GetOutputParams();
                if (output != null && output.ContainsKey(outputName))
                {
                    return output[outputName];
                }
            }
            return null;
        }

        public async Task<T> ExecuteQueryWithReadHandler<T>(string spKey, object spParams, Func<IDataReader, T> readHandler)
        {
            using (var db = ObjectDbFactory.CreateInstance(spKey))
            {
                using (var reader = await db.ExecuteReaderAsync(spParams))
                {
                    return readHandler.Invoke(reader);
                }
            }
        }
    }
}