using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PlogBot.Services.DiscordObjects
{
    public class Channel
    {
        [JsonProperty("id")]
        public ulong Id { get; set; }
        [JsonProperty("type")]
        public int Type { get; set; }
        [JsonProperty("guild_id", NullValueHandling = NullValueHandling.Ignore)]
        public ulong? GuildId { get; set; }
        [JsonProperty("position", NullValueHandling = NullValueHandling.Ignore)]
        public int? Position { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("topic", NullValueHandling = NullValueHandling.Ignore)]
        public string Topic { get; set; }
        [JsonProperty("nsfw", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Pornographic { get; set; }
        [JsonProperty("last_message_id", NullValueHandling = NullValueHandling.Ignore)]
        public ulong? LastMessageId { get; set; }
        [JsonProperty("bitrate", NullValueHandling = NullValueHandling.Ignore)]
        public ulong? Bitrate { get; set; }
        [JsonProperty("user_limit", NullValueHandling = NullValueHandling.Ignore)]
        public int? UserLimit { get; set; }
        [JsonProperty("recipients", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<User> Recipients { get; set; }
        [JsonProperty("icon", NullValueHandling = NullValueHandling.Ignore)]
        public string Icon { get; set; }
        [JsonProperty("owner_id", NullValueHandling = NullValueHandling.Ignore)]
        public ulong? OwnerId { get; set; }
        [JsonProperty("application_id", NullValueHandling = NullValueHandling.Ignore)]
        public ulong? ApplicationId { get; set; }
        [JsonProperty("parent_id", NullValueHandling = NullValueHandling.Ignore)]
        public ulong? ParentId { get; set; }
        [JsonProperty("last_pin_timestamp", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? LastPinTimestamp { get; set; }
    }
}
