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
        [Route("api/sportsbook/bettype/soccer")]
        [ProducesResponseType(typeof(ByBettypeResponse), (int)HttpStatusCode.OK)]
        [AutoBindingMetaModel(bindSiteName: true)]
        public async Task<JsonResult> ByBettypeSoccer([FromBody] ByBettypeSoccerRequest request)
        {
            var result = await _sender.Send(request);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/sportsbook/bettype/basketball")]
        [ProducesResponseType(typeof(ByBettypeResponse), (int)HttpStatusCode.OK)]
        [AutoBindingMetaModel(bindSiteName: true)]
        public async Task<JsonResult> ByBettypeBaseketball([FromBody] ByBettypeBasketballRequest request)
        {
            var result = await _sender.Send(request);
            return ToResult(result);
        }
    }
}