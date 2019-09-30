using System;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.Athena.AnalyticsAPI.Common.Extensions
{
    public static class CollectionExtension
    {
        public static IEnumerable<TOutput> GroupTo<TInput, TKey, TOutput>(this IEnumerable<TInput> collection, Func<TInput, TKey> keySelector, Func<IGrouping<TKey, TInput>, TOutput> mapper)
        {
            if (collection == null)
            {
                return Enumerable.Empty<TOutput>();
            }

            return collection.GroupBy(_ => keySelector(_))
                .Select(_ => mapper(_));
        }
    }
}