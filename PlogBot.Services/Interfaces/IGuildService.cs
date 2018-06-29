using System.Collections.Generic;
using System.Threading.Tasks;
using PlogBot.Services.DiscordObjects;

namespace PlogBot.Services.Interfaces
{
    public interface IGuildService
    {
         Task<Guild> GetGuild(ulong guildId);

         Task<List<Emoji>> GetGuildEmoji(ulong guildId);
    }
}