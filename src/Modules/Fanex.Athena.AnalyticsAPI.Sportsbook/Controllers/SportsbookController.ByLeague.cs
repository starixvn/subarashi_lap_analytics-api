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
        [Route("api/sportsbook/league/soccer")]
        [ProducesResponseType(typeof(ByLeagueResponse), (int)HttpStatusCode.OK)]
        [AutoBindingMetaModel(bindSiteName: true)]
        public async Task<JsonResult> ByLeagueSoccer([FromBody] ByLeagueSoccerRequest request)
        {
            var result = await _sender.Send(request);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/sportsbook/league/basketball")]
        [ProducesResponseType(typeof(ByLeagueResponse), (int)HttpStatusCode.OK)]
        [AutoBindingMetaModel(bindSiteName: true)]
        public async Task<JsonResult> ByLeagueBasketball([FromBody] ByLeagueBasketballRequest request)
        {
            var result = await _sender.Send(request);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/sportsbook/league/cricket")]
        [ProducesResponseType(typeof(ByLeagueResponse), (int)HttpStatusCode.OK)]
        [AutoBindingMetaModel(bindSiteName: true)]
        public async Task<JsonResult> ByLeagueCricket([FromBody] ByLeagueCricketRequest request)
        {
            var result = await _sender.Send(request);
            return ToResult(result);
        }
    }
}