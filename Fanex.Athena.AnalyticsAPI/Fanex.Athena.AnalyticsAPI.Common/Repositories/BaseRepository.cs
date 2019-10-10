using Fanex.Athena.AnalyticsAPI.Common.Models.Configuration;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.LGAPI.SSAS.Bridge.Models;
using Fanex.Athena.LGAPI.SSAS.Bridge.Services;
using Fanex.Data.Repository;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Common.Repositories
{
    public abstract class BaseRepository : IBaseRepository
    {
        protected readonly SSASSetting _setting;

        protected readonly IDaxQueryContentService _daxQueryService;

        protected readonly ISsasBridgeService _bridgeService;

        protected BaseRepository(
            IOptions<SSASSetting> setting,
            IDaxQueryContentService daxQueryService,
            ISsasBridgeService bridgeService
            )
        {
            _setting = setting.Value;
            _daxQueryService = daxQueryService;

            bridgeService.Setting = new BridgeSetting
            {
                BridgeHost = _setting?.BridgeHost,
                ConnectionKey = _setting?.Modules?.FirstOrDefault(_ => _.ModuleId.Equals(ModuleId))?.ConnectionKey,
                AuthKey = _setting?.AuthKey
            };

            _bridgeService = bridgeService;
        }

        public Task<IEnumerable<TResult>> QueryAsync<TResult>(string query, IDictionary<string, object> parameters = null)
            where TResult : new()
        {
            return _bridgeService.QueryAsync<TResult>(query, parameters);
        }

        public async Task<IEnumerable<TResult>> QueryAsync<TResult>(ICriteria criteria)
            where TResult : new()
        {
            var daxQuery = await _daxQueryService.GetDaxQuery(criteria);
            var result = await _bridgeService.QueryAsync<TResult>(daxQuery);

            return result;
        }

        protected abstract string ModuleId { get; }
    }
}