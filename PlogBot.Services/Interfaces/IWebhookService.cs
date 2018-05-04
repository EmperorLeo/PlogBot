using System.Threading.Tasks;

namespace PlogBot.Services.Interfaces
{
    public interface IWebhookService
    {
        Task ExecuteInactivePlogWebhook(string name);
    }
}
