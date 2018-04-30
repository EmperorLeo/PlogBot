using Microsoft.EntityFrameworkCore;
using PlogBot.Data;
using PlogBot.Data.Entities;
using PlogBot.Data.Enums;
using PlogBot.Services;
using PlogBot.Services.Extensions;
using PlogBot.Services.Interfaces;
using PlogBot.Services.Models;
using PlogBot.Services.WebModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PlogBot.DataSync
{
    public class Program
    {
        public static ILoggingService _loggingService;
        public static IBladeAndSoulService _bladeAndSoulService;
        public static SemaphoreSlim _semaphoreSlim;

        static Program()
        {
            _loggingService = new LoggingService();
            _bladeAndSoulService = new BladeAndSoulService(_loggingService);
            _semaphoreSlim = new SemaphoreSlim(1);
        }

        public static void Main(string[] args)
        {
            DataSyncClient().Wait();
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
                var items = await db.Items.ToDictionaryAsync(i => i.Name, i => i);
                client.BaseAddress = new Uri("http://na-bns.ncsoft.com");
                var loggingProcesses = db.Plogs.Select(p => ForEachPlog(p, db, items)).ToList();
                await Task.WhenAll(loggingProcesses);
                await db.SaveChangesAsync();
            }
        }

        public static async Task ForEachPlog(ClanMember plog, PlogDbContext db, Dictionary<string, Item> itemPool)
        {
            var start = DateTime.UtcNow;

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

            var totalPower = stats.Records.TotalAbility;

            var powerService = new PowerService();
            var powerScore = await powerService.CalculateScore(totalPower);

            // Wait here because the dictionary of existing items is not thread safe.
            await _semaphoreSlim.WaitAsync();

            var statLog = new ClanMemberStatLog
            {
                ClanMemberId = plog.Id,
                Recorded = DateTime.UtcNow,
                Level = character.Level, // No api route known, hard to parse from html
                HongmoonLevel = character.HongmoonLevel,
                Score = powerScore,
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
                DebuffDefense = totalPower.DebuffDamageDefense,

                Weapon = GetItem(itemPool, ItemType.Weapon, items.Weapon, items.WeaponImg),
                Gem1 = GetItem(itemPool, ItemType.Gem, items.Gem1, items.Gem1Img),
                Gem2 = GetItem(itemPool, ItemType.Gem, items.Gem2, items.Gem2Img),
                Gem3 = GetItem(itemPool, ItemType.Gem, items.Gem3, items.Gem3Img),
                Gem4 = GetItem(itemPool, ItemType.Gem, items.Gem4, items.Gem4Img),
                Gem5 = GetItem(itemPool, ItemType.Gem, items.Gem5, items.Gem5Img),
                Gem6 = GetItem(itemPool, ItemType.Gem, items.Gem6, items.Gem6Img),
                Ring = GetItem(itemPool, ItemType.Ring, items.Ring, items.RingImg),
                Earring = GetItem(itemPool, ItemType.Earring, items.Earring, items.EarringImg),
                Necklace = GetItem(itemPool, ItemType.Necklace, items.Necklace, items.NecklaceImg),
                Bracelet = GetItem(itemPool, ItemType.Bracelet, items.Bracelet, items.BraceletImg),
                Belt = GetItem(itemPool, ItemType.Belt, items.Belt, items.BeltImg),
                Gloves = GetItem(itemPool, ItemType.Gloves, items.Gloves, items.GlovesImg),
                Soul = GetItem(itemPool, ItemType.Soul, items.Soul, items.SoulImg),
                Heart = GetItem(itemPool, ItemType.Heart, items.Heart, items.HeartImg),
                Pet = GetItem(itemPool, ItemType.Pet, items.Pet, items.PetImg),
                SoulBadge = GetItem(itemPool, ItemType.SoulBadge, items.SoulBadge, items.SoulBadgeImg),
                MysticBadge = GetItem(itemPool, ItemType.MysticBadge, items.MysticBadge, items.MysticBadgeImg)
            };

            // Done modifying items dictionary, release the lock!
            _semaphoreSlim.Release();


            db.Logs.Add(statLog);

            var end = DateTime.UtcNow;
            await _loggingService.LogAsync($"Processing time for {plog.Name}: {(end - start).TotalMilliseconds}");
        }

        private static Item GetItem(Dictionary<string, Item> items, ItemType type, string name, string imgUrl)
        {
            if (name == null)
            {
                return null;
            }

            if (items.ContainsKey(name))
            {
                return items[name];
            }
            else
            {
                var newItem = new Item
                {
                    Name = name,
                    ItemType = type,
                    ImgUrl = imgUrl
                };
                items.Add(name, newItem);
                return newItem;
            }
        }
    }
}
