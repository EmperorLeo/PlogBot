using System.Net.WebSockets;
using System.Threading.Tasks;

namespace PlogBot.Processing.Interfaces
{
    public interface IPayloadProcessor
    {
        Task Process(string streamData, ClientWebSocket ws);
    }
}
