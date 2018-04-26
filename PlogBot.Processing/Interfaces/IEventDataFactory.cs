using Microsoft.Extensions.DependencyInjection;

namespace PlogBot.Processing.Interfaces
{
    public interface IEventDataFactory
    {
        IEventData BuildEventData(IServiceScope scope, int opcode, string data);
    }
}
