using Newtonsoft.Json;
using System.Collections.Generic;

namespace PlogBot.Services.DiscordObjects
{
    public class OutgoingMessage
    {
        [JsonProperty("content")]
        public string Content { get; set; }
        [JsonProperty("embed")]
        public Embed Embed { get; set; }
        [JsonIgnore]
        public byte[] File { get; set; }

    }
}
