using PlogBot.Services.Responses;
using System.Threading.Tasks;

namespace PlogBot.Services.Interfaces
{
    public interface IGatewayService
    {
        Task<GatewayResponse> GetGateway();
    }
}
