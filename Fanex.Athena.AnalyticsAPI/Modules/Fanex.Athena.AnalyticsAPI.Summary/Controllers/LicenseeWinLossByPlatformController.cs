using Fanex.Athena.AnalyticsAPI.Common.Controllers;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Core.Attributes;
using Fanex.Athena.AnalyticsAPI.Core.Constants;
using Fanex.Athena.AnalyticsAPI.Core.ONEBookAuth;
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
    [ONEBookAuthorize(Permissions = PermissionConst.SUMMARY_LICENSEE_WINLOSS_BY_PLATFORM)]
    public class LicenseeWinLossByPlatformController : ControllerBaseCustom
    {
        private readonly ILicenseeDataPlatformService _licenseeDataPlatformService;
        private readonly ICommonService _commonService;

        public LicenseeWinLossByPlatformController(
            IContextService contextService,
            ILicenseeDataPlatformService licenseeDataPlatformService,
            ICommonService commonService)
            : base(contextService)
        {
            _licenseeDataPlatformService = licenseeDataPlatformService;
            _commonService = commonService;
        }

        [HttpGet]
        [AutoBindingMetaModel(bindSiteName: true, bindAgent: true)]
        [ApiRequestValidation(ResponseContent = ResponseContentConst.DefaultWidgetResult)]
        public async Task<WidgetByPlatformViewModel> GetData([FromQuery]BaseSummaryMetaModel metaModel)
        {
            var now = DateTime.Now.Date;
            var licenseeInfo = await _commonService.GetLicenseeInfo(metaModel.SiteName);
            metaModel.FromDate = new DateTime(now.Year, now.Month, 1);
            metaModel.ToDate = now;
            metaModel.ProductList = await _commonService.GetAllowedSubProductCodeList(licenseeInfo.Id);

            var result = await _licenseeDataPlatformService.GetWinLoss(metaModel);
            return result;
        }
    }
}