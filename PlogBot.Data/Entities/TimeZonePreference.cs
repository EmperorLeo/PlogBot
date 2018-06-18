using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlogBot.Data.Entities
{
    public class TimeZonePreference
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public ulong DiscordUserId { get; set; }
        public string TimeZone { get; set; }
        public bool HasDaylightSavings { get; set; }
    }
}