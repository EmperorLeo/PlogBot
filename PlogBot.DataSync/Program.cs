using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlogBot.Configuration;
using PlogBot.Services;
using PlogBot.Services.Interfaces;
using System;
using System.Threading;

namespace PlogBot.DataSync
{
    public class Program
    {
        public static ILoggingService _loggingService;
        public static IBladeAndSoulService _bladeAndSoulService;
        public static IWebhookService _webhookService;
        public static SemaphoreSlim _lock;

        public static void Main(string[] args)
        {
            var env = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");

            var builder = new ConfigurationBuilder()
                .AddEnvironmentVariables();

            var configuration = builder.Build();

            var provider = new ServiceCollection()
                .Configure<AppSettings>(configuration)
                .AddOptions()
                .AddSingleton<DataSyncProcessor>()
                .AddSingleton<ILoggingService, LoggingService>()
                .AddSingleton<IBladeAndSoulService, BladeAndSoulService>()
                .AddSingleton<IWebhookService, WebhookService>()
                .AddSingleton<IPowerService, PowerService>()
                .BuildServiceProvider();


            var processor = provider.GetService<DataSyncProcessor>();

            processor.Run().Wait();
        }
    }
}
