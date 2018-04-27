using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PlogBot.Services.DiscordObjects
{
    public class Embed
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("timestamp")]
        public DateTime? Timestamp { get; set; }
        [JsonProperty("color")]
        public int? Color { get; set; }
        [JsonProperty("footer")]
        public EmbedItem Footer { get; set; }
        [JsonProperty("image")]
        public EmbedItem Image { get; set; }
        [JsonProperty("thumbnail")]
        public EmbedItem Thumbnail { get; set; }
        [JsonProperty("provider")]
        public EmbedItem Provider { get; set; }
        [JsonProperty("author")]
        public EmbedItem Author { get; set; }
        [JsonProperty("fields")]
        public IEnumerable<EmbedField> Fields { get; set; }
    }

    public class EmbedItem
    {
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("proxy_url")]
        public string ProxyUrl { get; set; }
        [JsonProperty("height")]
        public int? Height { get; set; }
        [JsonProperty("width")]
        public int? Width { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("icon_url")]
        public string IconUrl { get; set; }
        [JsonProperty("proxy_icon_url")]
        public string ProxyIconUrl { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class EmbedField
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
        [JsonProperty("inline")]
        public bool? Inline { get; set; }
    }
}
