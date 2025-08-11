namespace Fanex.Athena.AnalyticsAPI.Sportsbook.Controllers
{
    using System.Net;
    using System.Threading.Tasks;
    using Core.Attributes;
    using Microsoft.AspNetCore.Mvc;
    using Models;

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