using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlogBot.Configuration;
using PlogBot.Data;
using PlogBot.OAuth2;
using PlogBot.OAuth2.Interfaces;
using PlogBot.Services;
using PlogBot.Services.Interfaces;

namespace PlogBot.Alerts
{
    public class Program
    {
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
                .AddSingleton<AlertsProcessor>()
                .AddSingleton<ILoggingService, LoggingService>()
                .AddSingleton<IMessageService, MessageService>()
                .AddTransient<IAlertService, AlertService>()
                .AddTransient<ITimeZoneService, TimeZoneService>()
                .AddSingleton<IDiscordApiClient, DiscordApiClient>()
                .AddDbContext<PlogDbContext>(options => options.UseSqlite(sqliteFilePath))
                .BuildServiceProvider();

            while (true)
            {
                var scope = provider.CreateScope();
                var processor = scope.ServiceProvider.GetRequiredService<AlertsProcessor>();
                try
                {
                    processor.Process().Wait();
                }
                catch (Exception ex)
                {
                    var loggingService = scope.ServiceProvider.GetRequiredService<ILoggingService>();
                    loggingService.LogErrorAsync(ex.StackTrace).Wait();
                }
                Task.Delay(10000).Wait();
            }
        }
    }
}
