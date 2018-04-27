using System.Threading.Tasks;

namespace PlogBot.Services.Interfaces
{
    public interface ILoggingService
    {
        Task LogAsync(string log);

        Task LogErrorAsync(string error);
    }
}
