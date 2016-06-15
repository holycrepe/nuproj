namespace NuProj.Tasks.Extensions.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class DictExtensions
    {
        static IDictionary<TKey, TValue> ConcatInternal<TKey, TValue>(this IDictionary<TKey, TValue> original, bool overwrite, bool modifyOriginal, params IDictionary<TKey, TValue>[] newDicts)
        {
            IDictionary<TKey, TValue> merged = modifyOriginal
                                                  ? original
                                                  : new Dictionary<TKey, TValue>();
            var startingEntries = (modifyOriginal)
                ? original.ToArray()
                : new KeyValuePair<TKey, TValue>[0];

            foreach (var kvp in startingEntries.Concat(newDicts.SelectMany(x => x)))
                if (!merged.ContainsKey(kvp.Key) || overwrite)
                    merged[kvp.Key] = kvp.Value;

            return merged;
        }

        /// <summary>
        /// In-Place Merges all additional dictionaries (from left-to-right) with the original dictionary.
        /// NOTE: This method will OVERWRITE any existing keys
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="original"></param>
        /// <param name="newDicts"></param>
        public static IDictionary<TKey, TValue> Update<TKey, TValue>(
            this IDictionary<TKey, TValue> original,
            params IDictionary<TKey, TValue>[] newDicts) => original.ConcatInternal(true, true, newDicts);

        /// <summary>
        /// In-Place Merges all additional dictionaries (from left-to-right) with the original dictionary.
        /// NOTE: This method will SKIP any existing keys
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="original"></param>
        /// <param name="newDicts"></param>
        public static IDictionary<TKey, TValue> SetDefaults<TKey, TValue>(
            this IDictionary<TKey, TValue> original,
            params IDictionary<TKey, TValue>[] newDicts) => original.ConcatInternal(true, true, newDicts);
    }
}
