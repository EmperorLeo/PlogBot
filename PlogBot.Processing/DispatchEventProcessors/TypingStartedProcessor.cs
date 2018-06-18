using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PlogBot.Processing.Events;
using PlogBot.Processing.Interfaces;
using PlogBot.Services.DiscordObjects;
using PlogBot.Services.Interfaces;

namespace PlogBot.Processing.DispatchEventProcessors
{
    public class TypingStartedProcessor : IEventProcessor<TypingStarted>
    {
        private ILoggingService _loggingService;
        private IMessageService _messageService;
        private TypingStarted _event;

        public TypingStartedProcessor(ILoggingService loggingService, IMessageService messageService)
        {
            _loggingService = loggingService;
            _messageService = messageService;
        }

        public Task ProcessEvent(string serializedEvent)
        {
            // _event = JsonConvert.DeserializeObject<TypingStarted>(serializedEvent);
            // if (_event.UserId != LeoTestingDiscordId)
            // {
            //     return Task.CompletedTask;
            // }

            // var random = new Random();
            // var num = random.Next(100);

            // if (num < 50)
            // {
            //     return _messageService.SendMessage(_event.ChannelId, new OutgoingMessage
            //     {
            //         Content = "No."
            //     });
            // }

            return Task.CompletedTask;
        }
    }
}