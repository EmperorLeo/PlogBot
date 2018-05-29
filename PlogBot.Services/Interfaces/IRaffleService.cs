using System.Threading.Tasks;

namespace PlogBot.Services.Interfaces
{
    public interface IRaffleService
    {
         Task StartRaffle(int numTickets, ulong channelId);
         Task GetTicket(ulong discordUserId, ulong channelId);
         Task<ulong> DrawTicket(ulong channelId);
         Task<ulong> EndRaffle(ulong channelId);
    }
}