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

        public int BatchId { get; set; }

        [ForeignKey("ClanMemberId")]
        public ClanMember ClanMember { get; set; }

        public DateTime Recorded { get; set; }

        // Calculated Score
        public int Score { get; set; }

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

        // Equipment
        [ForeignKey("WeaponId")]
        public Item Weapon { get; set; }
        [ForeignKey("Gem1Id")]
        public Item Gem1 { get; set; }
        [ForeignKey("Gem2Id")]
        public Item Gem2 { get; set; }
        [ForeignKey("Gem3Id")]
        public Item Gem3 { get; set; }
        [ForeignKey("Gem4Id")]
        public Item Gem4 { get; set; }
        [ForeignKey("Gem5Id")]
        public Item Gem5 { get; set; }
        [ForeignKey("Gem6Id")]
        public Item Gem6 { get; set; }
        [ForeignKey("RingId")]
        public Item Ring { get; set; }
        [ForeignKey("EarringId")]
        public Item Earring { get; set; }
        [ForeignKey("NecklaceId")]
        public Item Necklace { get; set; }
        [ForeignKey("BraceletId")]
        public Item Bracelet { get; set; }
        [ForeignKey("BeltId")]
        public Item Belt { get; set; }
        [ForeignKey("GlovesId")]
        public Item Gloves { get; set; }
        [ForeignKey("SoulId")]
        public Item Soul { get; set; }
        [ForeignKey("HeartId")]
        public Item Heart { get; set; }
        [ForeignKey("SoulBadgeId")]
        public Item SoulBadge { get; set; }
        [ForeignKey("MysticBadgeId")]
        public Item MysticBadge { get; set; }
        [ForeignKey("PetId")]
        public Item Pet { get; set; }

        public int WeaponId { get; set; }
        public int Gem1Id { get; set; }
        public int Gem2Id { get; set; }
        public int Gem3Id { get; set; }
        public int Gem4Id { get; set; }
        public int Gem5Id { get; set; }
        public int Gem6Id { get; set; }
        public int RingId { get; set; }
        public int EarringId { get; set; }
        public int NecklaceId { get; set; }
        public int BraceletId { get; set; }
        public int BeltId { get; set; }
        public int GlovesId { get; set; }
        public int SoulId { get; set; }
        public int HeartId { get; set; }
        public int SoulBadgeId { get; set; }
        public int MysticBadgeId { get; set; }
        public int PetId { get; set; }
    }
}
