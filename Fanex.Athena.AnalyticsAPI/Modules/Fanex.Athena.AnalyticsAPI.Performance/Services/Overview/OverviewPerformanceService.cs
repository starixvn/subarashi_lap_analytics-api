using Fanex.Athena.AnalyticsAPI.Common.Repositories;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Models.Criteria;
using Fanex.Athena.AnalyticsAPI.Performance.Models.DTO;
using Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Performance.Services
{
    public class OverviewPerformanceService : PerformanceService, IOverviewPerformanceService
    {
        public OverviewPerformanceService(
               IContextService context,
               ISsasService ssasService,
               IDAXQueryRepository daxQueryRepo)
               : base(context, ssasService, daxQueryRepo)
        {
        }

        public Task<IEnumerable<string>> GetGrowthResult(PerformanceFilterMetalModel filter)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<OverviewTopChangeResultVM>> GetTopChangeResultWithComparison(PerformanceFilterMetalModel filter)
        {
            var criteria = filter.ToCriteria<OverviewTopChangeCriteria>();

            var query = _daxQueryRepo.GetQuery(criteria);
            var topChangeByCurrency = _ssasService.QueryAsync<TopChangeResultDTO>(query);


            //criteria.Perspective = 1;
            //var topChangeByBettype = await _performanceRepo.QueryAsync<TopChangeResultDTO>(criteria);

            //criteria.Perspective = 2;
            //var topChangeByPlatform = await _performanceRepo.QueryAsync<TopChangeResultDTO>(criteria);

            var taskResults = await Task.WhenAll(topChangeByCurrency);

            var result = new List<OverviewTopChangeResultVM>
            {
                new OverviewTopChangeResultVM
                {
                    Perspective = 0,
                    Table = taskResults[0].Select(_ => new OverviewTopChangeResultItemVM
                    {
                        Total = _.Total,
                        PercentChange = new Random().Next(-20, 20)
                    })
                }
            };

            return result;
        }

        public async Task<IEnumerable<OverviewTotalResultVM>> GetTotalResultWithComparison(PerformanceFilterMetalModel filter)
        {
            var criteria = filter.ToCriteria<OverviewTotalCriteria>();
            var query = _daxQueryRepo.GetQuery(criteria);

            var dto = await _ssasService.QueryAsync<TotalResultDTO>(query);
            var first = dto.FirstOrDefault();

            var result = new List<OverviewTotalResultVM>
            {
                new OverviewTotalResultVM
                {
                    Measurement = "Turnover",
                    BaseCurrency = filter.BaseCurrency.Name,
                    Value = first?.Turnover,
                    PercentChange = new Random().Next(-20, 20)
                },
                new OverviewTotalResultVM
                {
                    Measurement = "Gross Profit",
                    BaseCurrency = filter.BaseCurrency.Name,
                    Value = first?.GrossProfit,
                    PercentChange = new Random().Next(-20, 20)
                },
                new OverviewTotalResultVM
                {
                    Measurement = "Margin",
                    BaseCurrency = filter.BaseCurrency.Name,
                    Value = Convert.ToDecimal(first?.Margin),
                    PercentChange = new Random().Next(-20, 20)
                }
            };

            return result;
        }
    }
}