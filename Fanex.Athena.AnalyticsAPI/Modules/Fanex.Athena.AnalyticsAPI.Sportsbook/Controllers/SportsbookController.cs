using Fanex.Athena.AnalyticsAPI.Common.Controllers;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Core.Constants;
using Fanex.Athena.AnalyticsAPI.Core.ONEBookAuth;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Athena.AnalyticsAPI.Sportsbook.Controllers
{
    [ApiController]
    [ONEBookAuthorize(Permissions = PermissionConst.SPORTSBOOK_TAB)]
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