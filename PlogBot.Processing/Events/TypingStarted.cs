using System;
using Newtonsoft.Json;
using PlogBot.Services.DiscordObjects;

namespace PlogBot.Processing.Events
{
    public class TypingStarted : IEvent
    {
        [JsonProperty("user_id")]
        public ulong UserId { get; set; }
        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }
        [JsonProperty("member")]
        public Member Member { get; set; }
        [JsonProperty("channel_id")]
        public ulong ChannelId { get; set; }
        [JsonProperty("guild_id")]
        public ulong GuildId { get; set; }
    }
}