using Fanex.Athena.AnalyticsAPI.Common.Controllers;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Core.Attributes;
using Fanex.Athena.AnalyticsAPI.Core.Constants;
using Fanex.Athena.AnalyticsAPI.Summary.Consts;
using Fanex.Athena.AnalyticsAPI.Summary.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Summary.Models.ViewModels;
using Fanex.Athena.AnalyticsAPI.Summary.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Summary.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [BearerAuthorize(Roles.SuperAdmin, Roles.SummaryLicenseeWLByPlatform)]
    public class LicenseeWinLossByPlatformController : ControllerBaseCustom
    {
        private readonly ILicenseeDataPlatformService _licenseeDataPlatformService;

        public LicenseeWinLossByPlatformController(
            IContextService contextService,
            ILicenseeDataPlatformService licenseeDataPlatformService)
            : base(contextService)
        {
            _licenseeDataPlatformService = licenseeDataPlatformService;
        }

        [HttpGet]
        [AutoBindingMetaModel(bindAgent: true, bindProductList: true)]
        [ApiRequestValidation(ResponseContent = ResponseContentConst.DefaultWidgetResult)]
        public async Task<WidgetByPlatformViewModel> GetData([FromQuery]BaseSummaryMetaModel metaModel)
        {
            var now = DateTime.Now.Date;
            metaModel.FromDate = new DateTime(now.Year, now.Month, 1);
            metaModel.ToDate = now;

            var result = await _licenseeDataPlatformService.GetWinLoss(metaModel);
            return result;
        }
    }
}