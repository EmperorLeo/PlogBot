using System.Threading.Tasks;

namespace PlogBot.Services.Interfaces
{
    public interface IMessageService
    {
        Task SendMessage(ulong channelId, string content);
    }
}
