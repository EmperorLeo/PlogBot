using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PlogBot.OAuth2;
using PlogBot.OAuth2.Interfaces;
using PlogBot.Services.DiscordObjects;
using PlogBot.Services.Interfaces;

namespace PlogBot.Services
{
    public class GuildService : IGuildService
    {
        private readonly IDiscordApiClient _discordApiClient;

        public GuildService(IDiscordApiClient discordApiClient)
        {
            _discordApiClient = discordApiClient;
        }

        public async Task<Guild> GetGuild(ulong guildId)
        {
            var client = _discordApiClient.BotAuth();
            var response = await client.GetAsync($"{DiscordApiConstants.BaseUrl}/guilds/{guildId}");
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<Guild>(await response.Content.ReadAsStringAsync());
        }

        public async Task<List<Emoji>> GetGuildEmoji(ulong guildId)
        {
            var client = _discordApiClient.BotAuth();
            var response = await client.GetAsync($"{DiscordApiConstants.BaseUrl}/guilds/{guildId}/emojis");
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<List<Emoji>>(await response.Content.ReadAsStringAsync());
        }
    }
}