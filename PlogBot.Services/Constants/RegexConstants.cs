namespace PlogBot.Services.Constants
{
    public class RegexConstants
    {
        public const string MentionRegex = @"^(<@)!?\d+(>)$";
        // TODO: Fix Time Regex
        public const string TimeRegex = @"^\d{1-2}:\d{2}(AM|PM)?";
    }
}
