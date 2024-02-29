using System.Threading.Tasks;
using Fanex.Athena.AnalyticsAPI.Summary.Models.MetaModels;

namespace Fanex.Athena.AnalyticsAPI.Summary.Services
{
    public interface ISingleValueService
    {
        Task<decimal> GetCustomerBalance(BaseSummaryMetaModel metaModel);

        Task<decimal> GetPendingFunds(BaseSummaryMetaModel metaModel);

        Task<decimal> GetTodayDeposit(BaseSummaryMetaModel metaModel);

        Task<decimal> GetTodayWithdrawal(BaseSummaryMetaModel metaModel);
    }
}