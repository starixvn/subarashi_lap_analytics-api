using Fanex.Athena.AnalyticsAPI.Common.Models.Configuration;
using Fanex.Athena.AnalyticsAPI.Common.Services.SqlService;
using Fanex.Athena.LGAPI.SSAS.Bridge.Services;
using Fanex.Athena.LGAPI.SSAS.Bridge.Utils;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Common.Repositories
{
    public abstract class BaseRepo : IBaseRepo
    {
        protected readonly SSASSetting _setting;

        private ISsasBridgeService _bridgeService;

        protected ISsasBridgeService BridgeService
        {
            get
            {
                return _bridgeService ?? (_bridgeService = new SsasBridgeService(
                    _setting?.Modules?.FirstOrDefault(_ => _.ModuleId.Equals(ModuleId))?.ConnectionKey,
                    _setting?.BridgeHost));
            }
        }

        protected BaseRepo(IOptions<SSASSetting> setting)
        {
            _setting = setting.Value;
        }

        protected virtual SqlService GetSqlService(string key) => new SqlService(key);

        public Task<IEnumerable<TResult>> QueryAsync<TResult>(string query, IDictionary<string, object> parameters = null)
            where TResult : new()
        {
            return BridgeService.QueryAsync<TResult>(query, parameters);
        }

        public Task<IEnumerable<TResult>> QueryAsync<TResult>(ISsasCriteria criteria)
            where TResult : new()
        {
            return BridgeService.QueryAsync<TResult>(criteria.Query, criteria.GetParams());
        }

        protected abstract string ModuleId { get; }
    }
}