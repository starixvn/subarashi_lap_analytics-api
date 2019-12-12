using Fanex.Athena.AnalyticsAPI.Common.Attributes;
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
    [OneAuthorization(Roles = PermissionConst.PerformanceTab)]
    public class BettypePerformanceController : ControllerBaseCustom
    {
        private readonly IBettypePerformanceService _bettypeService;
        private readonly ITranslationService _translationService;

        public BettypePerformanceController(
            IContextService contextService,
            IBettypePerformanceService bettypeService,
            ITranslationService translationService
            )
            : base(contextService)
        {
            _bettypeService = bettypeService;
            _translationService = translationService;
        }

        [HttpPost]
        [Route("api/performance/bettype/distribution")]
        [AutoBindMetaModel]
        public async Task<JsonResult> GetDistribution([FromBody]BettypePerformanceFilterMetaModel filter)
        {
            var result = await _bettypeService.GetDistributionWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/bettype/diffences")]
        [AutoBindMetaModel]
        public async Task<JsonResult> GetDiffences([FromBody]BettypePerformanceFilterMetaModel filter)
        {
            var result = await _bettypeService.GetMeasurementDifferenceWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/bettype/period-growth")]
        [AutoBindMetaModel]
        public async Task<JsonResult> GetPeriodGrowth([FromBody]BettypePerformanceFilterMetaModel filter)
        {
            var result = await _bettypeService.GetPeriodGrowthWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/bettype/month-growth-all")]
        [AutoBindMetaModel]
        public async Task<JsonResult> GetAllMonthGrowth([FromBody]BettypePerformanceFilterMetaModel filter)
        {
            var result = await _bettypeService.GetAllDetailMonthGrowthWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/bettype")]
        [AutoBindMetaModel]
        public async Task<JsonResult> GetCombineData([FromBody]BettypePerformanceFilterMetaModel filter)
        {
            await BindTopDistributionsToFilterMetaModel(filter);

            _translationService.Translate(filter.TopDistributions, TranslationConst.BettypeType);

            var isMargin = filter.Measurement.Id == MeasurementConst.MARGIN;

            var (measurementDifferenceData, periodGrowthData, distributionData, overallMarginData) = await TaskExtension.WhenAllAsync(
                              _bettypeService.GetMeasurementDifferenceWithComparison(filter),
                              _bettypeService.GetPeriodGrowthWithComparison(filter),
                              !isMargin ? _bettypeService.GetDistributionWithComparison(filter) : Task.FromResult<IEnumerable<DistributionVM>>(null),
                              isMargin ? _bettypeService.GetOverallMeasurement(filter) : Task.FromResult<IEnumerable<OverallMeasurementVM>>(null));

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
            var topDistributions = await _bettypeService.GetTopDistributions(filter, 10);
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

                var (prevPeriodData, currentPeriodData) = await _bettypeService.GetDistributionRawData(filter);
                var prevPeriodOthersData = prevPeriodData.Where(_ => !_.IsSum && !dicTopDistributions.ContainsKey(_.Id));
                var currentPeriodOthersData = currentPeriodData.Where(_ => !_.IsSum && !dicTopDistributions.ContainsKey(_.Id));

                otherDistribution.PreviousPeriodDistributions = prevPeriodOthersData.Any() ? prevPeriodOthersData.Select(_ => new IdentityMetaModel { Id = _.Id, Name = _.Name }) : null;
                otherDistribution.CurrentPeriodDistributions = currentPeriodOthersData.Select(_ => new IdentityMetaModel { Id = _.Id, Name = _.Name });
            }
        }
    }
}