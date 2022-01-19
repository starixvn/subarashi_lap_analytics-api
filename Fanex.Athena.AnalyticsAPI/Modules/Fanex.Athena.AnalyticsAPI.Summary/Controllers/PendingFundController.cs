using Fanex.Athena.AnalyticsAPI.Common.Controllers;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Core.Attributes;
using Fanex.Athena.AnalyticsAPI.Core.Constants;
using Fanex.Athena.AnalyticsAPI.Summary.Consts;
using Fanex.Athena.AnalyticsAPI.Summary.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Summary.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Summary.Controllers
{
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
        public async Task<decimal> GetData([FromQuery]BaseSummaryMetaModel metaModel)
        {
            var result = await _singleValueService.GetPendingFunds(metaModel);
            return result;
        }
    }
}