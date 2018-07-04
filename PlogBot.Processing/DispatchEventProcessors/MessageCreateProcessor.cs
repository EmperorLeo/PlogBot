using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PlogBot.Data;
using PlogBot.Data.Entities;
using PlogBot.Processing.Events;
using PlogBot.Processing.Interfaces;
using PlogBot.Services.Constants;
using PlogBot.Services.DiscordObjects;
using PlogBot.Services.Exceptions;
using PlogBot.Services.Extensions;
using PlogBot.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private readonly IList<string> _bannedKashPhrases;
        private readonly PlogDbContext _plogDbContext;
        private readonly IBladeAndSoulService _bladeAndSoulService;
        private readonly ILoggingService _loggingService;
        private readonly IClanLogService _clanLogService;
        private readonly IRaffleService _raffleService;
        private readonly IAlertService _alertService;
        private readonly ITimeZoneService _timeZoneService;
        private readonly IPowerService _powerService;
        private readonly IGuildService _guildService;

        private MessageCreate _event;
        private string _response;

        public MessageCreateProcessor(
            IMessageService messageService,
            IUserService userService,
            PlogDbContext plogDbContext,
            IBladeAndSoulService bladeAndSoulService,
            ILoggingService loggingService,
            IClanLogService clanLogService,
            IRaffleService raffleService,
            IAlertService alertService,
            ITimeZoneService timeZoneService,
            IPowerService powerService,
            IGuildService guildService
        )
        {
            _plogDbContext = plogDbContext;
            _messageService = messageService;
            _userService = userService;
            _bladeAndSoulService = bladeAndSoulService;
            _loggingService = loggingService;
            _clanLogService = clanLogService;
            _raffleService = raffleService;
            _alertService = alertService;
            _timeZoneService = timeZoneService;
            _powerService = powerService;
            _guildService = guildService;
            _allowedTopLevelCommands = new List<string> { "test", "add", "me", "alt", "release", "characters", "whales", "clanlog", "raffle", "ticket", "alert", "mytime" };
            _adminCommands = new List<string> { "reset" };
            _bannedKashPhrases = new List<string> { "no", "nope", "nah", "nada", "n0", "n0pe" };
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
            else if (_event.Message.Author.Id == DiscordUserConstants.KashId && _bannedKashPhrases.Contains(Regex.Replace(_event.Message.Content, "\\.?\\??\\!?", "").Trim().ToLower()))
            {
                await _messageService.DeleteMessageAsync(_event.Message.ChannelId, _event.Message.Id);
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
                case "whales":
                    await ProcessWhales(cmdArguments);
                    break;
                case "clanlog":
                    await ProcessClanLog(cmdArguments);
                    break;
                case "raffle":
                    await ProcessRaffle(cmdArguments);
                    break;
                case "ticket":
                    await ProcessTicket(cmdArguments);
                    break;
                case "alert":
                    await ProcessAlert(cmdArguments);
                    break;
                case "mytime":
                    await ProcessMyTime(cmdArguments);
                    break;
                default:
                    break;
            }
        }

        private Task ProcessTest()
        {
            var timeZones = TimeZoneInfo.GetSystemTimeZones();
            var message = "";
            timeZones.ToList().ForEach(x => message += $"DN: {x.DisplayName} ID: {x.Id} \n");
            return _messageService.SendMessage(_event.Message.ChannelId, new OutgoingMessage
            {
                Content = message
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
                    alt.Main = main;
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
            if (args.Count != 1 || !Regex.IsMatch(args[0], RegexConstants.MentionRegex, RegexOptions.CultureInvariant))
            {
                await _messageService.SendMessage(_event.Message.ChannelId, new OutgoingMessage
                {
                    Content = "Incorrect command format: !plog characters [@discordusername]"
                });
                return;
            }

            var mentionId = args[0].StripMentionExtras();
            var characters = await _plogDbContext.Plogs.Where(p => p.DiscordId == mentionId).OrderBy(u => u.MainId).ToListAsync();
            var powers = (await _powerService.GetWhaleScoresForUser(mentionId)).ToDictionary(x => x.Name, x => x);

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
                var power = "Whale Score: 0";
                if (powers.ContainsKey(c.Name))
                {
                    power = $"Whale Score: {powers[c.Name].Score}";
                }
                fields.Add(new EmbedField
                {
                    Name = $"{_bladeAndSoulService.GetClassEmojiByClass(c.Class)} {c.Name}",
                    Value = power
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
                    IconUrl = user.Avatar != null ? $"https://cdn.discordapp.com/avatars/{user.Id}/{user.Avatar}.jpg" : null
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

        private async Task ProcessWhales(List<string> args)
        {
            var numWhales = 5;
            if (args.Count >= 2 || args.Count == 1 && !int.TryParse(args[0], out numWhales))
            {
                await _messageService.SendMessage(_event.Message.ChannelId, new OutgoingMessage
                {
                    Content = "Incorrect command format: !plog whales [numWhales?]"
                });
                return;
            }

            var topWhales = await _powerService.GetWhales(numWhales);

            var fields = new List<EmbedField>();
            for (var i = 0; i < topWhales.Count; i++)
            {
                var whale = topWhales[i];
                fields.Add(new EmbedField
                {
                    Name = $"{_bladeAndSoulService.GetClassEmojiByClass(whale.CharacterClass)} {whale.Name}",
                    Value = $"#{i + 1} whale with a score of {whale.Score}\n\n\n\n\n"
                });
            }

            var embed = new Embed
            {
                Title = "Ploggystyle Whales",
                Timestamp = DateTime.UtcNow,
                Color = HexConstants.Green,
                //Thumbnail = new EmbedItem
                //{
                //    Url = main.ImageUrl
                //},
                //Author = new EmbedItem
                //{
                //    Name = main.RealName,
                //    Url = $"http://na-bns.ncsoft.com/ingame/bs/character/profile?c={HttpUtility.UrlEncode(main.Name)}",
                //    IconUrl = user.Avatar
                //},
                Footer = new EmbedItem
                {
                    IconUrl = EmojiConstants.PlogUrl,
                    Text = "PlogBot"
                },
                Fields = fields
            };

            await _messageService.SendMessage(_event.Message.ChannelId, new OutgoingMessage
            {
                Content = "Whales",
                Embed = embed
            });
        }

        private async Task ProcessClanLog(List<string> args)
        {
            var csv = await _clanLogService.GetCsv(DateTime.UtcNow.AddDays(-10));
            var dm = await _userService.OpenDm(_event.Message.Author.Id);
            await _messageService.SendMessageWithAttachment(dm.Id, new OutgoingMessage
            {
                Content = "Hi! Here are the last 10 days of clan logs.",
                File = Encoding.UTF8.GetBytes(csv)
            });
        }

        private async Task ProcessRaffle(List<string> args)
        {
            if (args.Count == 0)
            {
                await _messageService.SendMessage(_event.Message.ChannelId, new OutgoingMessage
                {
                    Content = "Incorrect command format: !plog raffle [start/end] [maxTickets?]"
                });
                return;

            }
            else
            {
                var arg = args[0].ToLower();
                var subArgs = args.Where((item, index) => index >= 1).ToList();
                switch (arg)
                {
                    case "start":
                        await ProcessRaffleStart(subArgs);
                        break;
                    case "end":
                        await ProcessRaffleEnd(subArgs);
                        break;
                    case "draw":
                        await ProcessRaffleDraw(subArgs);
                        break;
                    default:
                        await _messageService.SendMessage(_event.Message.ChannelId, new OutgoingMessage
                        {
                            Content = "Incorrect command format: !plog raffle [start/end] [maxTickets?]"
                        });
                        break;
                }
            }
        }

        private async Task ProcessRaffleStart(List<string> args)
        {
            var maxTickets = 0;
            if (args.Count >= 2 || args.Count == 1 && !int.TryParse(args[0], out maxTickets))
            {
                await _messageService.SendMessage(_event.Message.ChannelId, new OutgoingMessage
                {
                    Content = "Incorrect command format: !plog raffle [maxTickets?]"
                });
                return;
            }
            try 
            {
                await _raffleService.StartRaffle(maxTickets, _event.Message.ChannelId);
                _response = "Raffle started. Type '!plog ticket' to get a ticket.";
            }
            catch (RaffleException ex)
            {
                _response = ex.Message;
            }
            finally
            {
                await _messageService.SendMessage(_event.Message.ChannelId, new OutgoingMessage
                {
                    Content = _response
                });
            }
        }
        
        private async Task ProcessTicket(List<string> args)
        {
            var ticketGetter = _event.Message.Author.Id;
            try
            {
                await _raffleService.GetTicket(ticketGetter, _event.Message.ChannelId);
                _response = $"Successfully got <@{ticketGetter}> a ticket.";
            }
            catch (RaffleException ex)
            {
                _response = $"<@{ticketGetter}> could not get a ticket. {ex.Message}";
            }
            finally
            {
                await _messageService.SendMessage(_event.Message.ChannelId, new OutgoingMessage
                {
                    Content = _response
                });
            }
        }

        private async Task ProcessRaffleDraw(List<string> args)
        {
            try
            {
                var winner = await _raffleService.EndRaffle(_event.Message.ChannelId);
                if (winner != 0)
                {
                    _response = $":tada: RNGesus blesses you, child! Congratulations <@{winner}> :confetti_ball:";
                }
                else
                {
                    _response = $"There are no more participants.";
                }
            }
            catch (RaffleException ex)
            {
                _response = ex.Message;
            }
            finally
            {
                await _messageService.SendMessage(_event.Message.ChannelId, new OutgoingMessage
                {
                    Content = _response
                });
            }
        }

        private async Task ProcessRaffleEnd(List<string> args)
        {
            try
            {
                var winner = await _raffleService.EndRaffle(_event.Message.ChannelId);
                if (winner != 0)
                {
                    _response = $":tada: RNGesus blesses you, child! Congratulations <@{winner}> :confetti_ball:. ";
                }
                _response += "The raffle is over.";
            }
            catch (RaffleException ex)
            {
                _response = ex.Message;
            }
            finally
            {
                await _messageService.SendMessage(_event.Message.ChannelId, new OutgoingMessage
                {
                    Content = _response
                });
            }
        }

        private async Task ProcessAlert(List<string> args)
        {
            var topLevelAlertArgs = new [] { "create", "edit", "delete" };
            if (args.Count == 0 || !topLevelAlertArgs.Contains(args[0].ToLower()))
            {
                await SendAlertCommandEmbed();
                return;
            }

            if (!await _timeZoneService.HasTimeZoneSet(_event.Message.Author.Id))
            {
                await _messageService.SendMessage(_event.Message.ChannelId, new OutgoingMessage
                {
                    Content = "Please set your time zone first with: !plog mytime [timeZone]"
                });
                return;
            }

            var method = args[0].ToLower();
            args = args.Where((item, index) => index > 0).ToList();
            switch (method)
            {
                case "create":
                    await ProcessAlertCreate(args);
                    break;
                case "edit":
                    await ProcessAlertEdit(args);
                    break;
                default:
                    await ProcessAlertDelete(args);
                    break;
            }
        }

        private async Task ProcessAlertCreate(List<string> args)
        {
            // TODO: Refactor all this logic into some kind of shared method with process alert edit
            if (args.Count < 3)
            {
                await SendAlertCommandEmbed();
                return;
            }
            var weekdays = new [] { "sunday", "monday", "tuesday", "wednesday", "thursday", "friday", "saturday" };
            var title = args[0];
            var text = args[1];
            int? day = null;
            var time = 0;
            var rolesStartingArg = 3;
            if (weekdays.Contains(args[2].ToLower()))
            {
                day = weekdays.ToList().IndexOf(args[2].ToLower());
                rolesStartingArg = 4;
                if (args.Count < 4)
                {
                    // Specified a week but no time.
                    await SendAlertCommandEmbed();
                    return;
                }
                var stringTime = args[3];
                if (!Regex.IsMatch(stringTime, RegexConstants.TimeRegex))
                {
                    await SendAlertCommandEmbed();
                    return;
                }
                time = stringTime.ParseTime();
            }
            else
            {
                var stringTime = args[2];
                if (!Regex.IsMatch(stringTime, RegexConstants.TimeRegex))
                {
                    await SendAlertCommandEmbed();
                    return;
                }
                time = stringTime.ParseTime();
            }
            var roles = args
                .Where((item, index) => index >= rolesStartingArg && Regex.IsMatch(item, RegexConstants.MentionRegex))
                .Select(x => x.StripMentionExtras()).ToList();
            await _alertService.CreateAlert(title, time, day, text, roles, _event.Message.ChannelId, _event.Message.Author.Id);
            ReactRandomly();
        }

        private async Task ProcessAlertEdit(List<string> args)
        {
            if (args.Count < 3)
            {
                await SendAlertCommandEmbed();
                return;
            }
            var weekdays = new [] { "sunday", "monday", "tuesday", "wednesday", "thursday", "friday", "saturday" };
            var title = args[0];
            var text = args[1];
            int? day = null;
            var time = 0;
            var rolesStartingArg = 3;
            if (weekdays.Contains(args[2].ToLower()))
            {
                day = weekdays.ToList().IndexOf(args[2].ToLower());
                rolesStartingArg = 4;
                if (args.Count < 4)
                {
                    // Specified a week but no time.
                    await SendAlertCommandEmbed();
                    return;
                }
                var stringTime = args[3];
                if (!Regex.IsMatch(stringTime, RegexConstants.TimeRegex))
                {
                    await SendAlertCommandEmbed();
                    return;
                }
                time = stringTime.ParseTime();
            }
            else
            {
                var stringTime = args[2];
                if (!Regex.IsMatch(stringTime, RegexConstants.TimeRegex))
                {
                    await SendAlertCommandEmbed();
                    return;
                }
                time = stringTime.ParseTime();
            }
            var roles = args
                .Where((item, index) => index >= rolesStartingArg && Regex.IsMatch(item, RegexConstants.MentionRegex))
                .Select(x => x.StripMentionExtras()).ToList();
            await _alertService.ModifyAlert(title, time, day, text, roles, _event.Message.ChannelId);
            ReactRandomly();
        }

        private async Task ProcessAlertDelete(List<string> args)
        {
            if (args.Count != 1)
            {
                await SendAlertCommandEmbed();
                return;
            }
            
            await _alertService.RetireAlert(args[0]);
            ReactRandomly();
        }

        private Task SendAlertCommandEmbed()
        {
            return _messageService.SendMessage(_event.Message.ChannelId, new OutgoingMessage
            {
                Content = "Alert Command",
                Embed = _alertService.GetAlertEmbed()
            });
        }

        private async Task ProcessMyTime(List<string> args)
        {
            if (args.Count != 1)
            {
                await _messageService.SendMessage(_event.Message.ChannelId, new OutgoingMessage
                {
                    Content = "Incorrect command format: !plog mytime [timezone]"
                });
                return;
            }
            var timeZone = args[0];
            if (!_timeZoneService.IsValid(timeZone))
            {
                await _messageService.SendMessage(_event.Message.ChannelId, new OutgoingMessage
                {
                    Content = $"Time Zone \"{timeZone}\" is not valid. Examples: EST, EDT, CST, CDT, PST, PDT"
                });
                return;
            }

            await _timeZoneService.SaveTimeZonePreference(timeZone, _event.Message.Author.Id);
            await _messageService.SendMessage(_event.Message.ChannelId, new OutgoingMessage
            {
                Content = $"Your time zone has been set to \"{timeZone}\"."
            });
        }

        private void ReactRandomly()
        {
            // TODO: Cache this stuff? Too many requests needed.
            Task.Run(async () => {
                var channel = await _messageService.GetChannel(_event.Message.ChannelId);
                var emojis = await _guildService.GetGuildEmoji(channel.GuildId.Value);
                if (emojis.Count == 0)
                {
                    return;
                }
                var random = new Random();
                var index = random.Next(0, emojis.Count);
                await _messageService.CreateReactionAsync(_event.Message.ChannelId, _event.Message.Id, emojis[index].Id);
            });
        }
    }
}
