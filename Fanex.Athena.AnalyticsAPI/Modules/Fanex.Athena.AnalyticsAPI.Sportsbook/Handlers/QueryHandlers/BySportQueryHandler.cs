using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fanex.Athena.AnalyticsAPI.Common.Constants;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Core.Services;
using Fanex.Athena.AnalyticsAPI.Sportsbook.Constants;
using Fanex.Athena.AnalyticsAPI.Sportsbook.Mappers;
using Fanex.Athena.AnalyticsAPI.Sportsbook.Models;
using Fanex.Athena.AnalyticsAPI.Sportsbook.Repositories;
using MediatR;

namespace Fanex.Athena.AnalyticsAPI.Sportsbook.Handlers
{
    public class BySportQueryHandler : SportsbookBaseQueryHandler, IRequestHandler<BySportRequest, BySportResponse>
    {
        public BySportQueryHandler(
            ISportsbookRepository sportsbookRepository,
            ISportsbookMapper sportsbookMapper,
            ITranslationService translationService,
            IAuthInfoService authInfoService)
            : base(sportsbookRepository, sportsbookMapper, translationService, authInfoService)
        {
        }

        protected override string TranslationType => TranslationConst.SportType;

        public async Task<BySportResponse> Handle(BySportRequest request, CancellationToken cancellationToken)
        {
            var rawData = (await GetRawData(request)).ToList();
            CombineParlay(rawData);

            return new BySportResponse
            {
                ByTurnover = Translate(_sportsbookMapper.ToTurnoverResponse(rawData, includeOther: true, onlySupportedData: DomainData.SupportedSports)),
                ByWinloss = Translate(_sportsbookMapper.ToWinlossResponse(rawData, includeOther: true, onlySupportedData: DomainData.SupportedSports))
            };
        }

        private void CombineParlay(IList<SportsbookDTO> rawData)
        {
            var sabaParlay = rawData.FirstOrDefault(_ => _.Id == DomainData.ParlaySportId && _.IsSaba);
            if (sabaParlay == null)
            {
                return;
            }
            rawData.Remove(sabaParlay);

            var nonSabaParlay = rawData.FirstOrDefault(_ => _.Id == DomainData.ParlaySportId && !_.IsSaba);
            if (nonSabaParlay == null)
            {
                nonSabaParlay = new SportsbookDTO
                {
                    Id = DomainData.ParlaySportId,
                    Name = sabaParlay.Name,
                    IsSaba = false
                };

                rawData.Add(nonSabaParlay);
            }

            nonSabaParlay.Turnover += sabaParlay.Turnover;
            nonSabaParlay.Winloss += sabaParlay.Winloss;
            nonSabaParlay.TurnoverNoDraw += sabaParlay.TurnoverNoDraw;
        }
    }
}