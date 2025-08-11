namespace Fanex.Athena.AnalyticsAPI.Sportsbook.Controllers
{
    using Common.Controllers;
    using Common.Services;
    using Core.Attributes;
    using Core.Constants;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

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