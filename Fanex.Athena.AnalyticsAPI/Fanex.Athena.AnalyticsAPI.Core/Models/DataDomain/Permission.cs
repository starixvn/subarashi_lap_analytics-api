using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Core.Models
{
    public class Permission
    {
        public string FunctionKey { get; set; }

        public IEnumerable<string> SubFunctionKeys { get; set; }
    }
}