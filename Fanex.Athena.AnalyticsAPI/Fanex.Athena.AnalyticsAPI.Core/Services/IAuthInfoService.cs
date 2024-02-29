using System.Collections.Generic;
using Fanex.Athena.AnalyticsAPI.Core.Models.DataDomain;

namespace Fanex.Athena.AnalyticsAPI.Core.Services
{
    public interface IAuthInfoService
    {
        string SiteName { get; }

        string UserName { get; }

        int UserId { get; }

        IEnumerable<CurrencyAgentInfo> CurrencyAgentList { get; }

        IEnumerable<string> OfferedSubProductList { get; }

        string Language { get; }

        int SpecialPlatform { get; }
        int SpecialPlatformType { get; }
    }
}