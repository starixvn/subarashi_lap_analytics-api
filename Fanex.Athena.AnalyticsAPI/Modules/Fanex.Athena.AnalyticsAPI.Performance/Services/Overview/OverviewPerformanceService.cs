using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Models.Criteria;
using Fanex.Athena.AnalyticsAPI.Performance.Models.DTO;
using Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels;
using Fanex.Athena.AnalyticsAPI.Performance.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Performance.Services
{
    public class OverviewPerformanceService : PerformanceService, IOverviewPerformanceService
    {
        public OverviewPerformanceService(IContextService context, IPerformanceRepo performanceRepo)
            : base(context, performanceRepo)
        {
        }

        public Task<IEnumerable<string>> GetGrowthResult(PerformanceFilterMetalModel filter)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<string>> GetTopChangeResultWithComparison(PerformanceFilterMetalModel filter)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<OverviewTotalResultVM>> GetTotalResultWithComparison(PerformanceFilterMetalModel filter)
        {
            var criteria = filter.ToCriteria<OverviewTotalCriteria>();

            var dto = await _performanceRepo.QueryAsync<TotalResultDTO>(criteria);
            var first = dto.FirstOrDefault();

            var result = new List<OverviewTotalResultVM>
            {
                new OverviewTotalResultVM
                {
                    Measurement = "Turnover",
                    BaseCurrency = filter.BaseCurrency.Name,
                    Value = first.Turnover,
                    PercentChange = new Random().Next(-20, 20)
                },
                new OverviewTotalResultVM
                {
                    Measurement = "Gross Profit",
                    BaseCurrency = filter.BaseCurrency.Name,
                    Value = first.GrossProfit,
                    PercentChange = new Random().Next(-20, 20)
                },
                new OverviewTotalResultVM
                {
                    Measurement = "Margin",
                    BaseCurrency = filter.BaseCurrency.Name,
                    Value = Convert.ToDecimal(first.Margin),
                    PercentChange = new Random().Next(-20, 20)
                }
            };

            return result;
        }
    }
}