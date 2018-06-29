using Newtonsoft.Json;
using PlogBot.Services.DiscordObjects;

namespace PlogBot.Processing.Events
{
    public class GuildMemberAdd : IEvent
    {
        public Member Member { get; set; }
    }
}