using PlogBot.Services.DiscordObjects;
using System.Threading.Tasks;

namespace PlogBot.Services.Interfaces
{
    public interface IMessageService
    {
        Task SendMessage(ulong channelId, OutgoingMessage message);

        Task SendMessageWithAttachment(ulong channelId, OutgoingMessage message);
        
        Task DeleteMessageAsync(ulong channelId, ulong messageId);
    }
}
