using PlogBot.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlogBot.Data.Entities
{
    public class ClanMember
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string RealName { get; set; }

        public ulong? DiscordId { get; set; }

        public CharacterClass Class { get; set; }

        public Guid? MainId { get; set; }

        [ForeignKey("MainId")]
        public ClanMember Main { get; set; }

        public bool Active { get; set; }

        // Need this saved so we don't have to look it up every time, update often.
        public string ImageUrl { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }

        public List<ClanMemberStatLog> ClanMemberStatLogs { get; set; }
    }
}
