namespace Fanex.Athena.AnalyticsAPI.Summary.Services
{
    using System.Threading.Tasks;
    using Models.MetaModels;
    using Models.ViewModels;

    public interface ILicenseeDataPlatformService
    {
        Task<WidgetByPlatformViewModel> GetWinLoss(BaseSummaryMetaModel metaModel);

        Task<WidgetByPlatformViewModel> GetTurnover(BaseSummaryMetaModel metaModel);
    }
}