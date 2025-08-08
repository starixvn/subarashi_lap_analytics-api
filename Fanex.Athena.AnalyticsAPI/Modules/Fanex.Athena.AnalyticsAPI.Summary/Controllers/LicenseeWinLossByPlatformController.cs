namespace Fanex.Athena.AnalyticsAPI.Summary.Controllers
{
    using System.Threading.Tasks;
    using Common.Controllers;
    using Common.Helpers;
    using Common.Services;
    using Consts;
    using Core.Attributes;
    using Core.Constants;
    using Core.Services;
    using Microsoft.AspNetCore.Mvc;
    using Models.MetaModels;
    using Models.ViewModels;
    using Services;

    [ApiController]
    [Route("api/[controller]/[action]")]
    [BearerAuthorize(Roles.SuperAdmin, Roles.SummaryLicenseeWLByPlatform)]
    public class LicenseeWinLossByPlatformController : ControllerBaseCustom
    {
        private readonly ILicenseeDataPlatformService _licenseeDataPlatformService;
        private readonly IAuthInfoService _authInfoService;

        public LicenseeWinLossByPlatformController(
            IContextService contextService,
            ILicenseeDataPlatformService licenseeDataPlatformService,
            IAuthInfoService authInfoService)
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
            metaModel.FromDate = DateTimeHelper.CreateDateTime(now.Year, now.Month, 1);
            metaModel.ToDate = now;
            metaModel.SpecialPlatform = _authInfoService.SpecialPlatformType;

            var result = await _licenseeDataPlatformService.GetWinLoss(metaModel);
            return result;
        }
    }
}