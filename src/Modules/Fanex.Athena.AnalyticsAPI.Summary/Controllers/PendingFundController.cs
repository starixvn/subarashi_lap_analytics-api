namespace Fanex.Athena.AnalyticsAPI.Summary.Controllers
{
    using System.Threading.Tasks;
    using Common.Controllers;
    using Common.Services;
    using Consts;
    using Core.Attributes;
    using Core.Constants;
    using Microsoft.AspNetCore.Mvc;
    using Models.MetaModels;
    using Services;

    [ApiController]
    [Route("api/[controller]/[action]")]
    [BearerAuthorize(Roles.SuperAdmin, Roles.SummaryPendingFunds)]
    public class PendingFundController : ControllerBaseCustom
    {
        private readonly ISingleValueService _singleValueService;

        public PendingFundController(IContextService contextService, ISingleValueService singleValueService)
            : base(contextService)
        {
            _singleValueService = singleValueService;
        }

        [HttpGet]
        [AutoBindingMetaModel(bindAgent: true)]
        [ApiRequestValidation(ResponseContent = ResponseContentConst.DefaultSingleValue)]
        public async Task<decimal> GetData([FromQuery] BaseSummaryMetaModel metaModel)
        {
            var result = await _singleValueService.GetPendingFunds(metaModel);
            return result;
        }
    }
}