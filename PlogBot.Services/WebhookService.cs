using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PlogBot.Configuration;
using PlogBot.Services.DiscordObjects;
using PlogBot.Services.Interfaces;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PlogBot.Services
{
    public class WebhookService : IWebhookService
    {
        private readonly string _inactivePlogWebhook;

        public WebhookService(IOptions<AppSettings> options)
        {
            _inactivePlogWebhook = options.Value.PlogWebhook;
        }

        public async Task ExecuteInactivePlogWebhook(string name)
        {
            var message = new OutgoingMessage
            {
                Content = $"{name} has left the clan! :sob:"
            };
            using (var client = new HttpClient())
            {
                await client.PostAsync(_inactivePlogWebhook, new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json"));
            }
        }
    }
}
