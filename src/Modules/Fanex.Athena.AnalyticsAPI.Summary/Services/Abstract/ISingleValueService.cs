namespace Fanex.Athena.AnalyticsAPI.Summary.Services
{
    using System.Threading.Tasks;
    using Models.MetaModels;

    public interface ISingleValueService
    {
        Task<decimal> GetCustomerBalance(BaseSummaryMetaModel metaModel);

        Task<decimal> GetPendingFunds(BaseSummaryMetaModel metaModel);

        Task<decimal> GetTodayDeposit(BaseSummaryMetaModel metaModel);

        Task<decimal> GetTodayWithdrawal(BaseSummaryMetaModel metaModel);
    }
}