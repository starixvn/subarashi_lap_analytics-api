using System;
using System.Collections.Generic;
using System.Text;

namespace Fanex.Athena.AnalyticsAPI.Common.Utility
{
    public interface ICustomMemoryCacheService
    {
        void Clear();

        bool Contains(string key);

        T Get<T>(string key);

        T Get<T>(string key, Func<T> action, int seconds = 900) where T : class;

        void Set<T>(string key, T value, int seconds = 900);
    }
}