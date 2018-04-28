using PlogBot.Data;
using PlogBot.Data.Entities;
using PlogBot.Services;
using PlogBot.Services.Interfaces;
using PlogBot.Services.Models;
using PlogBot.Services.WebModels;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PlogBot.DataSync
{
    public class Program
    {
        public static ILoggingService _loggingService;
        public static IBladeAndSoulService _bladeAndSoulService;

        static Program()
        {
            _loggingService = new LoggingService();
            _bladeAndSoulService = new BladeAndSoulService(_loggingService);
        }

        public static void Main(string[] args)
        {
            SaveNewLogs().Wait();
        }

        public async static Task DataSyncClient()
        {
            while (true)
            {
                try
                {
                    await SaveNewLogs();
                }
                catch (Exception ex)
                {
                    await _loggingService.LogErrorAsync(ex.StackTrace);
                }
                await Task.Delay(10000);
            }
        }

        public async static Task SaveNewLogs()
        {
            var bladeAndSoulService = new BladeAndSoulService(new LoggingService());

            using (var client = new HttpClient())
            using (var db = new PlogDbContext())
            {
                client.BaseAddress = new Uri("http://na-bns.ncsoft.com");
                var loggingProcesses = db.Plogs.Select(p => ForEachPlog(p, db)).ToList();
                await Task.WhenAll(loggingProcesses);
                await db.SaveChangesAsync();
            }
        }

        public static async Task ForEachPlog(ClanMember plog, PlogDbContext db)
        {
            var tasks = new Task[]
            {
                _bladeAndSoulService.GetBladeAndSoulCharacter(plog.Name),
                _bladeAndSoulService.GetBladeAndSoulCharacterAbilities(plog.Name),
                _bladeAndSoulService.GetBladeAndSoulItemsAsync(plog.Name)
            };

            await Task.WhenAll(tasks);

            var character = ((Task<BladeAndSoulCharacter>)tasks[0]).Result;
            var stats = ((Task<AbilitiesResult>)tasks[1]).Result;
            var items = ((Task<BladeAndSoulItems>)tasks[2]).Result;

            if (character == null || stats == null)
            {
                // Try again later?
                return;
            }

            var isInClan = character.Clan.Equals("Ploggystyle", StringComparison.OrdinalIgnoreCase);
            plog.Active = isInClan;

            if (!plog.Active)
            {
                // Don't process
                return;
            }

            if (character.Clan.ToLower() != "ploggystyle")
            {
                plog.Active = false;
            }

            var totalPower = stats.Records.TotalAbility;

            db.Logs.Add(new ClanMemberStatLog
            {
                ClanMemberId = plog.Id,
                Recorded = DateTime.UtcNow,
                Level = character.Level, // No api route known, hard to parse from html
                HongmoonLevel = character.HongmoonLevel,
                AttackPower = totalPower.AttackPower,
                PvpAttackPower = totalPower.PvpAttackPower,
                BossAttackPower = totalPower.BossAttackPower,
                Critical = totalPower.Critical,
                CriticalDamage = totalPower.CriticalDamage,
                AdditionalDamage = totalPower.AdditionalDamage,
                Accuracy = totalPower.Accuracy,
                Piercing = totalPower.Piercing,
                Concentration = totalPower.Concentration,
                DebuffDamage = totalPower.DebuffDamage,
                FlameDamage = totalPower.FireDamage,
                FrostDamage = totalPower.IceDamage,
                EarthDamage = totalPower.EarthDamage,
                LightningDamage = totalPower.LightningDamage,
                ShadowDamage = totalPower.ShadowDamage,
                WindDamage = totalPower.WindDamage,
                Health = totalPower.Health,
                Defense = totalPower.Defense,
                PvpDefense = totalPower.PvpDefense,
                BossDefense = totalPower.BossDefense,
                Evasion = totalPower.Evasion,
                Block = totalPower.Block,
                CriticalDefense = totalPower.CriticalDefense,
                DamageReduction = totalPower.DamageReduction,
                HealthRegen = totalPower.HealthRegen,
                HealthRegenCombat = totalPower.HealthCombatRegen,
                DebuffDefense = totalPower.DebuffDamageDefense
            });
        }
    }
}
