using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PlogBot.Processing.EventDataServices.Models;
using PlogBot.Processing.Interfaces;
using PlogBot.Services.Interfaces;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PlogBot.Processing.EventDataServices
{
    public class InvalidSessionEventDataService : IInvalidSessionEventDataService
    {
        private readonly ILoggingService _loggingService;

        public InvalidSessionEventDataService(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        public async Task RespondAsync(ClientWebSocket ws, Payload payload, string token)
        {
            var identify = new Payload
            {
                Opcode = 2,
                Data = JObject.FromObject(new Identify
                {
                    Token = token,
                    Properties = new Properties
                    {
                        OperatingSystem = "windows",
                        Browser = "disco",
                        Device = "disco"
                    }
                })
            };

            await ws.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(identify))), WebSocketMessageType.Binary, true, CancellationToken.None);
            await _loggingService.LogAsync("Identified the session again.");
        }
    }
}
