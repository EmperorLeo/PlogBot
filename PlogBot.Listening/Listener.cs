using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PlogBot.Configuration;
using PlogBot.Listening.Interfaces;
using PlogBot.Processing;
using PlogBot.Processing.EventDataServices.Models;
using PlogBot.Processing.Interfaces;
using PlogBot.Services.Interfaces;
using System;
using System.IO.Pipes;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PlogBot.Listening
{
    public class Listener : IListener
    {
        private const int sendChunkSize = 256;
        private const int receiveChunkSize = 256;

        private readonly IGatewayService _gatewayService;
        private readonly IPayloadProcessor _payloadProcessor;
        private readonly IUtilityService _utilityService;

        public Listener(IGatewayService gatewayService, IPayloadProcessor payloadProcessor, IUtilityService utilityService)
        {
            _gatewayService = gatewayService;
            _payloadProcessor = payloadProcessor;
            _utilityService = utilityService;
        }

        public async Task Listen()
        {
            //var cancellationSource = new CancellationTokenSource();
            //var _ = Task.Run(() => ListenInternal(), cancellationSource.Token);

            var gateway = await _gatewayService.GetGateway();
            // Loop forever, never want to stop trying to reconnect
            while (true)
            {
                using (var ws = new ClientWebSocket())
                {
                    await ws.ConnectAsync(new Uri(gateway.Url), CancellationToken.None);

                    while (ws.State == WebSocketState.Open)
                    {
                        var endOfMessage = false;
                        var sb = new StringBuilder();
                        while(!endOfMessage)
                        {
                            var bytesReceived = new ArraySegment<byte>(new byte[receiveChunkSize]);
                            var result = await ws.ReceiveAsync(bytesReceived, CancellationToken.None);
                            sb.Append(_utilityService.FromArraySegmentBytes(bytesReceived));
                            endOfMessage = result.EndOfMessage;
                        }
                        await _payloadProcessor.Process(sb.ToString(), ws);
                    }
                }
            }
        }
    }
}
