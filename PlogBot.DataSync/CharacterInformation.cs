using PlogBot.Data.Entities;
using PlogBot.Services.Models;
using PlogBot.Services.WebModels;

namespace PlogBot.DataSync
{
    public class CharacterInformation
    {
        public ClanMember Plog { get; set; }
        public BladeAndSoulCharacter Character { get; set; }
        public AbilitiesResult AbilitiesResult { get; set; }
        public BladeAndSoulItems Items { get; set; }
    }
}
