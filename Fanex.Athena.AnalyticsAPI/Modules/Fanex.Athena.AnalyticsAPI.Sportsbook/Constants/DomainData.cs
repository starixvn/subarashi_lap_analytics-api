using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Sportsbook.Constants
{
    public static class DomainData
    {
        public static readonly IReadOnlyDictionary<int, IEnumerable<bool>> SupportedSports = new Dictionary<int, IEnumerable<bool>>
        {
            { 1, new [] { false, true } }, //Soccer, Saba Soccer
            { 2, new [] { false, true } }, //Basketball, Saba Basketball
            { 43, new [] { false } }, //E-Sports
            { 5, new [] { false } }, //Tennis
            { -2, new [] { false } } //Parlay
        };
    }
}