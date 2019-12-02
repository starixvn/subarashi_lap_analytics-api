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

        public Task<IEnumerable<TResult>> QueryAsync<TResult>(string query)
            where TResult : new()
        {
            return _bridgeService.QueryAsync<TResult>(query);
        }

        public async Task<IEnumerable<TResult>> QueryAsync<TResult>(ICriteria criteria)
            where TResult : new()
        {
            var daxQuery = await _daxQueryService.GetDaxQuery(criteria);
            var result = await _bridgeService.QueryAsync<TResult>(daxQuery);

            return result;
        }

        public async Task<IEnumerable<IEnumerable<TResult>>> QueryMultipleAsync<TResult>(params ICriteria[] listCriteria)
            where TResult : new()
        {
            var daxQueries = await _daxQueryService.GetMultipleDaxQuery(listCriteria);
            var result = await _bridgeService.QueryMultipleAsync<TResult>(daxQueries);

            return result;
        }

        public async Task<IEnumerable<IEnumerable<TResult>>> QueryMultipleAsync<TResult>(IEnumerable<string> queries)
            where TResult : new()
        {
            var result = await _bridgeService.QueryMultipleAsync<TResult>(queries);

            return result;
        }

        protected abstract string ModuleId { get; }
    }
}