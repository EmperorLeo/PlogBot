using Newtonsoft.Json;
using PlogBot.Processing.Interfaces;
using System;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace PlogBot.Processing.EventDataServices.Models
{
    public class Ready : IEventDataService
    {
        [JsonProperty("v")]
        public int Version { get; set; }
        [JsonProperty("user")]
        public dynamic User { get; set; }
        [JsonProperty("private_channels")]
        public dynamic[] PrivateChannels { get; set; }
        [JsonProperty("guilds")]
        public dynamic[] Guilds { get; set; }
        [JsonProperty("session_id")]
        public string SessionId { get; set; }
        [JsonProperty("_trace")]
        public string[] Trace { get; set; }

        public Task RespondAsync(ClientWebSocket ws, Payload payload, string token)
        {
            throw new NotImplementedException();
        }
    }
}
