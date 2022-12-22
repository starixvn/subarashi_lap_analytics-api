using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Sportsbook.Constants
{
    public static class DomainData
    {
        public const int ParlaySportId = -2;

        public static readonly IReadOnlyDictionary<int, IEnumerable<bool>> SupportedSports = new Dictionary<int, IEnumerable<bool>>
        {
            { 1, new [] { false, true } }, //Soccer, Saba Soccer
            { 2, new [] { false, true } }, //Basketball, Saba Basketball
            { 50, new [] { false } }, //Cricket
            { 43, new [] { false, true } }, //E-Sports
            { 5, new [] { false } }, //Tennis
            { -2, new [] { false } } //Parlay
        };

        public static readonly IReadOnlyDictionary<int, IEnumerable<bool>> SupportedSoccerLeagues = new Dictionary<int, IEnumerable<bool>>
        {
            { 3, new [] { false } }, //ENGLISH PREMIER LEAGUE
            { 4, new [] { false } }, //ITALY SERIE A
            { 5, new [] { false } }, //GERMANY-BUNDESLIGA I
            { 13, new [] { false } }, //UEFA Champions League
            { 43, new [] { false } }, //SPAIN PRIMERA LALIGA
        };

        public static readonly IReadOnlyDictionary<int, IEnumerable<bool>> SupportedCricketLeagues = new Dictionary<int, IEnumerable<bool>>
        {
            { 53181, new [] { false } }, //Big Bash League
            { 53206, new [] { false } }, //Caribbean Premier League
            { 53284, new [] { false } }, //Indian Premier League
            { 96359, new [] { false } }, //Abu Dhabi T10 League
            { 62837, new [] { false } }, //Pakistan Super League
        };

        public static readonly IEnumerable<int> ExcludedPlatformsFromTop = new[]
        {
            -1  //Other Platform
        };
    }
}