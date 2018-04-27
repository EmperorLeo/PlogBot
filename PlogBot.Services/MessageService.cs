using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PlogBot.OAuth2;
using PlogBot.OAuth2.Interfaces;
using PlogBot.Services.DiscordObjects;
using PlogBot.Services.Interfaces;

namespace PlogBot.Services
{
    public class MessageService : IMessageService
    {
        private readonly IDiscordApiClient _discordApiClient;

        public MessageService(IDiscordApiClient discordApiClient)
        {
            _discordApiClient = discordApiClient;
        }

        public async Task SendMessage(ulong channelId, OutgoingMessage message)
        {
            var client = _discordApiClient.BotAuth();
            var result = await client.PostAsync($"{DiscordApiConstants.BaseUrl}/channels/{channelId}/messages",
                new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json"));
        }
    }
}
