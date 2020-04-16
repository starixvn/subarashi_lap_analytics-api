using Fanex.Athena.AnalyticsAPI.Common.Constants;
using Fanex.Athena.AnalyticsAPI.Common.Controllers;
using Fanex.Athena.AnalyticsAPI.Common.Extensions;
using Fanex.Athena.AnalyticsAPI.Common.Mappers;
using Fanex.Athena.AnalyticsAPI.Common.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Core.Attributes;
using Fanex.Athena.AnalyticsAPI.Core.Constants;
using Fanex.Athena.AnalyticsAPI.Performance.Constants;
using Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels;
using Fanex.Athena.AnalyticsAPI.Performance.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Performance.Controllers
{
    [ApiController]
    [ONEBookAuthorization(Permissions = PermissionConst.PERFORMANCE_TAB)]
    public class CurrencyPerformanceController : ControllerBaseCustom
    {
        private readonly ICurrencyPerformanceService _currencyService;
        private readonly ITranslationService _translationService;

        public CurrencyPerformanceController(
            IContextService contextService,
            ICurrencyPerformanceService currencyService,
            ITranslationService translationService
            )
            : base(contextService)
        {
            _currencyService = currencyService;
            _translationService = translationService;
        }

        [HttpPost]
        [Route("api/performance/currency/distribution")]
        [AutoBindingMetaModel(bindSiteName: true)]
        public async Task<JsonResult> GetDistribution([FromBody]CurrenyPerformanceFilterMetaModel filter)
        {
            var result = await _currencyService.GetDistributionWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/currency/diffences")]
        [AutoBindingMetaModel(bindSiteName: true)]
        public async Task<JsonResult> GetDiffences([FromBody]CurrenyPerformanceFilterMetaModel filter)
        {
            var result = await _currencyService.GetMeasurementDifferenceWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/currency/period-growth")]
        [AutoBindingMetaModel(bindSiteName: true)]
        public async Task<JsonResult> GetPeriodGrowth([FromBody]CurrenyPerformanceFilterMetaModel filter)
        {
            var result = await _currencyService.GetPeriodGrowthWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/currency/month-growth-all")]
        [AutoBindingMetaModel(bindSiteName: true)]
        public async Task<JsonResult> GetAllMonthGrowth([FromBody]CurrenyPerformanceFilterMetaModel filter)
        {
            var result = await _currencyService.GetAllDetailMonthGrowthWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/currency")]
        [AutoBindingMetaModel(bindSiteName: true)]
        public async Task<JsonResult> GetCombineData([FromBody]CurrenyPerformanceFilterMetaModel filter)
        {
            await BindTopDistributionsToFilterMetaModel(filter);
            _translationService.Translate(filter.TopDistributions, TranslationConst.CurrencyType);

            var isMargin = filter.Measurement.Id == MeasurementConst.MARGIN;

            var (measurementDifferenceData, periodGrowthData, distributionData, overallMarginData) = await TaskExtension.WhenAllAsync(
                              _currencyService.GetMeasurementDifferenceWithComparison(filter),
                              _currencyService.GetPeriodGrowthWithComparison(filter),
                              !isMargin ? _currencyService.GetDistributionWithComparison(filter) : Task.FromResult<IEnumerable<DistributionVM>>(null),
                              isMargin ? _currencyService.GetOverallMeasurement(filter) : Task.FromResult<IEnumerable<OverallMeasurementVM>>(null));

            _translationService.Translate(distributionData, TranslationConst.NormalType);
            _translationService.Translate(overallMarginData, TranslationConst.NormalType);

            return ToResult(new
            {
                BaseCurrency = filter.BaseCurrencyName,
                MeasurementId = filter.Measurement.Id,
                filter.CurrentPeriod,
                filter.PreviousPeriod,
                filter.TopDistributions,
                distributionData,
                overallMarginData,
                measurementDifferenceData,
                GrowthData = new
                {
                    PeriodGrowth = periodGrowthData,
                    MonthGrowth = new { }
                }
            });
        }

        private async Task BindTopDistributionsToFilterMetaModel(PerformanceFilterMetaModel filter)
        {
            var topDistributions = await _currencyService.GetTopDistributions(filter);
            filter.TopDistributions = topDistributions.Select(_ => new DistributionMetaModel
            {
                DisplayId = _.topDistributionId,
                DisplayName = _.topDistributionName,
                CurrentPeriodDistributions = new List<IdentityMetaModel> { new IdentityMetaModel { Id = _.topDistributionId, Name = _.topDistributionName } },
                PreviousPeriodDistributions = new List<IdentityMetaModel> { new IdentityMetaModel { Id = _.topDistributionId, Name = _.topDistributionName } }
            }).ToList();

            var otherDistribution = filter.TopDistributions.FirstOrDefault(_ => _.DisplayId == ViewModelMapper.DISTRIBUTION_OTHERS_ID);
            if (otherDistribution != null)
            {
                var dicTopDistributions = topDistributions.ToDictionary(_ => _.topDistributionId, _ => _);

                var (prevPeriodData, currentPeriodData) = await _currencyService.GetDistributionRawData(filter);

                var prevPeriodOthersData = prevPeriodData.Where(_ => !_.IsSum && !dicTopDistributions.ContainsKey(_.Id));
                var currentPeriodOthersData = currentPeriodData.Where(_ => !_.IsSum && !dicTopDistributions.ContainsKey(_.Id));

                otherDistribution.PreviousPeriodDistributions = prevPeriodOthersData.Any() ? prevPeriodOthersData.Select(_ => new IdentityMetaModel { Id = _.Id, Name = _.Name }) : null;
                otherDistribution.CurrentPeriodDistributions = currentPeriodOthersData.Select(_ => new IdentityMetaModel { Id = _.Id, Name = _.Name });
            }
        }
    }
}