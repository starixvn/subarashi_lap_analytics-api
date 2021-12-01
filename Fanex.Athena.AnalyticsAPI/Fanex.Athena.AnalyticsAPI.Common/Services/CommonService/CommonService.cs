using Fanex.Athena.AnalyticsAPI.Common.Models.Configuration;
using Fanex.Athena.AnalyticsAPI.Common.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Common.Services
{
    public class CommonService : ICommonService
    {
        private const int OneDayInSecond = 86400;

        private readonly ISqlService _sqlService;
        private readonly ICustomMemoryCacheService _cache;

        public CommonService(ISqlService sqlService, ICustomMemoryCacheService cache)
        {
            _sqlService = sqlService;
            _cache = cache;
        }

        public async Task<BalanceUp> GetBalanceUp()
        {
            const string spKey = "Lrf_SelBalanceUpDay";

            var result = await _sqlService.ExecuteQueryWithReadHandler(spKey, null, ReadBalanceUpHandler);

            return result;
        }

        private static Func<IDataReader, BalanceUp> ReadBalanceUpHandler = reader =>
        {
            BalanceUp balanceup = new BalanceUp();

            if (reader.Read())
            {
                balanceup.BalanceUpDay = Convert.ToDateTime(reader["balanceupday"]);
                balanceup.MoveBetsDay = Convert.ToDateTime(reader["movebetsday"]);
            }
            return balanceup;
        };

        public async Task<Licensee> GetLicenseeInfo(string licenseeName)
        {
            var cacheKey = "Lrf_LicenseeSelBySiteName_" + licenseeName;
            var licenseeInfo = _cache.Get<Licensee>(cacheKey);
            if (licenseeInfo == null)
            {
                const string spKey = "Lrf_LicenseeSelBySiteName";

                var uspParams = new { siteName = licenseeName };

                licenseeInfo = await _sqlService.ExecuteQueryWithReadHandler(spKey, uspParams, ReadLicenseeInfoHandler);

                if (licenseeInfo != null)
                {
                    _cache.Set(cacheKey, licenseeInfo, OneDayInSecond);
                }
            }

            return licenseeInfo;
        }

        public async Task<string> GetAllowedSubProductCodeList(int licenseeId)
        {
            var originalProducts = await GetProductList(licenseeId);
            var subSBProductList = new List<string> { Product.SLT };

            var subProducts = originalProducts.Where(i => subSBProductList.Contains(i.ProductCode)).Select(_ => _.ProductCode);
            if (subProducts.Any())
            {
                return Product.SB + "," + string.Join(",", subProducts);
            }

            return Product.SB;
        }

        public async Task<List<Product>> GetProductList(int licenseeId)
        {
            var cacheKey = "MemoryCache_Common_Product" + licenseeId;
            var productList = _cache.Get<List<Product>>(cacheKey);

            if (productList != null)
            {
                return productList;
            }

            const string spKey = "Lrf_LicenseeProduct3rdSel";
            var uspParams = new { licenseeId };
            productList = await _sqlService.ExecuteQueryWithReadHandler(spKey, uspParams, ReadProductHandler);

            _cache.Set(cacheKey, productList);

            return productList;
        }

        private static Func<IDataReader, Licensee> ReadLicenseeInfoHandler = reader =>
        {
            var licensee = new Licensee();

            if (reader.Read())
            {
                licensee.Id = Convert.ToInt32(reader["LicenseeID"]);
                licensee.Name = Convert.ToString(reader["LicenseeName"]);
                licensee.IsAllowedCustomerComm = Convert.ToBoolean(reader["IsAllowedMemberComm"]);
                licensee.IsEnableBA = Convert.ToBoolean(reader["EnableBA"]);
            }
            return licensee;
        };

        private static Func<IDataReader, List<Product>> ReadProductHandler = reader =>
        {
            var allowedProducts = new List<Product> { new Product { IsOfferedSingleWallet = true, ProductCode = "SB" } };

            while (reader.Read())
            {
                allowedProducts.Add(new Product
                {
                    ProductID = Convert.ToInt32(reader["ProductID"]),
                    ProductCode = Convert.ToString(reader["productprefix"]),
                    AllowCommission = Convert.ToBoolean(reader["comm"]),
                    AllowWinloss = Convert.ToBoolean(reader["winloss"]),
                    IsOfferedSingleWallet = Convert.ToBoolean(reader["IsOfferedSingleWallet"])
                });
            }

            return allowedProducts;
        };
    }
}