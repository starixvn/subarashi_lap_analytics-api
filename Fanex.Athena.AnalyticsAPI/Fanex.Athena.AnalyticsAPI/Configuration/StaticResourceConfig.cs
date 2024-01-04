using System.Linq;

namespace Fanex.Athena.AnalyticsAPI.Configuration
{
    public class StaticResourceConfig
    {
        private string[] _scripts;
        private string[] _styles;
        public string BundleVersion { get; set; }

        public string[] Scripts
        {
            get { return _scripts; }
            set { _scripts = value.Select(x => string.Format(x + "?v={0}", BundleVersion)).ToArray(); }
        }

        public string[] Styles
        {
            get { return _styles; }
            set { _styles = value.Select(x => string.Format(x + "?v={0}", BundleVersion)).ToArray(); }
        }
    }
}