﻿using Newtonsoft.Json;
using PlogBot.OAuth2;
using PlogBot.OAuth2.Interfaces;
using PlogBot.Services.DiscordObjects;
using PlogBot.Services.Interfaces;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PlogBot.Services
{
    public class UserService : IUserService
    {
        private readonly IDiscordApiClient _discordApiClient;

        public UserService(IDiscordApiClient discordApiClient)
        {
            _discordApiClient = discordApiClient;
        }

        public async Task<User> GetUser(ulong id)
        {
            var client = _discordApiClient.BotAuth();
            var result = await client.GetAsync($"{DiscordApiConstants.BaseUrl}/users/{id}");
            if (!result.IsSuccessStatusCode)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<User>(await result.Content.ReadAsStringAsync());
        }

        public async Task<Channel> OpenDm(ulong id)
        {
            var client = _discordApiClient.BotAuth();
            var result = await client.PostAsync($"{DiscordApiConstants.BaseUrl}/users/@me/channels",
                new StringContent(JsonConvert.SerializeObject(new { recipient_id = id}), Encoding.UTF8, "application/json"));

            if (!result.IsSuccessStatusCode)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<Channel>(await result.Content.ReadAsStringAsync());
        }
    }
}
