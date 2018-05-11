namespace PlogBot.Services.DiscordObjects
{
    public class Attachment
    {
        public ulong Id { get; set; }
        public string FileName { get; set; }
        public int Size { get; set; }
        public string Url { get; set; }
        public string ProxyUrl { get; set; }
    }
}
