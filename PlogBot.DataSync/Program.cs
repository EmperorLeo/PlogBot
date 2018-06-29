using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlogBot.Configuration;
using PlogBot.Data;
using PlogBot.Services;
using PlogBot.Services.Interfaces;
using System;
using System.IO;
using System.Runtime.InteropServices;
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

            var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            var sqliteFilePath = Path.Combine(Environment.GetEnvironmentVariable(isWindows ? "LocalAppData" : "HOME"), isWindows ? @"PlogBot\plog.db" : ".plogbot/plog.db");

            var provider = new ServiceCollection()
                .Configure<AppSettings>(configuration)
                .AddOptions()
                .AddSingleton<DataSyncProcessor>()
                .AddSingleton<ILoggingService, LoggingService>()
                .AddSingleton<IBladeAndSoulService, BladeAndSoulService>()
                .AddSingleton<IWebhookService, WebhookService>()
                .AddSingleton<IPowerService, PowerService>()
                .AddDbContext<PlogDbContext>(options => options.UseSqlite(sqliteFilePath))
                .BuildServiceProvider();

            var processor = provider.GetService<DataSyncProcessor>();

            processor.Run().Wait();
        }
    }
}
