using Fanex.Athena.AnalyticsAPI.Core.Attributes;
using Fanex.Athena.AnalyticsAPI.Sportsbook.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Sportsbook.Controllers
{
    public partial class SportsbookController
    {
        [HttpPost]
        [Route("api/sportsbook/platform")]
        [ProducesResponseType(typeof(ByCurrencyResponse), (int)HttpStatusCode.OK)]
        [AutoBindingMetaModel(bindSiteName: true)]
        public async Task<JsonResult> ByPlatform([FromBody]ByPlatformRequest request)
        {
            var result = await _sender.Send(request);
            return ToResult(result);
        }
    }
}