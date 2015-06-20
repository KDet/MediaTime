using System.Collections.Generic;
using System.Linq;
using MediaTime.Core.Common;

namespace MediaTime.Core.Extensions
{
    public static class CustomListExtensions
    {
        public static IEnumerable<ObservableKeyValuePair<TKey, TValue>> ToObservable<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> keyValuePairs)
        {
            return
                keyValuePairs.Select(
                    keyValuePair => new ObservableKeyValuePair<TKey, TValue>(keyValuePair.Key, keyValuePair.Value));
        }
    }
}
