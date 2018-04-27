using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PlogBot.Data;
using PlogBot.Data.Entities;
using PlogBot.Processing.Events;
using PlogBot.Processing.Interfaces;
using PlogBot.Services.Constants;
using PlogBot.Services.DiscordObjects;
using PlogBot.Services.Extensions;
using PlogBot.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace PlogBot.Processing.DispatchEventProcessors
{
    public class MessageCreateProcessor : IEventProcessor<MessageCreate>
    {
        private readonly IMessageService _messageService;
        private readonly IUserService _userService;
        private readonly IList<string> _allowedTopLevelCommands;
        private readonly IList<string> _adminCommands;
        private readonly PlogDbContext _plogDbContext;
        private readonly IBladeAndSoulService _bladeAndSoulService;
        private readonly ILoggingService _loggingService;

        private MessageCreate _event;
        private string _response;

        public MessageCreateProcessor(
            IMessageService messageService,
            IUserService userService,
            PlogDbContext plogDbContext,
            IBladeAndSoulService bladeAndSoulService,
            ILoggingService loggingService
        )
        {
            _plogDbContext = plogDbContext;
            _messageService = messageService;
            _userService = userService;
            _bladeAndSoulService = bladeAndSoulService;
            _loggingService = loggingService;
            _allowedTopLevelCommands = new List<string> { "test", "add", "me", "alt", "release", "characters" };
            _adminCommands = new List<string> { "reset" };
            _response = "There was an error processing this request.";
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
                case "characters":
                    await ProcessCharacters(cmdArguments);
                    break;
                default:

                    break;
            }
        }

        private Task ProcessTest()
        {
            return _messageService.SendMessage(_event.Message.ChannelId, new OutgoingMessage
            {
                Content = "Successful Command"
            });
        }

        private async Task ProcessAdd(List<string> args)
        {
            if (args.Count == 1)
            {
                var name = args[0];
                var plog = await _plogDbContext.Plogs.Where(p => p.Name.ToLower() == name.ToLower()).FirstOrDefaultAsync();
                if (plog == null)
                {
                    var character = await _bladeAndSoulService.GetBladeAndSoulCharacter(name);
                    if (character == null || character.Clan.ToLower() != "ploggystyle")
                    {
                        _response = $"{name} is not in Ploggystyle.";
                    }
                    else
                    {
                        plog = new ClanMember
                        {
                            RealName = character.AccountName,
                            Name = name,
                            Active = true,
                            Class = character.Class,
                            Created = DateTime.UtcNow,
                            ImageUrl = character.ProfileImageUrl
                        };
                        _plogDbContext.Add(plog);
                        await _plogDbContext.SaveChangesAsync();
                        _response = $"{name} has been added to the clan.";
                    }
                }
                else
                {
                    _response = "This plog already exists!";
                    if (plog.DiscordId.HasValue && plog.DiscordId.Value != _event.Message.Author.Id)
                    {
                        _response += $" <@{plog.DiscordId}> has claimed this character.";
                    }
                }
            }
            else
            {
                _response = "Incorrect command format: !plog add [name]";
            }

            await _messageService.SendMessage(_event.Message.ChannelId, new OutgoingMessage
            {
                Content = _response
            });
        }

        private async Task ProcessMe(List<string> args)
        {
            if (args.Count == 1)
            {
                var name = args[0];
                var plog = await _plogDbContext.Plogs.Where(p => p.Name.ToLower() == name.ToLower()).FirstOrDefaultAsync();
                if (plog == null)
                {
                    var character = await _bladeAndSoulService.GetBladeAndSoulCharacter(name);
                    if (character == null || character.Clan.ToLower() != "ploggystyle")
                    {
                        _response = $"You are not in Ploggystyle.";
                    }
                    else
                    {
                        plog = new ClanMember
                        {
                            RealName = character.AccountName,
                            Name = name,
                            Active = true,
                            DiscordId = _event.Message.Author.Id,
                            Class = character.Class,
                            Created = DateTime.UtcNow,
                            ImageUrl = character.ProfileImageUrl
                        };
                        _plogDbContext.Add(plog);
                        await _plogDbContext.SaveChangesAsync();
                        _response = $"Welcome to Ploggystyle, {name}!";
                    }
                }
                else
                {
                    if (!plog.DiscordId.HasValue)
                    {
                        plog.DiscordId = _event.Message.Author.Id;
                        await _plogDbContext.SaveChangesAsync();
                        _response = $"You have claimed {name}";
                    }
                    else if (plog.DiscordId.HasValue && plog.DiscordId.Value != _event.Message.Author.Id)
                    {
                        _response = $"This plog already exists! <@{plog.DiscordId}> has claimed this character.";
                    }
                    else
                    {
                        _response = $"You have already claimed {name}";
                    }
                }
            }
            else
            {
                _response = "Incorrect command format: !plog me [name]";
            }

            await _messageService.SendMessage(_event.Message.ChannelId, new OutgoingMessage
            {
                Content = _response
            });
        }

        private async Task ProcessAlt(List<string> args)
        {
            var name = args[0];
            var main = await _plogDbContext.Plogs.Where(p => p.DiscordId == _event.Message.Author.Id).FirstOrDefaultAsync();
            var alt = await _plogDbContext.Plogs.Where(p => p.Name.ToLower() == name.ToLower()).FirstOrDefaultAsync();
            if (main == null)
            {
                _response = $"You need to set up your main first.";
            }
            else if (alt == null)
            {
                var character = await _bladeAndSoulService.GetBladeAndSoulCharacter(name);
                if (character == null || character.Clan.ToLower() != "ploggystyle")
                {
                    _response = $"Your alt is not in Ploggystyle.";
                }
                else
                {
                    alt = new ClanMember
                    {
                        RealName = character.AccountName,
                        Name = name,
                        Active = true,
                        DiscordId = _event.Message.Author.Id,
                        Class = character.Class,
                        Created = DateTime.UtcNow,
                        Main = main,
                        ImageUrl = character.ProfileImageUrl
                    };
                    _plogDbContext.Add(alt);
                    await _plogDbContext.SaveChangesAsync();
                    _response = $"Setup alt character {name}!";
                }
            }
            else
            {
                if (!alt.DiscordId.HasValue)
                {
                    alt.DiscordId = _event.Message.Author.Id;
                    await _plogDbContext.SaveChangesAsync();
                    _response = $"Attached alt character {name}";
                }
                else if (alt.DiscordId.HasValue && alt.DiscordId.Value != _event.Message.Author.Id)
                {
                    _response = $"This plog already exists! <@{alt.DiscordId}> has claimed this character.";
                }
                else
                {
                    _response = $"You have already claimed {name}";
                }
            }
            await _messageService.SendMessage(_event.Message.ChannelId, new OutgoingMessage
            {
                Content = _response
            });
        }

        private async Task ProcessRelease(List<string> args)
        {
            if (args.Count == 1)
            {
                var name = args[0];
                var plog = await _plogDbContext.Plogs.Where(p => p.Name.ToLower() == name.ToLower()).FirstOrDefaultAsync();
                if (plog.DiscordId == _event.Message.Author.Id)
                {
                    plog.DiscordId = null;
                    await _plogDbContext.SaveChangesAsync();
                    _response = $"Successfully disassociated {name} from your discord account.";
                }
                else
                {
                    _response = $"{name} is not your character.";
                }

            }
            await _messageService.SendMessage(_event.Message.ChannelId, new OutgoingMessage
            {
                Content = _response
            });
        }

        private async Task ProcessReset(List<string> args)
        {
            await _plogDbContext.Database.EnsureDeletedAsync();
            await _plogDbContext.Database.MigrateAsync();
        }

        private async Task ProcessCharacters(List<string> args)
        {
            if (args.Count != 1 || !Regex.IsMatch(args[0], RegexConstants.MentionRegex))
            {
                await _messageService.SendMessage(_event.Message.ChannelId, new OutgoingMessage
                {
                    Content = "Incorrect command format: !plog me [@discordusername]"
                });
                return;
            }

            var mentionId = args[0].StripMentionExtras();
            var characters = await _plogDbContext.Plogs.Where(p => p.DiscordId == mentionId).OrderBy(u => u.MainId).ToListAsync();

            if (characters.Count == 0)
            {
                await _messageService.SendMessage(_event.Message.ChannelId, new OutgoingMessage
                {
                    Content = "That user doesn't have any characters in Ploggystyle."
                });
                return;
            }

            var user = await _userService.GetUser(mentionId);

            var fields = new List<EmbedField>();
            characters.ForEach(c =>
            {
                fields.Add(new EmbedField
                {
                    Name = $"{_bladeAndSoulService.GetClassEmojiByClass(c.Class)} {c.Name}",
                    Value = "TODO: Power level summary here."
                });
            });

            var main = characters.Where(c => c.MainId == null).FirstOrDefault();
            var embed = new Embed
            {
                Title = "Blade and Soul Characters",
                Url = $"http://na-bns.ncsoft.com/ingame/bs/character/search/info?c={HttpUtility.UrlEncode(main.Name)}",
                Timestamp = DateTime.UtcNow,
                Color = HexConstants.Green,
                Thumbnail = new EmbedItem
                {
                    Url = main.ImageUrl
                },
                Author = new EmbedItem
                {
                    Name = main.RealName,
                    Url = $"http://na-bns.ncsoft.com/ingame/bs/character/profile?c={HttpUtility.UrlEncode(main.Name)}",
                    IconUrl = user.Avatar
                },
                Footer = new EmbedItem
                {
                    IconUrl = EmojiConstants.PlogUrl,
                    Text = "PlogBot"
                },
                Fields = fields
            };

            await _messageService.SendMessage(_event.Message.ChannelId, new OutgoingMessage
            {
                Content = args[0],
                Embed = embed
            });
        }
    }
}
