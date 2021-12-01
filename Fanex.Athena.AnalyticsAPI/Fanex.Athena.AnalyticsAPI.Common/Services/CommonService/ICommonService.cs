namespace Fanex.Athena.AnalyticsAPI.Common.Services
{
    using Fanex.Athena.AnalyticsAPI.Common.Models.Configuration;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICommonService
    {
        Task<BalanceUp> GetBalanceUp();

        Task<Licensee> GetLicenseeInfo(string licenseeName);

        Task<List<Product>> GetProductList(int licenseeId);

        Task<string> GetAllowedSubProductCodeList(int licenseeId);
    }
}