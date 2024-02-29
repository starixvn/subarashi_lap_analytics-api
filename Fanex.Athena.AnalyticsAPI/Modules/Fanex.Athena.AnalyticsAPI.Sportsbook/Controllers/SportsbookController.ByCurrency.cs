using System.Net;
using System.Threading.Tasks;
using Fanex.Athena.AnalyticsAPI.Core.Attributes;
using Fanex.Athena.AnalyticsAPI.Sportsbook.Models;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Athena.AnalyticsAPI.Sportsbook.Controllers
{
    public partial class SportsbookController
    {
        [HttpPost]
        [Route("api/sportsbook/currency")]
        [ProducesResponseType(typeof(ByCurrencyResponse), (int)HttpStatusCode.OK)]
        [AutoBindingMetaModel(bindSiteName: true)]
        public async Task<JsonResult> ByCurrency([FromBody] ByCurrencyRequest request)
        {
            var result = await _sender.Send(request);
            return ToResult(result);
        }
    }
}