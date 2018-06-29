using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlogBot.Configuration;
using PlogBot.Data;
using PlogBot.Listening.Interfaces;
using System;
using System.IO;

namespace PlogBot.App
{
    public class Program
    {
        public static IConfigurationRoot Configuration { get; set; }

        public static void Main(string[] args)
        {
            var env = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddEnvironmentVariables();

            if (string.IsNullOrEmpty(env) || env.ToLower().Equals("development"))
            {
                builder.AddUserSecrets<Program>();
            }

            Configuration = builder.Build();

            var provider = new ServiceCollection()
                .Configure<AppSettings>(Configuration)
                .AddOptions()
                .AddPlogBotServices()
                .BuildServiceProvider();

            using (var scope = provider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<PlogDbContext>();
                dbContext.Database.Migrate();
            }
            var listener = provider.GetService<IListener>();

            Console.WriteLine("Waiting for events from the Ploggystyle server...");
            listener.Listen().Wait();
            Console.WriteLine("Done!");
        }
    }
}
