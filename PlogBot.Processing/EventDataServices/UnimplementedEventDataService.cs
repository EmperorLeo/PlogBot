using PlogBot.Processing.Interfaces;
using PlogBot.Services.Interfaces;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace PlogBot.Processing.EventDataServices
{
    public class UnimplementedEventDataService : IUnimplementedEventDataService
    {
        private readonly ILoggingService _loggingService;

        public UnimplementedEventDataService(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        public Task RespondAsync(ClientWebSocket ws, Payload payload, string token)
        {
            return _loggingService.LogAsync($"Processor for opcode {payload.Opcode} not implemented.");
        }
    }
}
