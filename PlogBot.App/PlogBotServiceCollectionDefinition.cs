using Microsoft.Extensions.DependencyInjection;
using PlogBot.Listening;
using PlogBot.Listening.Interfaces;
using PlogBot.OAuth2;
using PlogBot.OAuth2.Interfaces;
using PlogBot.Processing;
using PlogBot.Processing.EventData;
using PlogBot.Processing.Interfaces;
using PlogBot.Services;
using PlogBot.Services.Interfaces;

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

            // HttpClient Singletons
            services.AddSingleton<IDiscordApiClient, DiscordApiClient>();

            // Processing
            services.AddSingleton<IEventDataFactory, EventDataFactory>();
            services.AddSingleton<IPayloadProcessor, PayloadProcessor>();
            services.AddScoped<IDispatchEventData, DispatchEventData>();

            return services;
        }
    }
}
