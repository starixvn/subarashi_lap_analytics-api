using Fanex.Athena.AnalyticsAPI.Common.Models.Configuration;
using Fanex.Athena.AnalyticsAPI.Common.Services.SqlService;
using Fanex.Athena.LGAPI.SSAS.Bridge.Services;
using Fanex.Data.Repository;
using Microsoft.Extensions.Options;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Common.Repositories
{
    public abstract class BaseRepo
    {
        protected readonly SSASSetting _setting;

        private ISsasBridgeService _bridgeService;

        protected ISsasBridgeService BridgeService
        {
            get
            {
                return _bridgeService ?? (_bridgeService = new SsasBridgeService(
                    _setting?.Modules?.FirstOrDefault(_ => _.ModuledId.Equals(ModuleId))?.ConnectionKey,
                    _setting?.BrigdeHost));
            }
        }

        protected BaseRepo(IOptions<SSASSetting> setting)
        {
            _setting = setting.Value;
        }

        protected virtual SqlService GetSqlService(string key) => new SqlService(key);

        protected Task<IEnumerable<TResult>> QueryAsync<TResult>(string query)
            where TResult : new()
        {
            return BridgeService.QueryAsync<TResult>(query);
        }

        protected async Task<IEnumerable<TResult>> QueryAsync<TResult>(ICustomCriteria criteria)
            where TResult : new()
        {
            var query = string.Empty;
            using (var sqlService = GetSqlService(criteria.GetSettingKey()))
            {
                var queries = await sqlService.QueryAsync<TResult>(criteria.GetSqlParams());
                query = queries.FirstOrDefault()?.ToString();
            }

            var result = await BridgeService.QueryAsync<TResult>(query);

            return result;
        }

        protected abstract string ModuleId { get; }
    }

    public interface ICustomCriteria : ICriteria
    {
        Hashtable GetSqlParams();

        Hashtable GetSSASParams();
    }
}