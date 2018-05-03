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
                client.BaseAddress = new Uri("http://na-bns.ncsoft.com");
                var batchNumber = await db.Logs.MaxAsync(l => l.BatchId) + 1;
                var loggingProcesses = db.Plogs.Select(p => ForEachPlog_GetInformation(p)).ToList();
                await Task.WhenAll(loggingProcesses);
                var items = await ProcessItems(loggingProcesses.Select(lp => lp.Result?.Items).Where(i => i != null).ToList(), db);
                var saves = loggingProcesses.Select(lp => ForEachPlog_Add(lp.Result, db, items, batchNumber));
                await Task.WhenAll(saves);

                try
                {
                    await db.SaveChangesAsync();
                }
                catch(Exception e)
                {
                    await _loggingService.LogErrorAsync(e.StackTrace);
                }
            }
        }

        public static async Task<CharacterInformation> ForEachPlog_GetInformation(ClanMember plog)
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

            if (character == null || stats == null || items == null)
            {
                // Try again later?
                return null;
            }

            var isInClan = character.Clan.Equals("Ploggystyle", StringComparison.OrdinalIgnoreCase);
            plog.Active = isInClan;

            if (!plog.Active)
            {
                // Don't process
                return null;
            }

            var end = DateTime.UtcNow;
            await _loggingService.LogAsync($"Processing time for {plog.Name}: {(end - start).TotalMilliseconds}");

            return new CharacterInformation
            {
                Plog = plog,
                Character = character,
                AbilitiesResult = stats,
                Items = items
            };
        }

        public static async Task ForEachPlog_Add(CharacterInformation info, PlogDbContext db, Dictionary<string, Item> itemDict, int batchNumber)
        {
            var totalPower = info.AbilitiesResult.Records.TotalAbility;

            var powerService = new PowerService();
            var powerScore = await powerService.CalculateScore(totalPower);

            var items = info.Items;

            var statLog = new ClanMemberStatLog
            {
                ClanMemberId = info.Plog.Id,
                Recorded = DateTime.UtcNow,
                BatchId = batchNumber,

                Level = info.Character.Level, // No api route known, hard to parse from html
                HongmoonLevel = info.Character.HongmoonLevel,
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

                Weapon = itemDict.GetValue(items.Weapon),
                Gem1 = itemDict.GetValue(items.Gem1),
                Gem2 = itemDict.GetValue(items.Gem2),
                Gem3 = itemDict.GetValue(items.Gem3),
                Gem4 = itemDict.GetValue(items.Gem4),
                Gem5 = itemDict.GetValue(items.Gem5),
                Gem6 = itemDict.GetValue(items.Gem6),
                Ring = itemDict.GetValue(items.Ring),
                Earring = itemDict.GetValue(items.Earring),
                Necklace = itemDict.GetValue(items.Necklace),
                Bracelet = itemDict.GetValue(items.Bracelet),
                Belt = itemDict.GetValue(items.Belt),
                Gloves = itemDict.GetValue(items.Gloves),
                Soul = itemDict.GetValue(items.Soul),
                Heart = itemDict.GetValue(items.Heart),
                Pet = itemDict.GetValue(items.Pet),
                SoulBadge = itemDict.GetValue(items.SoulBadge),
                MysticBadge = itemDict.GetValue(items.MysticBadge),
            };

            db.Logs.Add(statLog);
        }

        private static async Task<Dictionary<string, Item>> ProcessItems(List<BladeAndSoulItems> itemGroups, PlogDbContext db)
        {
            var start = DateTime.UtcNow;

            var allItems = await db.Items.ToDictionaryAsync(i => i.Name, i => i);
            foreach (var itemGroup in itemGroups)
            {
                if (itemGroup.Weapon != null && !allItems.ContainsKey(itemGroup.Weapon))
                {
                    allItems.Add(itemGroup.Weapon, CreateNewItem(db, new Item { ItemType = ItemType.Weapon, Name = itemGroup.Weapon, ImgUrl = itemGroup.WeaponImg }));
                }

                if (itemGroup.Gem1 != null && !allItems.ContainsKey(itemGroup.Gem1))
                {
                    allItems.Add(itemGroup.Gem1, CreateNewItem(db, new Item { ItemType = ItemType.Gem, Name = itemGroup.Gem1, ImgUrl = itemGroup.Gem1Img }));
                }

                if (itemGroup.Gem2 != null && !allItems.ContainsKey(itemGroup.Gem2))
                {
                    allItems.Add(itemGroup.Gem2, CreateNewItem(db, new Item { ItemType = ItemType.Gem, Name = itemGroup.Gem2, ImgUrl = itemGroup.Gem2Img }));
                }

                if (itemGroup.Gem3 != null && !allItems.ContainsKey(itemGroup.Gem3))
                {
                    allItems.Add(itemGroup.Gem3, CreateNewItem(db, new Item { ItemType = ItemType.Gem, Name = itemGroup.Gem3, ImgUrl = itemGroup.Gem3Img }));
                }

                if (itemGroup.Gem4 != null && !allItems.ContainsKey(itemGroup.Gem4))
                {
                    allItems.Add(itemGroup.Gem4, CreateNewItem(db, new Item { ItemType = ItemType.Gem, Name = itemGroup.Gem4, ImgUrl = itemGroup.Gem4Img }));
                }

                if (itemGroup.Gem5 != null && !allItems.ContainsKey(itemGroup.Gem5))
                {
                    allItems.Add(itemGroup.Gem5, CreateNewItem(db, new Item { ItemType = ItemType.Gem, Name = itemGroup.Gem5, ImgUrl = itemGroup.Gem5Img }));
                }

                if (itemGroup.Gem6 != null && !allItems.ContainsKey(itemGroup.Gem6))
                {
                    allItems.Add(itemGroup.Gem6, CreateNewItem(db, new Item { ItemType = ItemType.Gem, Name = itemGroup.Gem6, ImgUrl = itemGroup.Gem6Img }));
                }

                if (itemGroup.Ring != null && !allItems.ContainsKey(itemGroup.Ring))
                {
                    allItems.Add(itemGroup.Ring, CreateNewItem(db, new Item { ItemType = ItemType.Ring, Name = itemGroup.Ring, ImgUrl = itemGroup.RingImg }));
                }

                if (itemGroup.Earring != null && !allItems.ContainsKey(itemGroup.Earring))
                {
                    allItems.Add(itemGroup.Earring, CreateNewItem(db, new Item { ItemType = ItemType.Earring, Name = itemGroup.Earring, ImgUrl = itemGroup.EarringImg }));
                }

                if (itemGroup.Necklace != null && !allItems.ContainsKey(itemGroup.Necklace))
                {
                    allItems.Add(itemGroup.Necklace, CreateNewItem(db, new Item { ItemType = ItemType.Necklace, Name = itemGroup.Necklace, ImgUrl = itemGroup.NecklaceImg }));
                }

                if (itemGroup.Bracelet != null && !allItems.ContainsKey(itemGroup.Bracelet))
                {
                    allItems.Add(itemGroup.Bracelet, CreateNewItem(db, new Item { ItemType = ItemType.Bracelet, Name = itemGroup.Bracelet, ImgUrl = itemGroup.BraceletImg }));
                }

                if (itemGroup.Belt != null && !allItems.ContainsKey(itemGroup.Belt))
                {
                    allItems.Add(itemGroup.Belt, CreateNewItem(db, new Item { ItemType = ItemType.Belt, Name = itemGroup.Belt, ImgUrl = itemGroup.BeltImg }));
                }

                if (itemGroup.Gloves != null && !allItems.ContainsKey(itemGroup.Gloves))
                {
                    allItems.Add(itemGroup.Gloves, CreateNewItem(db, new Item { ItemType = ItemType.Gloves, Name = itemGroup.Gloves, ImgUrl = itemGroup.GlovesImg }));
                }

                if (itemGroup.Pet != null && !allItems.ContainsKey(itemGroup.Pet))
                {
                    allItems.Add(itemGroup.Pet, CreateNewItem(db, new Item { ItemType = ItemType.Pet, Name = itemGroup.Pet, ImgUrl = itemGroup.PetImg }));
                }

                if (itemGroup.Soul != null && !allItems.ContainsKey(itemGroup.Soul))
                {
                    allItems.Add(itemGroup.Soul, CreateNewItem(db, new Item { ItemType = ItemType.Soul, Name = itemGroup.Soul, ImgUrl = itemGroup.SoulImg }));
                }

                if (itemGroup.Heart != null && !allItems.ContainsKey(itemGroup.Heart))
                {
                    allItems.Add(itemGroup.Heart, CreateNewItem(db, new Item { ItemType = ItemType.Heart, Name = itemGroup.Heart, ImgUrl = itemGroup.HeartImg }));
                }

                if (itemGroup.SoulBadge != null && !allItems.ContainsKey(itemGroup.SoulBadge))
                {
                    allItems.Add(itemGroup.SoulBadge, CreateNewItem(db, new Item { ItemType = ItemType.SoulBadge, Name = itemGroup.SoulBadge, ImgUrl = itemGroup.SoulBadgeImg }));
                }

                if (itemGroup.MysticBadge != null && !allItems.ContainsKey(itemGroup.MysticBadge))
                {
                    allItems.Add(itemGroup.MysticBadge, CreateNewItem(db, new Item { ItemType = ItemType.MysticBadge, Name = itemGroup.MysticBadge, ImgUrl = itemGroup.MysticBadgeImg }));
                }
            }

            // Save all items to the database.
            await db.SaveChangesAsync();

            var end = DateTime.UtcNow;
            await _loggingService.LogAsync($"Processing time to save items to the database: {(end - start).TotalMilliseconds}");

            return allItems;
        }

        private static Item CreateNewItem(PlogDbContext db, Item item)
        {
            db.Items.Add(item);
            return item;
        }
    }
}
