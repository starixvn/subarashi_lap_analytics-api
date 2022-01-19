using Fanex.Athena.AnalyticsAPI.Common.Controllers;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Core.Attributes;
using Fanex.Athena.AnalyticsAPI.Core.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Athena.AnalyticsAPI.Sportsbook.Controllers
{
    [ApiController]
    [BearerAuthorize(Roles.SuperAdmin, Roles.SportsbookTab)]
    public partial class SportsbookController : ControllerBaseCustom
    {
        private readonly ISender _sender;

        public SportsbookController(IContextService contextService, ISender sender)
            : base(contextService)
        {
            _sender = sender;
        }
    }
}