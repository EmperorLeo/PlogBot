using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlogBot.Services.Exceptions;
using PlogBot.Services.Interfaces;
using PlogBot.Services.Models;

namespace PlogBot.Services
{
    public class RaffleService : IRaffleService
    {
        private readonly IDictionary<ulong, Raffle> _raffleState;

        public RaffleService()
        {
            _raffleState = new Dictionary<ulong, Raffle>();
        }

        public Task<ulong> EndRaffle(ulong channelId)
        {
            if (!_raffleState.ContainsKey(channelId))
            {
                throw new RaffleException("You must start a raffle first!");
            }

            return DrawTicketInternal(channelId, true);
        }

        public Task GetTicket(ulong discordUserId, ulong channelId)
        {
            if (!_raffleState.ContainsKey(channelId))
            {
                throw new RaffleException("No current raffle!");
            }

            var raffle = _raffleState[channelId];

            if (raffle.MaxTickets != 0 && raffle.Participants.Count >= raffle.MaxTickets)
            {
                throw new RaffleException("There are no more tickets left!");
            }

            if (raffle.Winners.Contains(discordUserId))
            {
                throw new RaffleException("You have already won!");
            }

            if (!raffle.Participants.Add(discordUserId))
            {
                throw new RaffleException("You already have a ticket!");
            }

            return Task.CompletedTask;
        }

        public Task StartRaffle(int numTickets, ulong channelId)
        {
            if (_raffleState.ContainsKey(channelId))
            {
                throw new RaffleException("Cannot start a raffle when one is already in progress!");
            }
            var raffle = new Raffle
            {
                MaxTickets = numTickets
            };
            _raffleState.Add(channelId, raffle);
            return Task.CompletedTask;
        }

        public Task<ulong> DrawTicket(ulong channelId)
        {
            if (!_raffleState.ContainsKey(channelId))
            {
                throw new RaffleException("You must start a raffle first!");
            }

            return DrawTicketInternal(channelId, false);
        }

        private Task<ulong> DrawTicketInternal(ulong channelId, bool end)
        {
            var raffle = _raffleState[channelId];
            var participants = raffle.Participants.ToList();
            if (participants.Count == 0)
            {
                return Task.FromResult((ulong.Parse("0")));
            }
            var count = participants.Count;
            var random = new Random();
            var index = (int) Math.Floor((double)random.Next(0, count));

            var winner = participants[index];
            raffle.Winners.Add(winner);
            raffle.Participants.Remove(winner);
            if (end || raffle.Participants.Count == 0)
            {
                _raffleState.Remove(channelId);
            }
            return Task.FromResult(winner);
        }
    }
}