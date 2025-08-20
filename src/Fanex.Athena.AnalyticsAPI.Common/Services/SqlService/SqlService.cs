namespace Fanex.Athena.AnalyticsAPI.Common.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using Data;
    using Data.Repository;
    using Extensions;

    public class SqlService : ISqlService
    {
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

        public async Task<IEnumerable<T>> ExecuteQueryData<T>(ICriteria criteria) where T : new()
        {
            using var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey());
            var result = new LinkedList<T>();

            using (var reader = await db.ExecuteReaderAsync(criteria).ConfigureAwait(false))
            {
                while (reader.Read())
                {
                    var data = reader.ReadObject<T>();
                    result.AddLast(data);
                }
            }
            return result;
        }

        public async Task<IEnumerable<IEnumerable<T>>> ExecuteMultipleQueryData<T>(params ICriteria[] listCriteria) where T : new()
        {
            var tasks = new List<Task<IEnumerable<T>>>(listCriteria.Length);
            foreach (var criteria in listCriteria)
            {
                tasks.Add(ExecuteQueryData<T>(criteria));
            }

            var result = await Task.WhenAll(tasks);

            return result;
        }
    }
}