namespace Fanex.Athena.AnalyticsAPI.Core.Attributes
{
    using System;
    using System.Linq;
    using System.Net;
    using Constants;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Models.MetaModels;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class ApiRequestValidationAttribute : ActionFilterAttribute
    {
        public string ResponseContent { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            ValidateRequest(context);
        }

        private void ValidateRequest(ActionExecutingContext context)
        {
            var metaModel = context.ActionArguments.FirstOrDefault(x => x.Value is IValidationMetaModel);
            if (metaModel.Value is IValidationMetaModel validationMetaModel
                && !IsValid(validationMetaModel))
            {
                context.Result = GetResult(ResponseContent);
            }
        }

        private static bool IsValid(IValidationMetaModel model)
        {
            return model != null && model.IsValid;
        }

        private static IActionResult GetResult(string responseContent)
        {
            if (string.IsNullOrEmpty(responseContent))
            {
                return RequestResultConst.BadResult;
            }

            return new ContentResult
            {
                StatusCode = (int)HttpStatusCode.OK,
                Content = responseContent
            };
        }
    }
}