using System;
using System.Threading.Tasks;
using PlogBot.Services.Interfaces;

namespace PlogBot.Services
{
    public class LoggingService : ILoggingService
    {
        public Task LogAsync(string log)
        {
            Console.WriteLine(log);
            return Task.CompletedTask;
        }

        public Task LogErrorAsync(string error)
        {
            Console.Error.WriteLine(error);
            return Task.CompletedTask;
        }
    }
}
