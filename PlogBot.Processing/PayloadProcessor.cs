using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PlogBot.Configuration;
using PlogBot.Processing.Interfaces;
using System;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace PlogBot.Processing
{
    public class PayloadProcessor : IPayloadProcessor
    {
        public static int LastSequenceNumber;

        private readonly IEventDataFactory _eventDataFactory;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly string _token;

        public PayloadProcessor(IEventDataFactory eventDataFactory, IServiceScopeFactory serviceScopeFactory, IOptions<AppSettings> options)
        {
            _eventDataFactory = eventDataFactory;
            _serviceScopeFactory = serviceScopeFactory;
            _token = options.Value.BotToken;
        }

        public async Task Process(string streamData, ClientWebSocket ws)
        {
            var payload = JsonConvert.DeserializeObject<Payload>(streamData);
            if (payload.SequenceNumber.HasValue)
            {
                LastSequenceNumber = payload.SequenceNumber.Value;
            }
            Console.WriteLine("Recieved opcode: " + payload.Opcode);
            Console.WriteLine("Processed Data: " + streamData);
            // We want to create a new scope when we process a payload so we can resolve scoped services like DbContexts and DispatchData.
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                IEventData eventData = _eventDataFactory.BuildEventData(scope, payload.Opcode, JsonConvert.SerializeObject(payload.Data));
                await eventData.RespondAsync(ws, payload, _token);
            }
        }
    }
}
