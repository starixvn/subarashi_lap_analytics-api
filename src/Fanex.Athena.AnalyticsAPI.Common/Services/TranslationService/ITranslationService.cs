namespace Fanex.Athena.AnalyticsAPI.Common.Services
{
    using System.Collections.Generic;

    public interface ITranslationService
    {
        void Translate<T>(IEnumerable<T> originalItems, bool isHideSaba, string translationType = null)
            where T : class;
    }
}