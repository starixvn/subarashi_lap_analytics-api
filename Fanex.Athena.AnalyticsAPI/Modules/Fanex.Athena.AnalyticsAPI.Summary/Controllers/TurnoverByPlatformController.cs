using System;
using System.Threading.Tasks;
using Fanex.Athena.AnalyticsAPI.Common.Controllers;
using Fanex.Athena.AnalyticsAPI.Common.Helpers;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Core.Attributes;
using Fanex.Athena.AnalyticsAPI.Core.Constants;
using Fanex.Athena.AnalyticsAPI.Core.Services;
using Fanex.Athena.AnalyticsAPI.Summary.Consts;
using Fanex.Athena.AnalyticsAPI.Summary.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Summary.Models.ViewModels;
using Fanex.Athena.AnalyticsAPI.Summary.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Athena.AnalyticsAPI.Summary.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [BearerAuthorize(Roles.SuperAdmin, Roles.SummaryTurnoverByPlatform)]
    public class TurnoverByPlatformController : ControllerBaseCustom
    {
        private readonly ILicenseeDataPlatformService _licenseeDataPlatformService;
        private readonly IAuthInfoService _authInfoService;

        public TurnoverByPlatformController(
            IContextService contextService,
            ILicenseeDataPlatformService licenseeDataPlatformService,
            IAuthInfoService authInfoService
            )
            : base(contextService)
        {
            _licenseeDataPlatformService = licenseeDataPlatformService;
            _authInfoService = authInfoService;
        }

        [HttpGet]
        [AutoBindingMetaModel(bindAgent: true, bindProductList: true)]
        [ApiRequestValidation(ResponseContent = ResponseContentConst.DefaultWidgetResult)]
        public async Task<WidgetByPlatformViewModel> GetData([FromQuery] BaseSummaryMetaModel metaModel)
        {
            var now = DateTimeHelper.DatabaseServerNow.Date;
            metaModel.FromDate = new DateTime(now.Year, now.Month, 1);
            metaModel.ToDate = now;
            metaModel.SpecialPlatform = _authInfoService.SpecialPlatformType;

            var result = await _licenseeDataPlatformService.GetTurnover(metaModel);
            return result;
        }
    }
}