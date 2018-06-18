using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PlogBot.Data;
using PlogBot.Data.Entities;
using PlogBot.Services.Interfaces;

namespace PlogBot.Services
{
    public class TimeZoneService : ITimeZoneService
    {
        private static Dictionary<string, int> _timeZoneOffsets =
            new Dictionary<string, int>() {
            {"ACDT", 630 },
            {"ACST", 570 },
            {"ADT", -180 },
            {"AEDT", 660 },
            {"AEST", 600 },
            {"AHDT", -540 },
            {"AHST", -600 },
            {"AST", -240 },
            {"AT", -120 },
            {"AWDT", 540 },
            {"AWST", 480 },
            {"BAT", 180 },
            {"BDST", 120 },
            {"BET", -660 },
            {"BST", -180 },
            {"BT", 180 },
            {"BZT2", -180 },
            {"CADT", 630 },
            {"CAST", 570 },
            {"CAT", -600 },
            {"CCT", 480 },
            {"CDT", -300 },
            {"CED", 120 },
            {"CET", 60 },
            {"CEST", 120 },
            {"CST", -360 },
            {"EAST", 600 },
            {"EDT", -240 },
            {"EED", 180 },
            {"EET", 120 },
            {"EEST", 180 },
            {"EST", -300 },
            {"FST", 120 },
            {"FWT", 60 },
            {"GMT", 0 },
            {"GST", 600 },
            {"HDT", -540 },
            {"HST", -600 },
            {"IDLE", 720 },
            {"IDLW", -720 },
            {"IST", 330 },
            {"IT", 210 },
            {"JST", 540 },
            {"JT", 420 },
            {"MDT", -360 },
            {"MED", 120 },
            {"MET", 60 },
            {"MEST", 120 },
            {"MEWT", 60 },
            {"MST", -420 },
            {"MT", 480 },
            {"NDT", -150 },
            {"NFT", -210 },
            {"NT", -660 },
            {"NST", 390 },
            {"NZ", 660 },
            {"NZST", 720 },
            {"NZDT", 780 },
            {"NZT", 720 },
            {"PDT", -420 },
            {"PST", -480 },
            {"ROK", 540 },
            {"SAD", 600 },
            {"SAST", 540 },
            {"SAT", 540 },
            {"SDT", 600 },
            {"SST", 120 },
            {"SWT", 60 },
            {"USZ3", 240 },
            {"USZ4", 300 },
            {"USZ5", 360 },
            {"USZ6", 420 },
            {"UT", 0 },
            {"UTC", 0 },
            {"UZ10", 660 },
            {"WAT", -60 },
            {"WET", 0 },
            {"WST", 480 },
            {"YDT", -480 },
            {"YST", -540 },
            {"ZP4", 240 },
            {"ZP5", 300 },
            {"ZP6", 360 }
        };
        private readonly PlogDbContext _plogDbContext;

        public TimeZoneService(PlogDbContext plogDbContext)
        {
            _plogDbContext = plogDbContext;
        }

        public Tuple<int?, int> GetTime(string abbreviation, int? localDays, int localTime)
        {
            // Code in progress :(
            var offset = _timeZoneOffsets[abbreviation.ToUpper()];
            var utcTime = localTime - offset;
            if (utcTime >= 60 * 24)
            {
                if (localDays.HasValue)
                {
                    if (localDays == 7)
                    {
                        localDays = 0;
                    }
                    else
                    {
                        localDays++;
                    }
                }
                utcTime -= (60 * 24);
            }
            else if (utcTime < 0)
            {
                if (localDays.HasValue)
                {
                    if (localDays == 0)
                    {
                        localDays = 7;
                    }
                    else
                    {
                        localDays--;
                    }
                }
                utcTime += (60 * 24);
            }

            // Local days has been overwritten with UTC days
            return new Tuple<int?, int>(localDays, utcTime);
        }

        public async Task<Tuple<int?, int>> GetTime(ulong discordUserId, int? localDays, int localTime)
        {
            var abbreviation = await _plogDbContext.Times.Where(t => t.DiscordUserId == discordUserId).Select(t => t.TimeZone).FirstOrDefaultAsync();
            return GetTime(abbreviation, localDays, localTime);
        }

        public Task<bool> HasTimeZoneSet(ulong discordUserId)
        {
            return _plogDbContext.Times.AnyAsync(t => t.DiscordUserId == discordUserId);
        }

        public bool IsValid(string abbreviation)
        {
            return _timeZoneOffsets.ContainsKey(abbreviation.ToUpper());
        }

        public async Task SaveTimeZonePreference(string abbreviation, ulong discordUserId)
        {
            var timePref = await _plogDbContext.Times.FirstOrDefaultAsync(t => t.DiscordUserId == discordUserId);
            if (timePref == null)
            {
                timePref = new TimeZonePreference
                {
                    DiscordUserId = discordUserId,
                    TimeZone = abbreviation
                };
                _plogDbContext.Add(timePref);
            }
            else
            {
                timePref.TimeZone = abbreviation;
                _plogDbContext.Update(timePref);
            }

            await _plogDbContext.SaveChangesAsync();
        }
    }
}