using Newtonsoft.Json;

namespace PlogBot.Processing
{
    public class Payload
    {
        [JsonProperty("op")]
        public int Opcode { get; set; }
        [JsonProperty("d")]
        public dynamic Data { get; set; }
        [JsonProperty("s")]
        public int? SequenceNumber { get; set; }
        [JsonProperty("t")]
        public string EventName { get; set; }
    }
}
