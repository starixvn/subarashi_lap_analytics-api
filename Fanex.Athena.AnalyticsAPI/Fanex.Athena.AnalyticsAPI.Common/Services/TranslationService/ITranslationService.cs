using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Common.Services
{
    public interface ITranslationService
    {
        void Translate<T>(IEnumerable<T> originalItems, bool isHideSaba, string translationType = null)
            where T : class;
    }
}