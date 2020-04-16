using Fanex.Athena.AnalyticsAPI.Core.Models.DataDomain;
using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Core.Services
{
    public interface IAuthInfoService
    {
        string SiteName { get; }
        string UserName { get; }
        int UserId { get; }
        string CurrencyIDList { get; }
        IEnumerable<CurrencyAgentInfo> CurrencyAgentList { get; }
        string Language { get; }
    }
}