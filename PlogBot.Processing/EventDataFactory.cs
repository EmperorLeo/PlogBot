using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using PlogBot.Processing.Enums;
using PlogBot.Processing.EventData;
using PlogBot.Processing.Interfaces;

namespace PlogBot.Processing
{
    public class EventDataFactory : IEventDataFactory
    {
        //private readonly Dictionary<GatewayOpCode, Type> _typeDict;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public EventDataFactory(IServiceScopeFactory serviceScopeFactory)
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

        public IEventData BuildEventData(IServiceScope scope, int opcode, string data)
        {
            var opcodeVal = (GatewayOpCode)opcode;
            IEventData serialized = null;
            switch (opcodeVal)
            {
                case GatewayOpCode.Dispatch:
                    var dispatchEventData = scope.ServiceProvider.GetRequiredService<IDispatchEventData>();
                    dispatchEventData.Initialize(data);
                    serialized = dispatchEventData;
                    break;
                case GatewayOpCode.Hello:
                    serialized = JsonConvert.DeserializeObject<Hello>(data);
                    break;
                case GatewayOpCode.HeartbeatACK:
                    serialized = new HeartbeatAck();
                    break;
                case GatewayOpCode.InvalidSession:
                    serialized = new InvalidSession();
                    break;
                default:
                    break;
            }
            return serialized;
        }
    }
}
