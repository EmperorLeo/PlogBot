using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PlogBot.Configuration;
using PlogBot.Processing.Interfaces;
using PlogBot.Services.Interfaces;
using System;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace PlogBot.Processing
{
    public class PayloadProcessor : IPayloadProcessor
    {
        private readonly IEventDataServiceFactory _eventDataServiceFactory;
        private readonly ILoggingService _loggingService;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ISessionService _sessionService;
        private readonly string _token;

        public PayloadProcessor(
            IEventDataServiceFactory eventDataServiceFactory,
            ILoggingService loggingService,
            IServiceScopeFactory serviceScopeFactory,
            ISessionService sessionService,
            IOptions<AppSettings> options
        )
        {
            _eventDataServiceFactory = eventDataServiceFactory;
            _loggingService = loggingService;
            _serviceScopeFactory = serviceScopeFactory;
            _sessionService = sessionService;
            _token = options.Value.BotToken;
        }

        public async Task Process(string streamData, ClientWebSocket ws)
        {
            var payload = JsonConvert.DeserializeObject<Payload>(streamData);
            if (payload.SequenceNumber.HasValue)
            {
                _sessionService.LastSequenceNumber = payload.SequenceNumber.Value;
            }
            await _loggingService.LogAsync("Recieved opcode: " + payload.Opcode);
            await _loggingService.LogAsync("Recieved data: " + payload.Data.ToString());
            // We want to create a new scope when we process a payload so we can resolve scoped services like DbContexts and DispatchData.
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                IEventDataService eventDataService = _eventDataServiceFactory.BuildEventDataService(scope, payload.Opcode);
                await eventDataService.RespondAsync(ws, payload, _token);
            }
        }
    }
}
