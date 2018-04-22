using Newtonsoft.Json;
using PlogBot.Processing.Interfaces;
using System;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace PlogBot.Processing.EventData
{
    public class DispatchEventData : IDispatchEventData
    {
        public dynamic Data { get; set; }

        public void Initialize(string data)
        {
            Console.WriteLine("Dispatch Data: " + data);
            Data = JsonConvert.DeserializeObject(data);
        }

        public Task Respond(ClientWebSocket ws, int sequenceNum, string token)
        {
            Console.WriteLine("Responded to Dispatch event data");
            return Task.CompletedTask;
        }
    }
}
