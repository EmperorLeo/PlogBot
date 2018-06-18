using System;
using System.Net.Http;
using System.Net.Http.Headers;
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

        public Task DeleteMessageAsync(ulong channelId, ulong messageId)
        {
            var client = _discordApiClient.BotAuth();
            return client.DeleteAsync($"{DiscordApiConstants.BaseUrl}/channels/{channelId}/messages/{messageId}");
        }

        public Task SendMessage(ulong channelId, OutgoingMessage message)
        {
            var client = _discordApiClient.BotAuth();
            return client.PostAsync($"{DiscordApiConstants.BaseUrl}/channels/{channelId}/messages",
                new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json"));
        }

        public Task SendMessageWithAttachment(ulong channelId, OutgoingMessage message)
        {
            var client = _discordApiClient.BotAuth();
            var content = new MultipartFormDataContent();

            var fileContent = new ByteArrayContent(message.File);
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("text/csv");

            content.Add(fileContent, "file", "ploggystyle-clan-log.csv");
            return client.PostAsync($"{DiscordApiConstants.BaseUrl}/channels/{channelId}/messages", content);
        }
    }
}
