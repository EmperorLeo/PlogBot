using System.Net.WebSockets;
using System.Threading.Tasks;

namespace PlogBot.Processing.Interfaces
{
    public interface IEventData
    {
        Task RespondAsync(ClientWebSocket ws, Payload payload, string token);
    }
}
