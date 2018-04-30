using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PlogBot.Data;
using PlogBot.Services.Interfaces;
using PlogBot.Services.WebModels;

namespace PlogBot.Services
{
    public class PowerService : IPowerService
    {
        private readonly PlogDbContext _plogDbContext;

        private const double ApWeight = 1;
        private const double BossApWeight = 1;
        private const double PvpApWeight = 1;
        private const double CriticalWeight = 1;
        private const double CriticalDamageWeight = 1;
        private const double AdditionalDamageWeight = 1;
        private const double AccuracyWeight = 1;
        private const double PiercingWeight = 1;
        private const double ConcentrationWeight = 1;
        private const double DebuffDamageWeight = 1;
        private const double ElementalDamageWeight = 1;
        private const double HealthWeight = 1;
        private const double DefenseWeight = 1;
        private const double PvpDefenseWeight = 1;
        private const double BossDefenseWeight = 1;
        private const double EvasionWeight = 1;
        private const double BlockWeight = 1;
        private const double CriticalDefenseWeight = 1;
        private const double DamageReductionWeight = 1;
        private const double HealthRegenWeight = 1;
        private const double HealthRegenCombatWeight = 1;
        private const double DebuffDefenseWeight = 1;

        //public PowerService(PlogDbContext plogDbContext)
        //{
        //    _plogDbContext = plogDbContext;
        //}

        //public Task<int> GetWhaleScoreByCharacterName(string name)
        //{
        //    return _plogDbContext.Plogs
        //        .Where(p => p.Name == name)
        //        .Join(_plogDbContext.Logs, p => p.Id, l => l.ClanMemberId, (p, l) => l)
        //        .OrderBy(l => l.Recorded)
        //        .Take(5)
        //        .MaxAsync(x => x.Score);
        //}

        public Task<int> CalculateScore(AbilitiesResultAbility abilities)
        {
            return Task.Run(() =>
            {
                return (int)Math.Truncate(
                    abilities.AttackPower * ApWeight +
                    abilities.PvpAttackPower * PvpApWeight +
                    abilities.BossAttackPower * BossApWeight + 
                    abilities.Critical * CriticalWeight +
                    abilities.CriticalDamage * CriticalDamageWeight + 
                    abilities.AdditionalDamage * AdditionalDamageWeight +
                    abilities.Accuracy * AccuracyWeight + 
                    abilities.Piercing * PiercingWeight +
                    abilities.Concentration * ConcentrationWeight +
                    abilities.DebuffDamage * DebuffDamageWeight +
                    abilities.FireDamage * ElementalDamageWeight +
                    abilities.IceDamage * ElementalDamageWeight +
                    abilities.LightningDamage * ElementalDamageWeight +
                    abilities.WindDamage * ElementalDamageWeight +
                    abilities.EarthDamage * ElementalDamageWeight +
                    abilities.ShadowDamage * ElementalDamageWeight +
                    abilities.Health * HealthWeight +
                    abilities.Defense * DefenseWeight + 
                    abilities.PvpDefense * PvpDefenseWeight +
                    abilities.BossDefense * BossDefenseWeight +
                    abilities.Evasion * EvasionWeight +
                    abilities.Block * BlockWeight +
                    abilities.CriticalDefense * CriticalDefenseWeight +
                    abilities.DamageReduction * DamageReductionWeight +
                    abilities.HealthRegen * HealthRegenWeight +
                    abilities.HealthCombatRegen * HealthRegenCombatWeight +
                    abilities.DebuffDamageDefense * DebuffDefenseWeight
                );
            });
        }
    }
}
