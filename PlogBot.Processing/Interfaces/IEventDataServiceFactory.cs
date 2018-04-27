using Microsoft.Extensions.DependencyInjection;

namespace PlogBot.Processing.Interfaces
{
    public interface IEventDataServiceFactory
    {
        IEventDataService BuildEventDataService(IServiceScope scope, int opcode);
    }
}
