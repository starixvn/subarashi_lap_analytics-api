namespace Fanex.Athena.AnalyticsAPI.Common.Services
{
    public interface ITranslationService
    {
        string GetDisplayBetTypeName(string betTypeId, string langCode, string defaultValue);

        string GetProductName(string productId, string langCode, string defaultValue);

        string GetPlatformName(string platformId, string langCode, string defaultValue);
    }
}