using Fanex.Athena.AnalyticsAPI.Sportsbook.Constants;
using Fanex.Athena.AnalyticsAPI.Sportsbook.Mappers;
using Fanex.Athena.AnalyticsAPI.Sportsbook.Models;
using Fanex.Athena.AnalyticsAPI.Sportsbook.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Sportsbook.Handlers
{
    public class BySportQueryHandler : SportsbookBaseQueryHandler, IRequestHandler<BySportRequest, BySportResponse>
    {
        public BySportQueryHandler(ISportsbookRepository sportsbookRepository, ISportsbookMapper sportsbookMapper)
            : base(sportsbookRepository, sportsbookMapper)
        {
        }

        public async Task<BySportResponse> Handle(BySportRequest request, CancellationToken cancellationToken)
        {
            var rawData = await GetRawData(request);
            return new BySportResponse
            {
                ByTurnover = _sportsbookMapper.ToTurnoverResponse(rawData, includeOther: true, onlySupportedData: DomainData.SupportedSports, includeSabaName: true),
                ByWinloss = _sportsbookMapper.ToWinlossResponse(rawData, includeOther: true, onlySupportedData: DomainData.SupportedSports, includeSabaName: true)
            };
        }
    }
}