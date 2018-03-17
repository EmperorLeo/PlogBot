using System.Threading.Tasks;
using Newtonsoft.Json;
using PlogBot.OAuth2;
using PlogBot.OAuth2.Interfaces;
using PlogBot.Services.Interfaces;
using PlogBot.Services.Responses;

namespace PlogBot.Services
{
    public class GatewayService : IGatewayService
    {
        private const string GatewayPath = "/gateway/bot";
        private readonly IDiscordApiClient _discordApiClient;

        public GatewayService(IDiscordApiClient discordApiClient)
        {
            _discordApiClient = discordApiClient;
        }

        public async Task<GatewayResponse> GetGateway()
        {
            var client = _discordApiClient.BotAuth();
            var response = await client.GetAsync($"{DiscordApiConstants.BaseUrl}{GatewayPath}");
            return JsonConvert.DeserializeObject<GatewayResponse>(await response.Content.ReadAsStringAsync());
        }
    }
}
