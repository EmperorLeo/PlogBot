using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PlogBot.Data;
using PlogBot.Data.Entities;
using PlogBot.Processing.Events;
using PlogBot.Processing.Interfaces;
using PlogBot.Services.DiscordObjects;
using PlogBot.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlogBot.Processing.EventProcessors
{
    public class MessageCreateProcessor : IEventProcessor<MessageCreate>
    {
        private readonly IMessageService _messageService;
        private readonly IUserService _userService;
        private readonly IList<string> _allowedTopLevelCommands;
        private readonly IList<string> _adminCommands;
        private readonly PlogDbContext _plogDbContext;
        private readonly IBladeAndSoulService _bladeAndSoulService;

        private MessageCreate _event;

        public MessageCreateProcessor(IMessageService messageService, IUserService userService, PlogDbContext plogDbContext, IBladeAndSoulService bladeAndSoulService)
        {
            _plogDbContext = plogDbContext;
            _messageService = messageService;
            _userService = userService;
            _bladeAndSoulService = bladeAndSoulService;
            _allowedTopLevelCommands = new List<string> { "test", "add", "me", "alt", "release" };
            _adminCommands = new List<string> { "reset" };
        }

        public async Task ProcessEvent(string serializedEvent)
        {
            _event = new MessageCreate
            {
                Message = JsonConvert.DeserializeObject<Message>(serializedEvent)
            };

            var message = _event.Message;
            var content = message.Content;
            if (content.StartsWith("!plog "))
            {
                var commandArgs = content.Split('"').Select((element, index) =>
                {
                    return index % 2 == 0 ? element.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries) : new string[] { element };
                }).SelectMany(element => element).ToList();
                if (commandArgs.Count > 1 && (_allowedTopLevelCommands.Contains(commandArgs[1]) || (message.Author.Id == 123979419374059521 && _adminCommands.Contains(commandArgs[1]))))
                {
                    await ProcessEventInternal(commandArgs);
                }
            }
        }

        private async Task ProcessEventInternal(List<string> args)
        {
            var cmd = args[1];
            var cmdArguments = args.Where((item, index) => index > 1).ToList();
            switch (cmd)
            {
                case "test":
                    await ProcessTest();
                    break;
                case "add":
                    await ProcessAdd(cmdArguments);
                    break;
                case "me":
                    await ProcessMe(cmdArguments);
                    break;
                case "alt":
                    await ProcessAlt(cmdArguments);
                    break;
                case "release":
                    await ProcessRelease(cmdArguments);
                    break;
                case "reset":
                    await ProcessReset(cmdArguments);
                    break;
                default:

                    break;
            }
        }

        private Task ProcessTest()
        {
            return _messageService.SendMessage(_event.Message.ChannelId, "Successful command");
        }

        private async Task ProcessAdd(List<string> args)
        {
            var message = "There was an error processing this request.";
            if(args.Count == 1)
            {
                var name = args[0];
                var plog = await _plogDbContext.Plogs.Where(p => p.Name.ToLower() == name.ToLower()).FirstOrDefaultAsync();
                if (plog == null)
                {
                    var character = await _bladeAndSoulService.GetBladeAndSoulCharacter(name);
                    if (character == null || character.Clan.ToLower() != "ploggystyle")
                    {
                        message = $"{name} is not in Ploggystyle.";
                    }
                    else
                    {
                        plog = new ClanMember
                        {
                            Name = name,
                            Active = true,
                            Class = character.Class,
                            Created = DateTime.UtcNow
                        };
                        _plogDbContext.Add(plog);
                        await _plogDbContext.SaveChangesAsync();
                        message = $"{name} has been added to the clan.";
                    }
                }
                else
                {
                    message = "This plog already exists!";
                    if (plog.DiscordId.HasValue && plog.DiscordId.Value != _event.Message.Author.Id)
                    {
                        message += $" <@{plog.DiscordId}> has claimed this character.";
                    }
                }
            }
            else
            {
                message = "Incorrect command format: !plog add [name]";
            }

            await _messageService.SendMessage(_event.Message.ChannelId, message);
        }

        private async Task ProcessMe(List<string> args)
        {
            var message = "There was an error processing this request";
            if (args.Count == 1)
            {
                var name = args[0];
                var plog = await _plogDbContext.Plogs.Where(p => p.Name.ToLower() == name.ToLower()).FirstOrDefaultAsync();
                if (plog == null)
                {
                    var character = await _bladeAndSoulService.GetBladeAndSoulCharacter(name);
                    if (character == null || character.Clan.ToLower() != "ploggystyle")
                    {
                        message = $"You are not in Ploggystyle.";
                    }
                    else
                    {
                        plog = new ClanMember
                        {
                            Name = name,
                            Active = true,
                            DiscordId = _event.Message.Author.Id,
                            Class = character.Class,
                            Created = DateTime.UtcNow
                        };
                        _plogDbContext.Add(plog);
                        await _plogDbContext.SaveChangesAsync();
                        message = $"Welcome to Ploggystyle, {name}!";
                    }
                }
                else
                {
                    if (!plog.DiscordId.HasValue)
                    {
                        plog.DiscordId = _event.Message.Author.Id;
                        await _plogDbContext.SaveChangesAsync();
                        message = $"You have claimed {name}";
                    }
                    else if (plog.DiscordId.HasValue && plog.DiscordId.Value != _event.Message.Author.Id)
                    {
                        message += $"This plog already exists! <@{plog.DiscordId}> has claimed this character.";
                    }
                    else
                    {
                        message = $"You have already claimed {name}";
                    }
                }
            }
            else
            {
                message = "Incorrect command format: !plog me [name]";
            }

            await _messageService.SendMessage(_event.Message.ChannelId, message);
        }

        private async Task ProcessAlt(List<string> args)
        {

        }

        private async Task ProcessRelease(List<string> args)
        {
            
        }

        private async Task ProcessReset(List<string> args)
        {
            await _plogDbContext.Database.EnsureDeletedAsync();
            await _plogDbContext.Database.MigrateAsync();
        }
    }
}
