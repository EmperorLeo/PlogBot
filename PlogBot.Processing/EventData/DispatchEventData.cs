using PlogBot.Processing.Enums;
using PlogBot.Processing.Events;
using PlogBot.Processing.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace PlogBot.Processing.EventData
{
    public class DispatchEventData : IDispatchEventData
    {
        public string Data { get; set; }

        private readonly Dictionary<string, IEventProcessor<IEvent>> _processingDict;

        public DispatchEventData(
            IEventProcessor<MessageCreate> messageCreateProcessor
        )
        {
            _processingDict = new Dictionary<string, IEventProcessor<IEvent>>
            {
                { EventTypes.MESSAGE_CREATE.ToString(), messageCreateProcessor }
            };
        }

        public void Initialize(string data)
        {
            Console.WriteLine("Dispatch Data: " + data);
            Data = data;
        }

        public async Task RespondAsync(ClientWebSocket ws, Payload payload, string token)
        {
            if (_processingDict.ContainsKey(payload.EventName))
            {
                var processor = _processingDict[payload.EventName];
                try
                {
                    await processor.ProcessEvent(Data);
                    Console.WriteLine("Responded to Dispatch event data");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Dispatch event failed to process. Stacktrace: {ex.StackTrace}");
                }
            }
            else
            {
                Console.WriteLine("No processor found for Dispatch event data");
            }
        }
    }
}
