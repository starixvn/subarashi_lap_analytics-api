namespace Fanex.Athena.AnalyticsAPI.Common.Controllers
{
    using Core.Models.DataDomain;
    using Microsoft.AspNetCore.Mvc;
    using Services;

    public class ControllerBaseCustom : ControllerBase
    {
        protected readonly IContextService _contextService;

        public ControllerBaseCustom(IContextService contextService)
        {
            _contextService = contextService ?? new ContextService();
        }

        [NonAction]
        protected JsonResult ToResult<T>(T data)
        {
            return new JsonResult(new ResponseDataVM<T>(data,
                _contextService.Errors == null || _contextService.Errors.Count == 0,
                _contextService.Message));
        }

        [NonAction]
        protected JsonResult ToResult()
        {
            return new JsonResult(new ResponseDataVM(
                _contextService.Errors == null || _contextService.Errors.Count == 0,
                _contextService.Message));
        }
    }
}