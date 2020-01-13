using Fanex.Data.Repository;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Common.Services
{
    public interface ISqlService
    {
        Task<object> ExecuteNonQueryWithOutput(ICriteria criteria, string outputName);
    }
}