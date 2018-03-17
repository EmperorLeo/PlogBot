using Newtonsoft.Json;

namespace PlogBot.Listening.Messages
{
    public class Hello
    {
        [JsonProperty("heartbeat_interval")]
        public int HeartbeatInterval { get; set; }
        [JsonProperty("_trace")]
        public string Trace { get; set; }
    }
}
