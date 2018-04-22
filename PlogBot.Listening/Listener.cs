using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PlogBot.Configuration;
using PlogBot.Listening.Interfaces;
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
        private readonly string _token;

        public Listener(IGatewayService gatewayService, IPayloadProcessor payloadProcessor, IOptions<AppSettings> options, IUtilityService utilityService)
        {
            _gatewayService = gatewayService;
            _payloadProcessor = payloadProcessor;
            _token = options.Value.BotToken;
            _utilityService = utilityService;
        }

        public async Task Listen()
        {
            //var cancellationSource = new CancellationTokenSource();
            //var _ = Task.Run(() => ListenInternal(), cancellationSource.Token);

            var gateway = await _gatewayService.GetGateway();
            using (var ws = new ClientWebSocket())
            {
                await ws.ConnectAsync(new Uri(gateway.Url), CancellationToken.None);

                while (ws.State == WebSocketState.Open)
                {
                    Console.WriteLine("I'm Open! Implement me!");
                    var endOfMessage = false;
                    var sb = new StringBuilder();
                    while(!endOfMessage)
                    {
                        var bytesReceived = new ArraySegment<byte>(new byte[receiveChunkSize]);
                        var result = await ws.ReceiveAsync(bytesReceived, CancellationToken.None);
                        sb.Append(_utilityService.FromArraySegmentBytes(bytesReceived));
                        endOfMessage = result.EndOfMessage;
                    }
                    var data = _payloadProcessor.Process(sb.ToString());
                    await data.Respond(ws, _payloadProcessor.GetLastSequenceNumber(), _token);
                }
            }

            //cancellationSource.Cancel();
        }

        public async Task ListenInternal()
        {
            var pipeServer = new NamedPipeServerStream("plogpipe", PipeDirection.In);
            while (true)
            {
                await pipeServer.WaitForConnectionAsync();
                try
                {
                    var bytes = new byte[256];
                    await pipeServer.ReadAsync(bytes, 0, 256);
                    var result = JsonConvert.DeserializeAnonymousType(Encoding.UTF8.GetString(bytes), new { MessageType="", InactivePlog="" });
                    Console.WriteLine(result.InactivePlog);
                }
                catch
                {
                    Console.WriteLine("MASSIVE ERROR!!");
                }
            }
        }
    }
}
