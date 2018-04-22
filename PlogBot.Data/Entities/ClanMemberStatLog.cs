using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlogBot.Data.Entities
{
    public class ClanMemberStatLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid ClanMemberId { get; set; }

        [ForeignKey("ClanMemberId")]
        public ClanMember ClanMember { get; set; }

        public DateTime Recorded { get; set; }

        // STATS
        public int Level { get; set; }
        public int HongmoonLevel { get; set; }
        public int AttackPower { get; set; }
        public int PvpAttackPower { get; set; }
        public int BossAttackPower { get; set; }
        public int Critical { get; set; }
        public int CriticalDamage { get; set; }
        public int AdditionalDamage { get; set; }
        public int Accuracy { get; set; }
        public int Piercing { get; set; }
        public int Concentration { get; set; }
        public int DebuffDamage { get; set; }
        public int FlameDamage { get; set; }
        public int FrostDamage { get; set; }
        public int WindDamage { get; set; }
        public int EarthDamage { get; set; }
        public int LightningDamage { get; set; }
        public int ShadowDamage { get; set; }
        public int Health { get; set; }
        public int Defense { get; set; }
        public int PvpDefense { get; set; }
        public int BossDefense { get; set; }
        public int Evasion { get; set; }
        public int Block { get; set; }
        public int CriticalDefense { get; set; }
        public int DamageReduction { get; set; }
        public int HealthRegen { get; set; }
        public int HealthRegenCombat { get; set; }
        public int DebuffDefense { get; set; }
    }
}
