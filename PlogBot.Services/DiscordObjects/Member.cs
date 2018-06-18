using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PlogBot.Services.DiscordObjects
{
    public class Member
    {
        [JsonProperty("user")]
        public User User { get; set; }
        [JsonProperty("roles")]
        public IEnumerable<ulong> Roles { get; set; }
        [JsonProperty("nick")]
        public string Nick { get; set; }
        [JsonProperty("mute")]
        public bool Mute { get; set; }
        [JsonProperty("joined_at")]
        public DateTime JoinedAt { get; set; }
        [JsonProperty("deaf")]
        public bool Deaf { get; set; }
    }
}