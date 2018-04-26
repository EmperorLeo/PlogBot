using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PlogBot.Data;
using PlogBot.Listening;
using PlogBot.Listening.Interfaces;
using PlogBot.OAuth2;
using PlogBot.OAuth2.Interfaces;
using PlogBot.Processing;
using PlogBot.Processing.EventData;
using PlogBot.Processing.EventProcessors;
using PlogBot.Processing.Events;
using PlogBot.Processing.Interfaces;
using PlogBot.Services;
using PlogBot.Services.Interfaces;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace PlogBot.App
{
    public static class PlogBotServiceCollectionDefinition
    {
        public static IServiceCollection AddPlogBotServices(this IServiceCollection services)
        {
            // Listening
            services.AddSingleton<IListener, Listener>();

            // API Services
            services.AddSingleton<IGatewayService, GatewayService>();
            services.AddSingleton<IUtilityService, UtilityService>();
            services.AddSingleton<IMessageService, MessageService>();
            services.AddSingleton<IBladeAndSoulService, BladeAndSoulService>();
            services.AddSingleton<IUserService, UserService>();

            // HttpClient Singletons
            services.AddSingleton<IDiscordApiClient, DiscordApiClient>();

            // Processing
            services.AddSingleton<IEventDataFactory, EventDataFactory>();
            services.AddSingleton<IPayloadProcessor, PayloadProcessor>();
            services.AddScoped<IDispatchEventData, DispatchEventData>();
            services.AddScoped<IEventProcessor<MessageCreate>, MessageCreateProcessor>();

            // DB
            var sqliteFilePath = Path.Combine(Environment.GetEnvironmentVariable(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "LocalAppData" : "Home"), @"PlogBot\plog.db");
            services.AddDbContext<PlogDbContext>(options => options.UseSqlite(sqliteFilePath));

            return services;
        }
    }
}
