using System;

namespace PlogBot.Services.Models
{
    public class ClanLogCsvEntry
    {
        public string CharacterName { get; set; }
        public bool Active { get; set; }
        public int Level { get; set; }
        public int HmLevel { get; set; }
        public int BatchNumber { get; set; }
        public DateTime Recorded { get; set; }
        public string Weapon { get; set; }
        public string Gems { get; set; }
        public string Ring { get; set; }
        public string Earring { get; set; }
        public string Necklace { get; set; }
        public string Belt { get; set; }
        public string Bracelet { get; set; }
        public string Gloves { get; set; }
        public string Pet { get; set; }
        public string Soul { get; set; }
        public string Heart { get; set; }
        public string SoulBadge { get; set; }
        public string MysticBadge { get; set; }
    }
}
