using Fanex.Athena.AnalyticsAPI.Common.Controllers;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Core.Attributes;
using Fanex.Athena.AnalyticsAPI.Core.Constants;
using Fanex.Athena.AnalyticsAPI.Summary.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Summary.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Summary.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [ONEBookAuthorization(Permissions = PermissionConst.SUMMARY_TODAY_DEPOSIT)]
    public class TodayDepositController : ControllerBaseCustom
    {
        private readonly ISingleValueService _singleValueService;

        public TodayDepositController(IContextService contextService, ISingleValueService singleValueService)
            : base(contextService)
        {
            _singleValueService = singleValueService;
        }

        [HttpGet]
        [AutoBindingMetaModel(bindAgent: true)]
        [ApiRequestValidation]
        public async Task<decimal> GetData([FromQuery]SummaryBaseMetaModel metaModel)
        {
            metaModel.FromDate = DateTime.Now.Date;
            var result = await _singleValueService.GetTodayDeposit(metaModel);
            return result;
        }
    }
}