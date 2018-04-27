using Newtonsoft.Json;

namespace PlogBot.Processing.EventDataServices.Models
{
    public class Identify
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("properties")]
        public Properties Properties { get; set; }
    }

    public class Properties
    {
        [JsonProperty("$os")]
        public string OperatingSystem { get; set; }
        [JsonProperty("$browser")]
        public string Browser { get; set; }
        [JsonProperty("$device")]
        public string Device { get; set; }
    }
}
