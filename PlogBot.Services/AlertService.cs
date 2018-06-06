using System.Collections.Generic;
using System.Threading.Tasks;
using PlogBot.Services.DiscordObjects;
using PlogBot.Services.Interfaces;

namespace PlogBot.Services
{
    public class AlertService : IAlertService
    {
        public Task BlastAlert(string name, string tect, List<ulong> roles, ulong channel)
        {
            throw new System.NotImplementedException();
        }

        public Task CreateAlert(string name, int time, int? day, string text, List<ulong> roles, ulong channel)
        {
            throw new System.NotImplementedException();
        }

        public Embed GetAlertEmbed()
        {
            throw new System.NotImplementedException();
        }

        public Task GetReadyAlerts()
        {
            throw new System.NotImplementedException();
        }

        public Task ModifyAlert(string name, int time, int? day, string text, List<ulong> roles, ulong channel)
        {
            throw new System.NotImplementedException();
        }

        public Task RetireAlert(string name)
        {
            throw new System.NotImplementedException();
        }
    }
}