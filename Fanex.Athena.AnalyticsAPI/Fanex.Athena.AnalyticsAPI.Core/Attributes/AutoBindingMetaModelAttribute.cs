using Fanex.Athena.AnalyticsAPI.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class AutoBindingMetaModelAttribute : TypeFilterAttribute
    {
        public AutoBindingMetaModelAttribute(bool bindSiteName = false, bool bindAgent = false, bool bindProductList = false)
            : base(typeof(AutoBindingMetaModelImplAttribute))
        {
            Arguments = new object[] { bindSiteName, bindAgent, bindProductList };
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class AutoBindingMetaModelImplAttribute : ActionFilterAttribute
    {
        private readonly IAuthInfoService _authInfoService;
        private readonly IMetaModelBindingService _bindingService;

        private readonly bool _bindSiteName;
        private readonly bool _bindAgent;
        private readonly bool _bindProductList;

        public AutoBindingMetaModelImplAttribute(IAuthInfoService authInfoService, IMetaModelBindingService bindingService,
            bool bindSiteName,
            bool bindAgent,
            bool bindProductList)
        {
            _authInfoService = authInfoService;
            _bindingService = bindingService;

            _bindSiteName = bindSiteName;
            _bindAgent = bindAgent;
            _bindProductList = bindProductList;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (_bindSiteName)
            {
                _bindingService.BindSiteName(context.ActionArguments, _authInfoService.SiteName);
            }

            if (_bindAgent)
            {
                _bindingService.BindAgentXml(context.ActionArguments, _authInfoService.CurrencyAgentList);
            }

            if(_bindProductList)
            {
                _bindingService.BindProductCodeList(context.ActionArguments, _authInfoService.OfferedSubProductList);
            }

            await next();
        }
    }
}