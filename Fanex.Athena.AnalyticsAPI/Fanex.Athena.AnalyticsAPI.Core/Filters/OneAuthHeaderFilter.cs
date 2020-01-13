using Fanex.Athena.AnalyticsAPI.Core.Constants;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Core.Filters
{
    public class OneAuthHeaderFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<IParameter>();
            }

            operation.Parameters.Add(new NonBodyParameter
            {
                Name = AuthConst.AUTH_KEY,
                In = "header",
                Type = "string"
            });
        }
    }
}