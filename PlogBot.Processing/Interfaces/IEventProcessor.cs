using PlogBot.Processing.Events;
using System.Threading.Tasks;

namespace PlogBot.Processing.Interfaces
{
    public interface IEventProcessor<out T> where T : class, IEvent
    {
        Task ProcessEvent(string serializedEvent);
    }
}
