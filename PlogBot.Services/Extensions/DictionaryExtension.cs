using System.Collections.Generic;

namespace PlogBot.Services.Extensions
{
    public static class DictionaryExtension
    {
        public static TValue GetValueOrInsert<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue toAdd)
        {
            if (dict.ContainsKey(key))
            {
                return dict[key];
            }
            else
            {
                dict.Add(key, toAdd);
                return toAdd;
            }
        }

    }
}
