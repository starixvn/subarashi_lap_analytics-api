namespace Fanex.Athena.AnalyticsAPI.Core.Models
{
    using System.Collections.Generic;

    public class Permission
    {
        public string FunctionKey { get; set; }

        public IEnumerable<string> SubFunctionKeys { get; set; }
    }
}