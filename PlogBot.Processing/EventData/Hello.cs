using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PlogBot.Processing.Interfaces;

namespace PlogBot.Processing.EventData
{
    public class Hello : IEventData
    {
        [JsonProperty("heartbeat_interval")]
        public int HeartbeatInterval { get; set; }
        [JsonProperty("_trace")]
        public string[] Trace { get; set; }

        public async Task Respond(ClientWebSocket ws, int sequenceNum, string token)
        {

            var identify = new Payload {
                Opcode = 2,
                Data = new Identify
                {
                    Token = token,
                    Properties = new Properties
                    {
                        OperatingSystem = "windows",
                        Browser = "disco",
                        Device = "disco"
                    }
                }
            };


            var heartbeat = new Payload
            {
                Opcode = 1,
                Data = sequenceNum
            };
            await ws.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(identify))), WebSocketMessageType.Binary, true, CancellationToken.None);
            Console.WriteLine("Identified the session.");

            // Setup task to keep heartbeating
            var _ = Task.Run(async () =>
            {
                while (true)
                {
                    Console.WriteLine("Delaying for heartbeat interval: " + HeartbeatInterval + " ms");
                    await Task.Delay(HeartbeatInterval);
                    if (HeartbeatAck.LastHeartbeatRecieved.HasValue && HeartbeatAck.LastHeartbeatRecieved.Value.AddMilliseconds(HeartbeatInterval * 2) < DateTime.UtcNow)
                    {
                        // TODO: try to reconnect?
                    }
                    var arraySegment = new ArraySegment<byte>(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(heartbeat)));
                    await ws.SendAsync(arraySegment, WebSocketMessageType.Binary, true, CancellationToken.None);
                    Console.WriteLine("Successful heartbeat!");
                }
            });
        }
    }
}
