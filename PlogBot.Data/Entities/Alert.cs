using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlogBot.Data.Entities
{
    public class Alert
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public ulong DiscordUserId { get; set; }
        public ulong ChannelId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Day { get; set; }
        public int Time { get; set; }
        public string Roles { get; set; }
        public DateTime? LastProcessed { get; set; }
    }
}