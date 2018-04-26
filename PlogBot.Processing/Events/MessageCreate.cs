using PlogBot.Services.DiscordObjects;

namespace PlogBot.Processing.Events
{
    public class MessageCreate : IEvent
    {
        public Message Message { get; set; }
    }
}
