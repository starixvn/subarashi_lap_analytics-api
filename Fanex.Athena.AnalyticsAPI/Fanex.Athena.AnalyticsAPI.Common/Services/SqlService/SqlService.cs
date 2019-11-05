using Fanex.Data;
using Fanex.Data.Repository;
using System.Threading.Tasks;

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
    }
}