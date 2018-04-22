using System.Net.WebSockets;
using System.Threading.Tasks;

namespace PlogBot.Processing.Interfaces
{
    public interface IEventData
    {
        Task Respond(ClientWebSocket ws, int sequenceNum, string token);
    }
}
