namespace PlogBot.Services.Extensions
{
    public static class StringExtensions
    {
        public static ulong StripMentionExtras(this string s)
        {
            return ulong.Parse(s.Replace("<@", "").Replace(">", ""));
        }
    }
}
