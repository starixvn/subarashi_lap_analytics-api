using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanex.Data.Repository;

namespace Fanex.Athena.AnalyticsAPI.Common.Services
{
    public class DaxQueryContentService : IDaxQueryContentService
    {
        private const string DAX_QUERY_OUTPUT_NAME = "DAX_Query";
        private readonly ISqlService _sqlService;

        public DaxQueryContentService(ISqlService sqlService)
        {
            _sqlService = sqlService;
        }

        public async Task<string> GetDaxQuery<T>(T criteria)
            where T : ICriteria
        {
            var output = await _sqlService.ExecuteNonQueryWithOutput(criteria, DAX_QUERY_OUTPUT_NAME);
            var query = output?.ToString() ?? string.Empty;
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