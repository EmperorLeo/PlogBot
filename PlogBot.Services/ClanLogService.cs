using Microsoft.EntityFrameworkCore;
using PlogBot.Data;
using PlogBot.Services.Interfaces;
using PlogBot.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlogBot.Services
{
    public class ClanLogService : IClanLogService
    {
        private readonly PlogDbContext _plogDbContext;

        public ClanLogService(PlogDbContext plogDbContext)
        {
            _plogDbContext = plogDbContext;
        }

        public async Task<string> GetCsv(DateTime since)
        {
            var entries = await _plogDbContext.Logs.Where(l => l.Recorded >= since).OrderBy(l => l.BatchId).ThenBy(l => l.Recorded).Select(l => new ClanLogCsvEntry
            {
                CharacterName = l.ClanMember.Name,
                Active = l.ClanMember.Active,
                Level = l.Level,
                HmLevel = l.HongmoonLevel,
                BatchNumber = l.BatchId,
                Recorded = l.Recorded,
                //Weapon = l.Weapon != null ? l.Weapon.Name : null,
                //Gems = $"{l.Gem1.Name}+{l.Gem2.Name} + {l.Gem3.Name} + {l.Gem4.Name} + {l.Gem5.Name} + {l.Gem5.Name} + {l.Gem6.Name}",
                //Earring = l.Earring.Name,
                //Ring = l.Ring.Name,
                //Necklace = l.Necklace.Name,
                //Belt = l.Belt.Name,
                //Bracelet = l.Bracelet.Name,
                //Gloves = l.Gloves.Name,
                //Pet = l.Pet.Name,
                //Soul = l.Pet.Name,
                //Heart = l.Heart.Name,
                //SoulBadge = l.SoulBadge.Name,
                //MysticBadge = l.MysticBadge.Name
            }).ToListAsync();

            var entryDict = new Dictionary<string, Dictionary<int, ClanLogCsvEntry>>();
            var batchToDate = new Dictionary<int, DateTime>();

            var batchNumber = 0;
            var batchNumbers = new List<int>();
            var characterNames = new HashSet<string>();

            // Process entries into an easy dataset to write CSV with.
            foreach (var entry in entries)
            {
                characterNames.Add(entry.CharacterName);

                if (entry.BatchNumber != batchNumber)
                {
                    batchToDate.Add(entry.BatchNumber, entry.Recorded);
                    batchNumbers.Add(entry.BatchNumber);
                    batchNumber = entry.BatchNumber;
                }


                if (!entryDict.ContainsKey(entry.CharacterName))
                {
                    entryDict.Add(entry.CharacterName, new Dictionary<int, ClanLogCsvEntry>());
                }

                var innerDict = entryDict[entry.CharacterName];
                if (!innerDict.ContainsKey(entry.BatchNumber))
                {
                    entryDict[entry.CharacterName].Add(entry.BatchNumber, entry);
                }
            }

            var sb = new StringBuilder();
            foreach (var batch in batchNumbers)
            {
                // Very first cell should be blank
                sb.Append(",");
                if (batchToDate.ContainsKey(batch))
                {
                    sb.Append(batchToDate[batch]);
                }
            }
            sb.Append("\n");


            foreach (var name in characterNames.OrderBy(x => x))
            {
                sb.Append($"{name}");
                foreach (var i in batchNumbers)
                {
                    sb.Append(",");
                    if (entryDict.ContainsKey(name) && entryDict[name].ContainsKey(i))
                    {
                        // Checkmark - They were active then so they were processed.
                        sb.Append('\u2714');
                    }
                }
                sb.Append("\n");
            }

            return sb.ToString();
        }
    }
}
