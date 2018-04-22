using HtmlAgilityPack;
using Newtonsoft.Json;
using PlogBot.Data;
using PlogBot.Data.Entities;
using PlogBot.DataSync.WebModels;
using System;
using System.IO.Pipes;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PlogBot.DataSync
{
    public class Program
    {
        public static void Main(string[] args)
        {
            SaveNewLogs().Wait();
        }

        public async static Task DataSyncClient()
        {
            while (true)
            {
                await SaveNewLogs();
                await Task.Delay(10000);
            }
        }

        public async static Task SaveNewLogs()
        {
            using (var client = new HttpClient())
            using (var db = new PlogDbContext())
            {
                client.BaseAddress = new Uri("http://na-bns.ncsoft.com");
                var plogs = db.Plogs.ToList();

                foreach (var plog in plogs)
                {
                    var profileHtml = await client.GetAsync($"/ingame/bs/character/profile?c={HttpUtility.UrlDecode(plog.Name)}");

                    if (!profileHtml.IsSuccessStatusCode)
                    {
                        continue;
                    }

                    var htmlDocument = new HtmlDocument();
                    htmlDocument.Load(await profileHtml.Content.ReadAsStreamAsync());

                    int.TryParse(htmlDocument.DocumentNode.SelectSingleNode("//span[@class=\"masteryLv\"]")?.InnerText.Split(' ')[1], out var hmLevel);
                    var guild = htmlDocument.DocumentNode.SelectSingleNode("//li[@class=\"guild\"]")?.InnerText;

                    if (guild != "Ploggystyle" && plog.Active)
                    {
                        plog.Active = false;
                        var pipeClient = new NamedPipeClientStream(".", "plogpipe", PipeDirection.Out);
                        await pipeClient.ConnectAsync();
                        var message = new { MessageType = "ClanLeaveMessage", InactivePlog = plog.Name };
                        await pipeClient.WriteAsync(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message)), 0, 256);
                        continue;
                    }

                    var abilities = await client.GetAsync($"/ingame/bs/character/data/abilities.json?c={HttpUtility.UrlEncode(plog.Name)}");
                    var result = JsonConvert.DeserializeObject<AbilitiesResult>(await abilities.Content.ReadAsStringAsync());

                    if (result.Result.ToLower() != "success")
                    {
                        continue;
                    }

                    var totalPower = result.Records.TotalAbility;

                    db.Logs.Add(new ClanMemberStatLog
                    {
                        ClanMemberId = plog.Id,
                        Recorded = DateTime.UtcNow,
                        Level = 0, // No api route known, hard to parse from html
                        HongmoonLevel = hmLevel,
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

                await db.SaveChangesAsync();
            }
        }
    }
}
