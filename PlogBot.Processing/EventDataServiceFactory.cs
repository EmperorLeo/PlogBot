using Microsoft.Extensions.DependencyInjection;
using PlogBot.Processing.Enums;
using PlogBot.Processing.Interfaces;
using System.Linq;

namespace PlogBot.Processing
{
    public class EventDataServiceFactory : IEventDataServiceFactory
    {
        //private readonly Dictionary<GatewayOpCode, Type> _typeDict;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public EventDataServiceFactory(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            //_typeDict = new Dictionary<GatewayOpCode, Type>
            //{
            //    { GatewayOpCode.Dispatch, typeof(object) },
            //    { GatewayOpCode.Heartbeat, typeof(object) },
            //    { GatewayOpCode.Identify, typeof(object) },
            //    { GatewayOpCode.StatusUpdate, typeof(object) },
            //    { GatewayOpCode.VoiceStatusUpdate, typeof(object) },
            //    { GatewayOpCode.VoiceServerPing, typeof(object) },
            //    { GatewayOpCode.Resume, typeof(object) },
            //    { GatewayOpCode.Reconnect, typeof(object) },
            //    { GatewayOpCode.RequestGuildMembers, typeof(object) },
            //    { GatewayOpCode.InvalidSession, typeof(object) },
            //    { GatewayOpCode.Hello, typeof(Hello) },
            //    { GatewayOpCode.HeartbeatACK, typeof(object) }
            //};
        }

        public IEventDataService BuildEventDataService(IServiceScope scope, int opcode)
        {
            var opcodeVal = (GatewayOpCode)opcode;
            IEventDataService eventDataService;
            switch (opcodeVal)
            {
                case GatewayOpCode.Dispatch:
                    eventDataService = scope.ServiceProvider.GetRequiredService<IDispatchEventDataService>();
                    break;
                case GatewayOpCode.Hello:
                    eventDataService = scope.ServiceProvider.GetRequiredService<IHelloEventDataService>();
                    break;
                case GatewayOpCode.HeartbeatACK:
                    eventDataService = scope.ServiceProvider.GetRequiredService<IHeartbeatAckEventDataService>();
                    break;
                case GatewayOpCode.InvalidSession:
                    eventDataService = scope.ServiceProvider.GetRequiredService<IInvalidSessionEventDataService>();
                    break;
                default:
                    eventDataService = scope.ServiceProvider.GetRequiredService<IUnimplementedEventDataService>();
                    break;
            }

            return eventDataService;
        }
    }
}
