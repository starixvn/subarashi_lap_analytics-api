using Fanex.Athena.AnalyticsAPI.Sportsbook.Mappers;
using Fanex.Athena.AnalyticsAPI.Sportsbook.Models;
using Fanex.Athena.AnalyticsAPI.Sportsbook.Models.Criteria;
using Fanex.Athena.AnalyticsAPI.Sportsbook.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Sportsbook.Handlers
{
    public class SportsbookBaseQueryHandler
    {
        protected readonly ISportsbookRepository _sportsbookRepository;
        protected readonly ISportsbookMapper _sportsbookMapper;

        protected SportsbookBaseQueryHandler(ISportsbookRepository sportsbookRepository, ISportsbookMapper sportsbookMapper)
        {
            _sportsbookRepository = sportsbookRepository;
            _sportsbookMapper = sportsbookMapper;
        }

        protected async Task<IEnumerable<SportsbookDTO>> GetRawData(SportsbookBaseRequest request)
        {
            var criteria = GetCriteria(request);
            var rawData = await _sportsbookRepository.QueryAsync<SportsbookDTO>(criteria);
            return rawData;
        }

        private SportsbookCriteria GetCriteria(SportsbookBaseRequest request)
        {
            return new SportsbookCriteria
            {
                SiteName = request.SiteName,
                FromDate = request.FromDate,
                ToDate = request.ToDate,
                BaseCurrencyName = request.BaseCurrency,
                Type = request.TabType,
                SportType = request.SportType
            };
        }
    }
}