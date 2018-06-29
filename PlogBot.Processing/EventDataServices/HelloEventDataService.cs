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
    public class HelloEventDataService : IHelloEventDataService
    {
        private Hello _hello;
        private readonly ISessionService _sessionService;
        private readonly ILoggingService _loggingService;

        public HelloEventDataService(ISessionService sessionService, ILoggingService loggingService)
        {
            _sessionService = sessionService;
            _loggingService = loggingService;
        }

        public async Task RespondAsync(ClientWebSocket ws, Payload payload, string token)
        {
            _hello = JsonConvert.DeserializeObject<Hello>(payload.Data.ToString());

            if (_sessionService.Reconnect)
            {
                var resume = new Payload 
                {
                    Opcode = 6,
                    Data = JObject.FromObject(new Resume
                    {
                        SessionId = _sessionService.SessionId,
                        Token = token,
                        SequenceNumber = _sessionService.LastSequenceNumber.HasValue ? _sessionService.LastSequenceNumber.Value : 0
                    })
                };
                var arraySegment = new ArraySegment<byte>(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(resume)));
                await ws.SendAsync(arraySegment, WebSocketMessageType.Binary, true, CancellationToken.None);
                await _loggingService.LogAsync("Reconnected.");
            }
            else
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
                await _loggingService.LogAsync("Identified the session.");
                // Now that we have identified, next time this method is called we want to reconnect.
                _sessionService.Reconnect = true;
            }

            // Setup task to keep heartbeating
            var heartbeatCancellationTokenSource = new CancellationTokenSource();
            var heartbeatCancellationToken = heartbeatCancellationTokenSource.Token;
            var _ = Task.Run(async () =>
            {
                while (true)
                {
                    await _loggingService.LogAsync("Delaying for heartbeat interval: " + _hello.HeartbeatInterval + " ms");
                    await Task.Delay(_hello.HeartbeatInterval, heartbeatCancellationToken);
                    if (_sessionService.LastHeartbeatAck.HasValue && _sessionService.LastHeartbeatAck.Value.AddMilliseconds(_hello.HeartbeatInterval * 2) < DateTime.UtcNow)
                    {
                        // TODO: try to reconnect?
                        await _loggingService.LogAsync("Closing websocket, attempting to reconnect...");
                        await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Discord won't send me heartbeats.", CancellationToken.None);
                        heartbeatCancellationTokenSource.Cancel();
                    }
                    try
                    {
                        var heartbeat = new Payload
                        {
                            Opcode = 1,
                            Data = JToken.FromObject(_sessionService.LastSequenceNumber.Value)
                        };
                        var arraySegment = new ArraySegment<byte>(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(heartbeat)));
                        await ws.SendAsync(arraySegment, WebSocketMessageType.Binary, true, CancellationToken.None);
                        await _loggingService.LogAsync("Successful heartbeat!");
                    }
                    catch (Exception ex)
                    {
                        await _loggingService.LogErrorAsync($"What the heck is happening to my heartbeat?? {ex.StackTrace}");
                    }
                }
            }, heartbeatCancellationToken);
            heartbeatCancellationToken.Register(async () => await _loggingService.LogErrorAsync("HEARTBEAT WAS CANCELED SOMEHOW!"));
        }
    }
}
