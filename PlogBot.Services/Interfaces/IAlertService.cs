using System.Collections.Generic;
using System.Threading.Tasks;
using PlogBot.Data.Entities;
using PlogBot.Services.DiscordObjects;

namespace PlogBot.Services.Interfaces
{
    public interface IAlertService
    {
         Task CreateAlert(string name, int time, int? day, string text, List<ulong> roles, ulong channel, ulong discordUserId);
         Task ModifyAlert(string name, int time, int? day, string text, List<ulong> roles, ulong channel);
         Task RetireAlert(string name);
         Task<List<Alert>> GetReadyAlerts();
         Task BlastAlert(string name, string text, int time, List<ulong> roles, ulong channel);
         Embed GetAlertEmbed();
    }
}