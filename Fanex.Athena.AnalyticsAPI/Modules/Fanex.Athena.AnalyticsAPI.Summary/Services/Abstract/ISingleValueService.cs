using Fanex.Athena.AnalyticsAPI.Summary.Models.MetaModels;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Summary.Services
{
    public interface ISingleValueService
    {
        Task<decimal> GetCustomerBalance(SummaryBaseMetaModel metaModel);

        Task<decimal> GetPendingFunds(SummaryBaseMetaModel metaModel);

        Task<decimal> GetTodayDeposit(SummaryBaseMetaModel metaModel);

        Task<decimal> GetTodayWithdrawal(SummaryBaseMetaModel metaModel);
    }
}