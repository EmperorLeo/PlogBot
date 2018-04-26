using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PlogBot.Processing.Interfaces;

namespace PlogBot.Processing.EventData
{
    public class InvalidSession : IEventData
    {
        public async Task RespondAsync(ClientWebSocket ws, Payload payload, string token)
        {
            var identify = new Payload
            {
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

            await ws.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(identify))), WebSocketMessageType.Binary, true, CancellationToken.None);
            Console.WriteLine("Identified the session again.");
        }
    }
}
