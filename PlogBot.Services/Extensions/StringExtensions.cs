﻿using System.Collections.Generic;
using System.Linq;

namespace PlogBot.Services.Extensions
{
    public static class StringExtensions
    {
        public static ulong StripMentionExtras(this string s)
        {
            return ulong.Parse(s.Replace("<@", "").Replace(">", "").Replace("!", "").Replace("&", ""));
        }

        public static int ParseTime(this string s)
        {
            var parts = s.Split(':');
            var hours = int.Parse(parts[0]);
            if (parts[1].EndsWith("PM") && hours != 12)
            {
                hours += 12;
            }
            else if (parts[1].EndsWith("AM") && hours == 12)
            {
                hours = 0;
            }
            var minutes = int.Parse(parts[1].Substring(0, 2));
            return hours * 60 + minutes;
        }

        public static string ConcatenateULongs(this List<ulong> list)
        {
            return string.Join(";", list.Select(x => x.ToString()));
        }

        public static List<ulong> GetULongs(this string s)
        {
            return string.IsNullOrEmpty(s) ? new List<ulong>() : s.Split(';').Select(x => ulong.Parse(x)).ToList();
        }
    }
}
