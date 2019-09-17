using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Common.Models.Configuration
{
    public class SSASSetting
    {
        public string BrigdeHost { get; set; }
        public IList<ModuleSetting> Modules { get; set; }
    }

    public class ModuleSetting
    {
        public string ModuleId { get; set; }
        public string ConnectionKey { get; set; }
    }
}