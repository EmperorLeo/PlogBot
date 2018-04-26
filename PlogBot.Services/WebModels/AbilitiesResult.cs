using Newtonsoft.Json;

namespace PlogBot.Services.WebModels
{
    public class AbilitiesResult
    {
        public AbilitiesResultRecords Records { get; set; }
        public string Result { get; set; }
    }

    public class AbilitiesResultRecords
    {
        [JsonProperty("base_ability")]
        public AbilitiesResultAbility BaseAbility { get; set; }
        [JsonProperty("equipped_ability")]
        public AbilitiesResultAbility EquippedAbility { get; set; }
        [JsonProperty("total_ability")]
        public AbilitiesResultAbility TotalAbility { get; set; }
    }

    public class AbilitiesResultAbility
    {
        [JsonProperty("int_attack_power_value")]
        public int AttackPower { get; set; }
        [JsonProperty("int_attack_pierce_value")]
        public int Piercing { get; set; }
        [JsonProperty("attack_defend_pierce_rate")]
        public double PierceDefenseRate { get; set; }
        [JsonProperty("attack_parry_pierce_rate")]
        public double PierceParryRate { get; set; }
        [JsonProperty("int_attack_hit_value")]
        public int Accuracy { get; set; }
        [JsonProperty("attack_hit_rate")]
        public double AccuracyRate { get; set; }
        [JsonProperty("int_attack_concentrate_value")]
        public int Concentration { get; set; }
        [JsonProperty("attack_perfect_parry_damage_rate")]
        public double ConcentrationParryDamageRate { get; set; }
        [JsonProperty("attack_counter_damage_rate")]
        public double ConcentrationCounterDamageRate { get; set; }
        [JsonProperty("int_attack_critical_value")]
        public int Critical { get; set; }
        [JsonProperty("attack_critical_rate")]
        public double CriticalRate { get; set; }
        [JsonProperty("attack_stiff_duration_level")]
        public double StiffDurationLevel { get; set; }
        [JsonProperty("int_attack_damage_modify_diff")]
        public int AdditionalDamage { get; set; }
        [JsonProperty("attack_damage_modify_rate")]
        public double AdditionalDamageBonus { get; set; }
        [JsonProperty("int_hate_power_value")]
        public int Threat { get; set; }
        [JsonProperty("hate_power_rate")]
        public double ThreatBonus { get; set; }
        [JsonProperty("int_max_hp")]
        public int Health { get; set; }
        [JsonProperty("int_defend_power_value")]
        public int Defense { get; set; }
        [JsonProperty("defend_physical_damage_reduce_rate")]
        public double DefenseDamageReductionRate { get; set; }
        [JsonProperty("int_aoe_defend_power_value")]
        public int AoeDefense { get; set; }
        [JsonProperty("aoe_defend_damage_reduce_rate")]
        public double AoeDefenseDamageReductionRate { get; set; }
        [JsonProperty("int_defend_dodge_value")]
        public int Evasion { get; set; }
        [JsonProperty("defend_dodge_rate")]
        public double EvasionRate { get; set; }
        [JsonProperty("counter_damage_reduce_rate")]
        public double CounterImprovementRate { get; set; }
        [JsonProperty("int_defend_parry_value")]
        public int Block { get; set; }
        [JsonProperty("defend_parry_reduce_rate")]
        public double BlockDamageReductionRate { get; set; }
        [JsonProperty("perfect_parry_damage_reduce_rate")]
        public double BlockImprovementRate { get; set; }
        [JsonProperty("defend_parry_rate")]
        public double BlockRate { get; set; }
        [JsonProperty("int_defend_critical_value")]
        public int CriticalDefense { get; set; }
        [JsonProperty("defend_critical_rate")]
        public double CriticalDefenseRate { get; set; }
        [JsonProperty("defend_critical_damage_rate")]
        public double CriticalDefenseDamageReduction { get; set; }
        [JsonProperty("defend_stiff_duration_level")]
        public double Willpower { get; set; }
        [JsonProperty("int_defend_damage_modify_diff")]
        public int DamageReduction { get; set; }
        [JsonProperty("defend_damage_modify_rate")]
        public double DamageReductionRate { get; set; }
        [JsonProperty("int_hp_regen")]
        public int HealthRegen { get; set; }
        [JsonProperty("int_hp_regen_combat")]
        public int HealthCombatRegen { get; set; }
        [JsonProperty("heal_power_rate")]
        public double RecoveryRate { get; set; }
        [JsonProperty("int_heal_power_value")]
        public int Recovery { get; set; }
        [JsonProperty("heal_power_diff")]
        public double RecoveryBonus { get; set; }
        [JsonProperty("attack_critical_damage_value")]
        public int CriticalDamage { get; set; }
        [JsonProperty("attack_critical_damage_rate")]
        public double CriticalDamageRate { get; set; }
        [JsonProperty("attack_attribute_fire_value")]
        public int FireDamage { get; set; }
        [JsonProperty("attack_attribute_fire_rate")]
        public double FireDamageRate { get; set; }
        [JsonProperty("attack_attribute_ice_value")]
        public int IceDamage { get; set; }
        [JsonProperty("attack_attribute_ice_rate")]
        public double IceDamageRate { get; set; }
        [JsonProperty("attack_attribute_wind_value")]
        public int WindDamage { get; set; }
        [JsonProperty("attack_attribute_wind_rate")]
        public double WindDamageRate { get; set; }
        [JsonProperty("attack_attribute_earth_value")]
        public int EarthDamage { get; set; }
        [JsonProperty("attack_attribute_earth_rate")]
        public double EarthDamageRate { get; set; }
        [JsonProperty("attack_attribute_lightning_value")]
        public int LightningDamage { get; set; }
        [JsonProperty("attack_attribute_lightning_rate")]
        public double LightningDamageRate { get; set; }
        [JsonProperty("attack_attribute_void_value")]
        public int ShadowDamage { get; set; }
        [JsonProperty("attack_attribute_void_rate")]
        public double ShadowDamageRate { get; set; }
        [JsonProperty("abnormal_attack_power_value")]
        public int DebuffDamage { get; set; }
        [JsonProperty("abnormal_attack_power_rate")]
        public double DebuffDamageRate { get; set; }
        [JsonProperty("pc_attack_power_value")]
        public int PvpAttackPower { get; set; }
        [JsonProperty("boss_attack_power_value")]
        public int BossAttackPower { get; set; }
        [JsonProperty("pc_defend_power_value")]
        public int PvpDefense { get; set; }
        [JsonProperty("pc_defend_power_rate")]
        public double PvpDefenseRate { get; set; }
        [JsonProperty("boss_defend_power_value")]
        public int BossDefense { get; set; }
        [JsonProperty("boss_defend_power_rate")]
        public double BossDefenseRate { get; set; }
        [JsonProperty("abnormal_defend_power_value")]
        public int DebuffDamageDefense { get; set; }
        [JsonProperty("abnormal_defend_power_rate")]
        public double DebuffDamageDefenseRate { get; set; }
    }
}
