using Newtonsoft.Json;

namespace PlogBot.Processing.EventDataServices.Models
{
    public class Resume
    {
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("session_id")]
        public string SessionId { get; set; }
        [JsonProperty("seq")]
        public int SequenceNumber { get; set; }
    }
}