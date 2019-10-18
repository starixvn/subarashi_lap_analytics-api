using Fanex.Data;
using Fanex.Data.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Common.Services
{
    public class DaxQueryContentService : IDaxQueryContentService
    {
        private const string DAX_Query_Output_Name = "DAX_Query";

        public async Task<string> GetDaxQuery<T>(T criteria)
            where T : ICriteria
        {
            var query = string.Empty;
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                await db.ExecuteNonQueryAsync(criteria);
                var output = db.GetOutputParams();
                if (output != null && output.ContainsKey(DAX_Query_Output_Name))
                {
                    query = output[DAX_Query_Output_Name]?.ToString();
                }
            }

            return query;
        }

        public async Task<IEnumerable<string>> GetMultipleDaxQuery<T>(IEnumerable<T> listCriteria)
            where T : ICriteria
        {
            var tasks = new List<Task<string>>(listCriteria.Count());
            foreach (var criteria in listCriteria)
            {
                tasks.Add(GetDaxQuery(criteria));
            }

            var queries = await Task.WhenAll(tasks);

            return queries;
        }
    }
}