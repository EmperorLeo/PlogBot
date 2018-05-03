using System.Collections.Generic;

namespace PlogBot.Services.Extensions
{
    public static class DictionaryExtension
    {
        public static TValue GetValue<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key)
        {
            if (key != null && dict.ContainsKey(key))
            {
                return dict[key];
            }

            return default(TValue);
        }
    }
}
