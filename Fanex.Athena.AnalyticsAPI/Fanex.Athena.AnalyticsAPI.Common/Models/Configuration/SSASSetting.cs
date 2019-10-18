using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Common.Models.Configuration
{
    public class SSASSetting
    {
        public string BridgeHost { get; set; }
        public string AuthKey { get; set; }
        public int ConnectionExprieTime { get; set; }
        public IList<ModuleSetting> Modules { get; set; }
    }

    public class ModuleSetting
    {
        public string ModuleId { get; set; }
        public string ConnectionKey { get; set; }
    }
}