using System.Threading.Tasks;
using Fanex.Athena.AnalyticsAPI.Common.Controllers;
using Fanex.Athena.AnalyticsAPI.Common.Helpers;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Core.Attributes;
using Fanex.Athena.AnalyticsAPI.Core.Constants;
using Fanex.Athena.AnalyticsAPI.Summary.Consts;
using Fanex.Athena.AnalyticsAPI.Summary.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Summary.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Athena.AnalyticsAPI.Summary.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [BearerAuthorize(Roles.SuperAdmin, Roles.SummaryCustomerBalance)]
    public class CustomerBalanceController : ControllerBaseCustom
    {
        private readonly ISingleValueService _singleValueService;

        public CustomerBalanceController(IContextService contextService, ISingleValueService singleValueService)
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
            var result = await _singleValueService.GetCustomerBalance(metaModel);
            return result;
        }
    }
}