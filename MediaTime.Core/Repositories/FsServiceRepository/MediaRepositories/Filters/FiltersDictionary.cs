using System;
using System.Collections.Generic;
using System.Linq;

namespace MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.Filters
{
    public class FiltersDictionary : Dictionary<string, object>
    {
        public Dictionary<string, string[]> GetFiltersNames()
        {
            return FiltersDictionaryExtensions.GetFiltersNames(this);
        }
        public Dictionary<string, Enum[]> GetEnums()
        {
            return FiltersDictionaryExtensions.GetEnums(this);
        }
        public Dictionary<string, object[]> GetFilters()
        {
            return FiltersDictionaryExtensions.GetFilters(this);
        }
    }

    public static class FiltersDictionaryExtensions
    {
        public static Dictionary<string, string[]> GetFiltersNames(this FiltersDictionary filters)
        {
            return filters.Where(pair => pair.Value != null)
                .ToDictionary(pair => pair.Key,
                    pair => pair.Value is Enum ? Enum.GetValues(pair.Value.GetType()).ToStringArray() : null);
            //return
            //    GetFilters(filters)
            //        .Select(
            //            filter =>
            //                new KeyValuePair<string, List<string>>(filter.Key, filter.Value.ToStringList())).ToArray();
        }

        //public static KeyValuePair<string, Array>[] GetFilters(this FiltersDictionary filters)
        //{
        //    return (from filter in filters
        //            let enumVal = filter.Value
        //            where enumVal != null
        //        select
        //            new KeyValuePair<string, Array>(filter.Key, (enumVal != null) ? Enum.GetValues(enumVal.GetType()) : null))
        //        .ToArray();
        //}
        public static Dictionary<string, object[]> GetFilters(this FiltersDictionary filters)
        {
        //    return (from filter in filters
        //            let enumVal = filter.Value
        //            where enumVal != null
        //            select 
        //                new KeyValuePair<string, Enum[]>(filter.Key, (enumVal != null) ? Enum.GetValues(enumVal.GetType()).ToArrayOf<Enum>() : null))
        //        .ToArray();
            return filters.Where(pair => !string.IsNullOrEmpty(pair.Key))
                .ToDictionary(pair => pair.Key,
                    pair =>
                        pair.Value is Enum
                            ? Enum.GetValues(pair.Value.GetType()).ToArrayOf<object>()
                            : new[] {pair.Value});

        }

        public static List<string> ToStringList(this Array array)
        {
            var list = new List<string>();
            var enumerator = array.GetEnumerator();
            while (enumerator.MoveNext())
                if (enumerator.Current != null)
                    list.Add(enumerator.Current.ToString());
            return list;
        }
        public static string[] ToStringArray(this Array array)
        {
            return array.OfType<object>().Where(o => o != null).Select(o => o.ToString()).ToArray();
        }

        public static List<T> ToListOf<T>(this Array array)
        {
            var list = new List<T>();
            var enumerator = array.GetEnumerator();
            while (enumerator.MoveNext())
                if (enumerator.Current is T)
                    list.Add((T)enumerator.Current);
            return list;
        }

        public static T[] ToArrayOf<T>(this Array array)
        {
            return array.OfType<T>().ToArray();
        }

        public static Dictionary<string, Enum[]> GetEnums(this FiltersDictionary filters)
        {
            return filters.Where(pair => !string.IsNullOrEmpty(pair.Key))
                .ToDictionary(pair => pair.Key,
                    pair =>
                        pair.Value is Enum
                            ? Enum.GetValues(pair.Value.GetType()).ToArrayOf<Enum>()
                            : null);
        }
    }
}