namespace Fanex.Athena.AnalyticsAPI.Common.Services
{
    public interface ILangResourceService
    {
        string GetName(string type, string id, string langCode, string defaultValue);
    }
}