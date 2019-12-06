using Fanex.Athena.AnalyticsAPI.Common.Constants;
using Fanex.Athena.AnalyticsAPI.Common.Resources.Lang;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Common.UnitTest.Services.TranslationSerivce
{
    public class TranslationService_Test
    {
        [Fact]
        public void Test()
        {
            var input = "asdsa (Xyz)";
            var parttern = @"\(\w+\)$";
            var result = Regex.Split(input, parttern);
            var x = Regex.Replace(input, result[0], "replace");
        }
    }
}