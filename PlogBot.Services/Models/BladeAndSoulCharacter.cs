using PlogBot.Data.Enums;
using PlogBot.Services.WebModels;

namespace PlogBot.Services.Models
{
    public class BladeAndSoulCharacter
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public int HongmoonLevel { get; set; }
        public string Clan { get; set; }
        public CharacterClass Class { get; set; }
        public AbilitiesResult AbilitiesResult { get; set; }
    }
}
