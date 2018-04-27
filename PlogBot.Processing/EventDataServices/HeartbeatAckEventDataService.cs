using PlogBot.Processing.Interfaces;
using PlogBot.Services.Interfaces;
using System;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace PlogBot.Processing.EventDataServices
{
    public class HeartbeatAckEventDataService : IHeartbeatAckEventDataService
    {
        private readonly ISessionService _sessionService;
        private readonly ILoggingService _loggingService;

        public HeartbeatAckEventDataService(ISessionService sessionService, ILoggingService loggingService)
        {
            _sessionService = sessionService;
            _loggingService = loggingService;
        }

        public Task RespondAsync(ClientWebSocket ws, Payload payload, string token)
        {
            _sessionService.LastHeartbeatAck = DateTime.UtcNow;
            return _loggingService.LogAsync("Processed heartbeat ACK.");
        }
    }
}
