using PlogBot.Services.DiscordObjects;
using System.Threading.Tasks;

namespace PlogBot.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUser(ulong id);
    }
}
