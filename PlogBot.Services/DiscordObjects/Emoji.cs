using System.Collections.Generic;
using Newtonsoft.Json;

namespace PlogBot.Services.DiscordObjects
{
    public class Emoji
    {
        [JsonProperty("id")]
        public ulong Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("roles")]
        public IList<ulong> Roles { get; set; }
        [JsonProperty("user")]
        public User User { get; set; }
        [JsonProperty("require_colons")]
        public bool RequireColons { get; set; }
        [JsonProperty("managed")]
        public bool Managed { get; set; }
        [JsonProperty("animated")]
        public bool Animated { get; set; }
    }
}