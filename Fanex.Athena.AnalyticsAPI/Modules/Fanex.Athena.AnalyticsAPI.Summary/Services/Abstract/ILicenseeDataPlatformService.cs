using Fanex.Athena.AnalyticsAPI.Summary.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Summary.Models.ViewModels;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Summary.Services
{
    public interface ILicenseeDataPlatformService
    {
        Task<WidgetByPlatformViewModel> GetWinLoss(SummaryBaseMetaModel metaModel);

        Task<WidgetByPlatformViewModel> GetTurnover(SummaryBaseMetaModel metaModel);
    }
}