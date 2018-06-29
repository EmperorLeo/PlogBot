using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PlogBot.Data;
using PlogBot.Data.Entities;
using PlogBot.Services.Constants;
using PlogBot.Services.DiscordObjects;
using PlogBot.Services.Extensions;
using PlogBot.Services.Interfaces;

namespace PlogBot.Services
{
    public class AlertService : IAlertService
    {
        private readonly IMessageService _messageService;
        private readonly ITimeZoneService _timeZoneService;
        private readonly PlogDbContext _plogDbContext;

        public AlertService(PlogDbContext plogDbContext, IMessageService messageService, ITimeZoneService timeZoneService)
        {
            _plogDbContext = plogDbContext;
            _messageService = messageService;
            _timeZoneService = timeZoneService;
        }

        public Task BlastAlert(string name, string text, int time, List<ulong> roles, ulong channel)
        {
            var curDate = DateTime.UtcNow;
            var totalDayTime =  curDate.Minute + curDate.Hour * 60;
            var diff = time - totalDayTime;
            if (diff < -15)
            {
                diff += 1440; 
            }

            var whenString = "NOW!";
            if (Math.Abs(diff) > 1)
            {
                whenString = $"in {diff} minutes.";
            }

            var message = new OutgoingMessage
            {
                Content = $"Calling all ",
                Embed = new Embed
                {
                    Title = name,
                    Description = text,
                    Timestamp = DateTime.UtcNow,
                    Color = HexConstants.Green,
                    Fields = new List<EmbedField>
                    {
                        new EmbedField
                        {
                            Name = "Starting",
                            Value = whenString
                        }
                    }
                }
            };

            if (!roles.Any())
            {
                message.Content = null;
            }
            else
            {
                roles.ForEach(r => message.Content += ($"<@&{r}> "));
                message.Content += "!";
            }

            return _messageService.SendMessage(channel, message);
        }

        public async Task CreateAlert(string name, int time, int? day, string text, List<ulong> roles, ulong channel, ulong user)
        {
            var timeInfo = await _timeZoneService.GetTime(user, day, time);
            var alert = new Alert 
            {
                Name = name,
                Description = text,
                Roles = roles.ConcatenateULongs(),
                Time = timeInfo.Item2,
                Day = timeInfo.Item1,
                ChannelId = channel,
                DiscordUserId = user
            };
            _plogDbContext.Add(alert);
            await _plogDbContext.SaveChangesAsync();
        }

        public Embed GetAlertEmbed()
        {
            return new Embed
            {
                Title = "Alert Help Menu",
                Description = "The following commands can be used to set reoccurring events either daily or weekly.",
                Fields = new List<EmbedField>
                {
                    new EmbedField
                    {
                        Name = "Create an alert",
                        Value = "!plog alert create [name] [description] [dayofweek?] [time] [roles...]"
                    },
                    new EmbedField
                    {
                        Name = "Modify an existing alert",
                        Value = "!plog alert modify [name] [description] [dayofweek?] [time] [roles...]"
                    },
                    new EmbedField
                    {
                        Name = "Delete an existing alert",
                        Value = "!plog alert delete [name]"
                    }
                }
            };
        }

        public async Task<List<Alert>> GetReadyAlerts()
        {
            var curDate = DateTime.UtcNow;
            var curWeek = (int)curDate.DayOfWeek;
            var totalDayTime =  curDate.Minute + curDate.Hour * 60;
            var readyAlerts = await _plogDbContext.Alerts
                .Where(x => !x.LastProcessed.HasValue || (curDate - x.LastProcessed.Value).TotalMinutes >= 15)
                .Where(x => !x.Day.HasValue || (x.Day == curWeek && (x.Time - totalDayTime <= 60 && x.Time - totalDayTime >= -14) || (curWeek - x.Day == 1 || curWeek == 6 && x.Day == 0) && (1440 - totalDayTime + x.Time <= 60 && 1440 - totalDayTime + x.Time >= -14)))
                .Where(x => x.Day.HasValue || (x.Time - totalDayTime <= 60 && x.Time - totalDayTime >= -14) || (1440 - totalDayTime + x.Time <= 60 && 1440 - totalDayTime + x.Time >= -14))
                .ToListAsync();

            return readyAlerts;
        }

        public async Task ModifyAlert(string name, int time, int? day, string text, List<ulong> roles, ulong channel)
        {
            var alert = await _plogDbContext.Alerts.Where(x => x.Name == name).FirstOrDefaultAsync();
            var timeInfo = await _timeZoneService.GetTime(alert.DiscordUserId, day, time);
            alert.Time = timeInfo.Item2;
            alert.Day = timeInfo.Item1;
            alert.Description = text;
            alert.Roles = roles.ConcatenateULongs();
            alert.ChannelId = channel;
            _plogDbContext.Update(alert);
            await _plogDbContext.SaveChangesAsync();
        }

        public async Task RetireAlert(string name)
        {
            var alert = await _plogDbContext.Alerts.Where(x => x.Name == name).FirstOrDefaultAsync();
            _plogDbContext.Remove(alert);
            await _plogDbContext.SaveChangesAsync();
        }
    }
}