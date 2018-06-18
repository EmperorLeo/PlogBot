using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PlogBot.Services.DiscordObjects
{
    public class Message
    {
        [JsonProperty("id")]
        public ulong Id { get; set; }
        [JsonProperty("channel_id")]
        public ulong ChannelId { get; set; }
        [JsonProperty("author")]
        public User Author { get; set; }
        [JsonProperty("content")]
        public string Content { get; set; }
        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }
        [JsonProperty("edited_timestamp")]
        public DateTime? EditedTimestamp { get; set; }
        [JsonProperty("tts")]
        public bool TTS { get; set; }
        [JsonProperty("mention_everyone")]
        public bool MentionEveryone { get; set; }
        [JsonProperty("mentions")]
        public IEnumerable<User> Mentions { get; set; }
        [JsonProperty("mention_roles")]
        public IEnumerable<ulong> MentionRoles { get; set; }
        [JsonProperty("attachments")]
        public IEnumerable<Attachment> Attachments { get; set; }
        [JsonProperty("embeds")]
        public IEnumerable<Embed> Embeds { get; set; }
        [JsonProperty("reactions")]
        public IEnumerable<Reaction> Reactions { get; set; }
        [JsonProperty("nonce")]
        public ulong? Nonce { get; set; }
        [JsonProperty("pinned")]
        public bool Pinned { get; set; }
        [JsonProperty("webhook_id")]
        public ulong? WebhookId { get; set; }
        [JsonProperty("type")]
        public int Type { get; set; }
        [JsonProperty("activity")]
        public MessageActivity Activity { get; set; }
        [JsonProperty("application")]
        public MessageApplication Application { get; set; }
    }
}
