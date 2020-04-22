using Fanex.Athena.AnalyticsAPI.Summary.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Summary.Models.ViewModels;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Summary.Services
{
    public interface ILicenseeDataPlatformService
    {
        Task<WidgetByPlatformViewModel> GetWinLoss(BaseSummaryMetaModel metaModel);

        Task<WidgetByPlatformViewModel> GetTurnover(BaseSummaryMetaModel metaModel);
    }
}