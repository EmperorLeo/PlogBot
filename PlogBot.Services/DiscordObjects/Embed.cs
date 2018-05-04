using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PlogBot.Services.DiscordObjects
{
    public class Embed
    {
        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }
        [JsonProperty("timestamp", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? Timestamp { get; set; }
        [JsonProperty("color", NullValueHandling = NullValueHandling.Ignore)]
        public int? Color { get; set; }
        [JsonProperty("footer", NullValueHandling = NullValueHandling.Ignore)]
        public EmbedItem Footer { get; set; }
        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
        public EmbedItem Image { get; set; }
        [JsonProperty("thumbnail", NullValueHandling = NullValueHandling.Ignore)]
        public EmbedItem Thumbnail { get; set; }
        [JsonProperty("provider", NullValueHandling = NullValueHandling.Ignore)]
        public EmbedItem Provider { get; set; }
        [JsonProperty("author", NullValueHandling = NullValueHandling.Ignore)]
        public EmbedItem Author { get; set; }
        [JsonProperty("fields", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<EmbedField> Fields { get; set; }
    }

    public class EmbedItem
    {
        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }
        [JsonProperty("proxy_url", NullValueHandling = NullValueHandling.Ignore)]
        public string ProxyUrl { get; set; }
        [JsonProperty("height", NullValueHandling = NullValueHandling.Ignore)]
        public int? Height { get; set; }
        [JsonProperty("width", NullValueHandling = NullValueHandling.Ignore)]
        public int? Width { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("icon_url", NullValueHandling = NullValueHandling.Ignore)]
        public string IconUrl { get; set; }
        [JsonProperty("proxy_icon_url", NullValueHandling = NullValueHandling.Ignore)]
        public string ProxyIconUrl { get; set; }
        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }
    }

    public class EmbedField
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public string Value { get; set; }
        [JsonProperty("inline", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Inline { get; set; }
    }
}
