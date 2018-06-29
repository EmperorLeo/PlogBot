using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PlogBot.Data;
using PlogBot.Services.Interfaces;
using PlogBot.Services.Models;
using PlogBot.Services.WebModels;

namespace PlogBot.Services
{
    public class PowerService : IPowerService
    {
        private readonly PlogDbContext _plogDbContext;

        private const double ApWeight = 1;
        private const double BossApWeight = 1;
        private const double PvpApWeight = 1;
        private const double CriticalWeight = 0.16;
        private const double CriticalDamageWeight = 0.35;
        private const double AdditionalDamageWeight = 0.028;
        private const double AccuracyWeight = 0.4;
        private const double PiercingWeight = 0.25;
        private const double ConcentrationWeight = 1;
        private const double DebuffDamageWeight = 1;
        private const double ElementalDamageWeight = 1.1;
        private const double HealthWeight = 0.008;
        private const double DefenseWeight = 0.1;
        private const double PvpDefenseWeight = 1;
        private const double BossDefenseWeight = 1;
        private const double EvasionWeight = 0.2;
        private const double BlockWeight = 0.2;
        private const double CriticalDefenseWeight = 0.4;
        private const double DamageReductionWeight = 1;
        private const double HealthRegenWeight = 1;
        private const double HealthRegenCombatWeight = 0.2;
        private const double DebuffDefenseWeight = 1;

        public PowerService(PlogDbContext plogDbContext)
        {
           _plogDbContext = plogDbContext;
        }

        public Task<int> CalculateScore(AbilitiesResultAbility abilities)
        {
            return Task.Run(() =>
            {
                return (int)Math.Truncate(
                    abilities.AttackPower * ApWeight +
                    (abilities.PvpAttackPower - abilities.AttackPower) * PvpApWeight +
                    (abilities.BossAttackPower - abilities.AttackPower) * BossApWeight + 
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
                    (abilities.PvpDefense - abilities.Defense) * PvpDefenseWeight +
                    (abilities.BossDefense - abilities.Defense) * BossDefenseWeight +
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

        public Task<List<WhaleField>> GetWhales(int numWhales)
        {
            return _plogDbContext.Logs.GroupBy(x => x.ClanMemberId).Select(x => new
            {
                ClanMemberId = x.Key,
                Score = x.Max(c => c.Score),
            }).Join(_plogDbContext.Plogs, l => l.ClanMemberId, p => p.Id, (l, p) => new WhaleField
            {
                Name = p.Name,
                CharacterClass = p.Class,
                Score = l.Score
            }).OrderByDescending(x => x.Score).Take(numWhales).ToListAsync();
        }

        public Task<List<WhaleField>> GetWhaleScoresForUser(ulong discordUserId)
        {
            return _plogDbContext.Logs.GroupBy(x => x.ClanMemberId).Select(x => new
            {
                ClanMemberId = x.Key,
                Score = x.Max(c => c.Score),
            }).Join(_plogDbContext.Plogs, l => l.ClanMemberId, p => p.Id, (l, p) => new
            {
                p.Name,
                p.Class,
                p.DiscordId,
                l.Score
            }).Where(x => x.DiscordId == discordUserId).Select(a => new WhaleField
            {
                Name = a.Name,
                CharacterClass = a.Class,
                Score = a.Score
            }).ToListAsync();

            // TODO: try amd write more performant, working code.
            // return _plogDbContext.Plogs.Where(x => x.DiscordId == discordUserId).Join(_plogDbContext.Logs, p => p.Id, l => l.ClanMemberId, (p, l) => new WhaleField
            // {
            //     Name = p.Name,
            //     CharacterClass = p.Class,
            //     Score = l.Score
            // }).GroupBy(whale => new { whale.Name, whale.CharacterClass }).Select(x => new WhaleField
            // {
            //     Name = x.Key.Name,
            //     Score = x.Max(c => c.Score),
            //     CharacterClass = x.Key.CharacterClass
            // }).ToListAsync();
        }
    }
}
