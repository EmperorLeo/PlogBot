﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PlogBot.Data;
using PlogBot.Listening;
using PlogBot.Listening.Interfaces;
using PlogBot.OAuth2;
using PlogBot.OAuth2.Interfaces;
using PlogBot.Processing;
using PlogBot.Processing.DispatchEventProcessors;
using PlogBot.Processing.EventDataServices;
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

            // Logging
            services.AddSingleton<ILoggingService, LoggingService>();

            // Application Services
            services.AddSingleton<IGatewayService, GatewayService>();
            services.AddSingleton<IUtilityService, UtilityService>();
            services.AddSingleton<IMessageService, MessageService>();
            services.AddSingleton<IBladeAndSoulService, BladeAndSoulService>();
            services.AddSingleton<IUserService, UserService>();
            services.AddTransient<IClanLogService, ClanLogService>();
            services.AddSingleton<IRaffleService, RaffleService>();
            services.AddTransient<IAlertService, AlertService>();
            services.AddTransient<ITimeZoneService, TimeZoneService>();
            services.AddSingleton<IGuildService, GuildService>();
            services.AddTransient<IPowerService, PowerService>();

            // HttpClient Singletons
            services.AddSingleton<IDiscordApiClient, DiscordApiClient>();

            // Processing
            services.AddSingleton<IEventDataServiceFactory, EventDataServiceFactory>();
            services.AddSingleton<IPayloadProcessor, PayloadProcessor>();
            services.AddSingleton<ISessionService, SessionService>();
            services.AddScoped<IHelloEventDataService, HelloEventDataService>();
            services.AddScoped<IInvalidSessionEventDataService, InvalidSessionEventDataService>();
            services.AddScoped<IHeartbeatAckEventDataService, HeartbeatAckEventDataService>();
            services.AddScoped<IDispatchEventDataService, DispatchEventDataService>();
            services.AddScoped<IUnimplementedEventDataService, UnimplementedEventDataService>();
            services.AddScoped<IEventProcessor<MessageCreate>, MessageCreateProcessor>();
            services.AddScoped<IEventProcessor<TypingStarted>, TypingStartedProcessor>();
            services.AddScoped<IEventProcessor<GuildMemberAdd>, GuildMemberAddProcessor>();

            // DB
            var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            var sqliteFilePath = Path.Combine(Environment.GetEnvironmentVariable(isWindows ? "LocalAppData" : "HOME"), isWindows ? @"PlogBot\plog.db" : ".plogbot/plog.db");
            services.AddDbContext<PlogDbContext>(options => options.UseSqlite(sqliteFilePath));

            return services;
        }
    }
}
