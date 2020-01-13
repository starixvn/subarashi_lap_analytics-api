using Fanex.Data.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Common.Services
{
    public interface IDaxQueryContentService
    {
        Task<string> GetDaxQuery<T>(T criteria)
            where T : ICriteria;

        Task<IEnumerable<string>> GetMultipleDaxQuery<T>(IEnumerable<T> listCriteria)
            where T : ICriteria;
    }
}