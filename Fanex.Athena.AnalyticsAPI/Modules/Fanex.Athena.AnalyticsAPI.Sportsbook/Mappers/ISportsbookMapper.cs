using System.Collections.Generic;
using Fanex.Athena.AnalyticsAPI.Sportsbook.Models;

namespace Fanex.Athena.AnalyticsAPI.Sportsbook.Mappers
{
    public interface ISportsbookMapper
    {
        IEnumerable<DimDistributionResponse> ToTurnoverResponse(IEnumerable<SportsbookDTO> fullItems, bool includeOther,
          IReadOnlyDictionary<int, IEnumerable<bool>> onlySupportedData = null, IEnumerable<int> excludedFromTopData = null);

        IEnumerable<DimDistributionResponse> ToWinlossResponse(IEnumerable<SportsbookDTO> fullItems, bool includeOther,
           IReadOnlyDictionary<int, IEnumerable<bool>> onlySupportedData = null, IEnumerable<int> excludedFromTopData = null);
    }
}