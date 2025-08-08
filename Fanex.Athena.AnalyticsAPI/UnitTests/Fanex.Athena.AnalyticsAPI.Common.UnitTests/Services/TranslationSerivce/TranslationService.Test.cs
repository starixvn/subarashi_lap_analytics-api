namespace Fanex.Athena.AnalyticsAPI.Common.UnitTests.Services.TranslationSerivce
{
    using System.Text.RegularExpressions;
    using Xunit;

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