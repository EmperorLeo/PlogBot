using System.Net.WebSockets;
using System.Threading.Tasks;

namespace PlogBot.Processing.Interfaces
{
    public interface IEventDataService
    {
        Task RespondAsync(ClientWebSocket ws, Payload payload, string token);
    }
}
