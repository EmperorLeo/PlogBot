using PlogBot.Data.Enums;

namespace PlogBot.Services.Models
{
    public class BladeAndSoulCharacter
    {
        public string AccountName { get; set; }
        public string Name { get; set; }
        public string ProfileImageUrl { get; set; }
        public int Level { get; set; }
        public int HongmoonLevel { get; set; }
        public string Clan { get; set; }
        public CharacterClass Class { get; set; }
    }
}
