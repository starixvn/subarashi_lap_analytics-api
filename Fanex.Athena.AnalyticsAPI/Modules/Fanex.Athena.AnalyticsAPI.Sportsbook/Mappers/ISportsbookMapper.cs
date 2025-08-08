namespace Fanex.Athena.AnalyticsAPI.Sportsbook.Mappers
{
    using System.Collections.Generic;
    using Models;

    public interface ISportsbookMapper
    {
        IEnumerable<DimDistributionResponse> ToTurnoverResponse(IEnumerable<SportsbookDTO> fullItems, bool includeOther,
          IReadOnlyDictionary<int, IEnumerable<bool>> onlySupportedData = null, IEnumerable<int> excludedFromTopData = null);

        IEnumerable<DimDistributionResponse> ToWinlossResponse(IEnumerable<SportsbookDTO> fullItems, bool includeOther,
           IReadOnlyDictionary<int, IEnumerable<bool>> onlySupportedData = null, IEnumerable<int> excludedFromTopData = null);
    }
}