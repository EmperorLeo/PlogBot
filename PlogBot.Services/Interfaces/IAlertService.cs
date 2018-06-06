using System.Collections.Generic;
using System.Threading.Tasks;
using PlogBot.Services.DiscordObjects;

namespace PlogBot.Services.Interfaces
{
    public interface IAlertService
    {
         Task CreateAlert(string name, int time, int? day, string text, List<ulong> roles, ulong channel);
         Task ModifyAlert(string name, int time, int? day, string text, List<ulong> roles, ulong channel);
         Task RetireAlert(string name);
         Task GetReadyAlerts();
         Task BlastAlert(string name, string tect, List<ulong> roles, ulong channel);
         Embed GetAlertEmbed();
    }
}