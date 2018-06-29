using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PlogBot.Services.DiscordObjects
{
    public class Guild
    {
        [JsonProperty("id")]
        public ulong Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("icon")]
        public string Icon { get; set; }
        [JsonProperty("splash")]
        public string Splash { get; set; }
        [JsonProperty("owner")]
        public bool Owner { get; set; }
        [JsonProperty("owner_id")]
        public ulong OwnerId { get; set; }
        [JsonProperty("permissions")]
        public int Permissions { get; set; }
        [JsonProperty("region")]
        public string Region { get; set; }
        [JsonProperty("afk_channel_id")]
        public ulong? AfkChannelId { get; set; }
        [JsonProperty("afk_timeout")]
        public int AfkTimeout { get; set; }
        [JsonProperty("embed_enabled")]
        public bool EmbedEnabled { get; set; }
        [JsonProperty("embed_channel_id")]
        public ulong? EmbedChannelId { get; set; }
        [JsonProperty("verification_level")]
        public VerificationLevel VerificationLevel { get; set; }
        [JsonProperty("default_message_notifications")]
        public DefaultMessageNotificationLevel DefaultMessageNotifications { get; set; }
        [JsonProperty("explicit_content_filter")]
        public ExplicitContentFilterLevel ExplicitContentFilter { get; set; }
        [JsonProperty("roles")]
        public IList<Role> Roles { get; set; }
        [JsonProperty("emojis")]
        public IList<Emoji> Emojis { get; set;}
        [JsonProperty("features")]
        public IList<string> Features { get; set; }
        [JsonProperty("mfa_level")]
        public MfaLevel MfaLevel { get; set; }
        [JsonProperty("application_id")]
        public ulong? ApplicationId { get; set; }
        [JsonProperty("widget_enabled")]
        public bool WidgetEnabled { get; set; }
        [JsonProperty("widget_channel_id")]
        public ulong? WidgetChannelId { get; set; }
        [JsonProperty("system_channel_id")]
        public ulong? SystemChannelId { get; set; }
        [JsonProperty("max_presences")]
        public int? MaxPresences { get; set; }
        // Following fields are only within GUILD_CREATE event, not yet implemented
        // public DateTime JoinedAt { get; set; }
        // public bool Large { get; set; }
        // public bool Unavailable { get; set; }
        // public int MemberCount { get; set; }
        // public IList<VoiceState> VoiceStates { get; set; }
        // public IList<Member> Members { get; set; }
        // public IList<Channel> Channels { get; set; }
        // public IList<Presence> Presences { get; set; }
    }
}