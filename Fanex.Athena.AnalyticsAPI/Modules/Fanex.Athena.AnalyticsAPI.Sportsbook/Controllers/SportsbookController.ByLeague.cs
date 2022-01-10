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