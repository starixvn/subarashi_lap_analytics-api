namespace Fanex.Athena.AnalyticsAPI.Summary.Controllers
{
    using System.Threading.Tasks;
    using Common.Controllers;
    using Common.Helpers;
    using Common.Services;
    using Consts;
    using Core.Attributes;
    using Core.Constants;
    using Microsoft.AspNetCore.Mvc;
    using Models.MetaModels;
    using Services;

    [ApiController]
    [Route("api/[controller]/[action]")]
    [BearerAuthorize(Roles.SuperAdmin, Roles.SummaryTodayWithdrawal)]
    public class TodayWithdrawalController : ControllerBaseCustom
    {
        private readonly ISingleValueService _singleValueService;

        public TodayWithdrawalController(IContextService contextService, ISingleValueService singleValueService)
            : base(contextService)
        {
            _singleValueService = singleValueService;
        }

        [HttpGet]
        [AutoBindingMetaModel(bindAgent: true)]
        [ApiRequestValidation(ResponseContent = ResponseContentConst.DefaultSingleValue)]
        public async Task<decimal> GetData([FromQuery] BaseSummaryMetaModel metaModel)
        {
            metaModel.FromDate = DateTimeHelper.DatabaseServerNow.Date;
            var result = await _singleValueService.GetTodayWithdrawal(metaModel);
            return result;
        }
    }
}