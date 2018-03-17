using PlogBot.Listening.Interfaces;
using PlogBot.Services.Interfaces;
using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace PlogBot.Listening
{
    public class Listener : IListener
    {
        private const int sendChunkSize = 256;
        private const int receiveChunkSize = 256;

        private readonly IGatewayService _gatewayService;

        public Listener(IGatewayService gatewayService)
        {
            _gatewayService = gatewayService;
        }

        public async Task Listen()
        {
            var gateway = await _gatewayService.GetGateway();
            using (var ws = new ClientWebSocket())
            {
                await ws.ConnectAsync(new Uri(gateway.Url), CancellationToken.None);

                while (ws.State == WebSocketState.Open)
                {
                    Console.WriteLine("I'm Open! Implement me!");
                }
            }
        }
    }
}
