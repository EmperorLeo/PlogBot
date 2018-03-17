using System.Net.Http;

namespace PlogBot.OAuth2.Interfaces
{
    public interface IDiscordApiClient
    {
        HttpClient BotAuth();
    }
}
