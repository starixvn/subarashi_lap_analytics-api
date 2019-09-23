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
        public OverviewPerformanceService(
            IContextService context,
            IPerformanceRepository performanceRepository
            ) : base(context, performanceRepository)
        {
        }

        public Task<IEnumerable<string>> GetGrowthResultWithComparison(PerformanceFilterMetaModel filter)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<OverviewTopChangeResultVM>> GetTopChangeResultWithComparison(PerformanceFilterMetaModel filter)
        {
            var criteria = filter.ToCriteria<OverviewTopChangeCriteria>();

            var topChangeByCurrency = await _performanceRepository.QueryAsync<TopChangeResultDTO>(criteria);

            //criteria.Perspective = 1;
            //var topChangeByBettype = await _performanceRepo.QueryAsync<TopChangeResultDTO>(criteria);


            //criteria.Perspective = 2;
            //var topChangeByPlatform = await _performanceRepo.QueryAsync<TopChangeResultDTO>(criteria);

            //var taskResults = await Task.WhenAll(topChangeByCurrency);

            //var result = new List<OverviewTopChangeResultVM>
            //{
            //    new OverviewTopChangeResultVM
            //    {
            //        Perspective = 0,
            //        Table = taskResults[0].Select(_ => new OverviewTopChangeResultItemVM
            //        {
            //            Total = _.Total,
            //            PercentChange = new Random().Next(-20, 20)
            //        })
            //    }
            //};

            //return result;

            return null;
        }

        public async Task<IEnumerable<OverviewTotalResultVM>> GetSummaryResultWithComparison(PerformanceFilterMetaModel filter)
        {

            //var criteria = filter.ToCriteria<OverviewTotalCriteria>();
            //var query = await _daxQueryRepo.GetQuery(criteria);

            //var dto = await _ssasService.QueryAsync<TotalResultDTO>(query);
            //var first = dto.FirstOrDefault();

            //var result = new List<OverviewTotalResultVM>
            //{
            //    new OverviewTotalResultVM
            //    {
            //        Measurement = "Turnover",
            //        BaseCurrency = filter.BaseCurrency.Name,
            //        Value = first?.Turnover,
            //        PercentChange = new Random().Next(-20, 20)
            //    },
            //    new OverviewTotalResultVM
            //    {
            //        Measurement = "Gross Profit",
            //        BaseCurrency = filter.BaseCurrency.Name,
            //        Value = first?.GrossProfit,
            //        PercentChange = new Random().Next(-20, 20)
            //    },
            //    new OverviewTotalResultVM
            //    {
            //        Measurement = "Margin",
            //        BaseCurrency = filter.BaseCurrency.Name,
            //        Value = Convert.ToDecimal(first?.Margin),
            //        PercentChange = new Random().Next(-20, 20)
            //    }
            //};

            //return result;

            return null;
        }
    }
}