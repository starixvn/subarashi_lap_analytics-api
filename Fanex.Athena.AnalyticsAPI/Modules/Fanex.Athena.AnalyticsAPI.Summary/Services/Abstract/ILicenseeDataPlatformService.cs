using System.Threading.Tasks;
using Fanex.Athena.AnalyticsAPI.Summary.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Summary.Models.ViewModels;

namespace Fanex.Athena.AnalyticsAPI.Summary.Services
{
    public interface ILicenseeDataPlatformService
    {
        Task<WidgetByPlatformViewModel> GetWinLoss(BaseSummaryMetaModel metaModel);

        Task<WidgetByPlatformViewModel> GetTurnover(BaseSummaryMetaModel metaModel);
    }
}