using Fanex.Data.Repository;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Common.Services
{
    public interface IDaxQueryService
    {
        Task<string> GetDaxQuery<T>(T criteria)
            where T : ICriteria;
    }
}