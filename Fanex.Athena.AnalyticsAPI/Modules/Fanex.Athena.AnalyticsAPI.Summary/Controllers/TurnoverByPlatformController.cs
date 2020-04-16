using Fanex.Athena.AnalyticsAPI.Common.Controllers;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Core.Attributes;
using Fanex.Athena.AnalyticsAPI.Core.Constants;
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
    [ONEBookAuthorization(Permissions = PermissionConst.SUMMARY_TURNOVER_BY_PLATFORM)]
    public class TurnoverByPlatformController : ControllerBaseCustom
    {
        private readonly ILicenseeDataPlatformService _licenseeDataPlatformService;

        public TurnoverByPlatformController(IContextService contextService, ILicenseeDataPlatformService licenseeDataPlatformService)
            : base(contextService)
        {
            _licenseeDataPlatformService = licenseeDataPlatformService;
        }

        [HttpGet]
        [ApiRequestValidation]
        [AutoBindingMetaModel(bindAgent: true)]
        public async Task<WidgetByPlatformViewModel> GetData([FromQuery]SummaryBaseMetaModel metaModel)
        {
            var now = DateTime.Now.Date;
            metaModel.FromDate = new DateTime(now.Year, now.Month, 1);
            metaModel.ToDate = now;

            var result = await _licenseeDataPlatformService.GetTurnover(metaModel);
            return result;
        }
    }
}