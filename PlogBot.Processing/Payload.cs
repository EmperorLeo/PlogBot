using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PlogBot.Processing
{
    public class Payload
    {
        [JsonProperty("op")]
        public int Opcode { get; set; }
        [JsonProperty("d")]
        public JToken Data { get; set; }
        [JsonProperty("s")]
        public int? SequenceNumber { get; set; }
        [JsonProperty("t")]
        public string EventName { get; set; }
    }
}
