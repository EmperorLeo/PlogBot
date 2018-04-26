using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PlogBot.OAuth2;
using PlogBot.OAuth2.Interfaces;
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

        public async Task SendMessage(ulong channelId, string content)
        {
            var client = _discordApiClient.BotAuth();
            var result = await client.PostAsync($"{DiscordApiConstants.BaseUrl}/channels/{channelId}/messages",
                new StringContent(JsonConvert.SerializeObject(new
            {
                content
            }), Encoding.UTF8, "application/json"));
            if (result.IsSuccessStatusCode)
            {
                Console.WriteLine("Send Message Success");
            }
            else
            {
                Console.WriteLine("Send Message ERROR");
            }
        }
    }
}
