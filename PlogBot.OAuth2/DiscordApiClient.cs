using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using PlogBot.Configuration;
using PlogBot.OAuth2.Interfaces;

namespace PlogBot.OAuth2
{
    public class DiscordApiClient : IDiscordApiClient
    {
        private readonly HttpClient _botAuthClient;

        public DiscordApiClient(IOptions<AppSettings> options)
        {
            _botAuthClient = new HttpClient();
            _botAuthClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(DiscordApiConstants.BotAuthorizationScheme, options.Value.BotToken);
        }

        public HttpClient BotAuth()
        {
            return _botAuthClient;
        }
    }
}
