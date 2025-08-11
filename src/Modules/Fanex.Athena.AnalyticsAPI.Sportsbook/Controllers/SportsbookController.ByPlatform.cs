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
        [Route("api/sportsbook/platform")]
        [ProducesResponseType(typeof(ByCurrencyResponse), (int)HttpStatusCode.OK)]
        [AutoBindingMetaModel(bindSiteName: true)]
        public async Task<JsonResult> ByPlatform([FromBody] ByPlatformRequest request)
        {
            var result = await _sender.Send(request);
            return ToResult(result);
        }
    }
}