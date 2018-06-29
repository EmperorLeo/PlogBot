using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PlogBot.Processing.Events;
using PlogBot.Processing.Interfaces;
using PlogBot.Services.Constants;
using PlogBot.Services.DiscordObjects;
using PlogBot.Services.Interfaces;

namespace PlogBot.Processing.DispatchEventProcessors
{
    public class GuildMemberAddProcessor : IEventProcessor<GuildMemberAdd>
    {
        private readonly IMessageService _messageService;
        private readonly IGuildService _guildService;
        public GuildMemberAddProcessor(IMessageService messageService, IGuildService guildService)
        {
            _messageService = messageService;
            _guildService = guildService;
        }

        public async Task ProcessEvent(string serializedEvent)
        {
            var @event = new GuildMemberAdd
            {
                Member = JsonConvert.DeserializeObject<Member>(serializedEvent)
            };

            // Guild ID should always exist in this processor
            var guild = await _guildService.GetGuild(@event.Member.GuildId.Value);
            if (!guild.SystemChannelId.HasValue)
            {
                return;
            }

            var embed = new Embed
            {
                Title = "Welcome Commands",
                Description = "Please use the following commands to attach your blade and soul characters to your discord account. Remember to surround multi-word names with double quotes.",
                Timestamp = DateTime.UtcNow,
                Color = HexConstants.Green,
                Footer = new EmbedItem
                {
                    IconUrl = EmojiConstants.PlogUrl,
                    Text = "PlogBot"
                },
                Fields = new List<EmbedField>
                {
                    new EmbedField
                    {
                        Name = "Add your main",
                        Value = "!plog me [characterName]"
                    },
                    new EmbedField
                    {
                        Name = "Add an alt",
                        Value = "!plog alt [characterName]"
                    },
                    new EmbedField
                    {
                        Name = "Disassociate a character with your discord account",
                        Value = "!plog release [characterName]"
                    }
                }
            };
            await _messageService.SendMessage(guild.SystemChannelId.Value, new OutgoingMessage
            {
                Content = $"Welcome to Ploggystyle, <@{@event.Member.User.Id}>!",
                Embed = embed
            });
        }
    }
}