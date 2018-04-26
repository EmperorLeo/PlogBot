using HtmlAgilityPack;
using Newtonsoft.Json;
using PlogBot.Data.Enums;
using PlogBot.Services.Interfaces;
using PlogBot.Services.Models;
using PlogBot.Services.WebModels;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace PlogBot.Services
{
    public class BladeAndSoulService : IBladeAndSoulService
    {
        private readonly HttpClient _client;
        private readonly Dictionary<string, CharacterClass> _classDict;

        public BladeAndSoulService()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("http://na-bns.ncsoft.com")
            };
            _classDict =  new Dictionary<string, CharacterClass>
            {
                { "Warlock", CharacterClass.Warlock },
                { "Blade Master", CharacterClass.BladeMaster },
                { "Blade Dancer", CharacterClass.BladeDancer },
                { "Soul Fighter", CharacterClass.Soulfighter },
                { "Gunner", CharacterClass.Gunner },
                { "Kung Fu Master", CharacterClass.KungfuMaster },
                { "Force Master", CharacterClass.ForceMaster },
                { "Assassin", CharacterClass.Assassin },
                { "Summoner", CharacterClass.Summoner },
                { "Destroyer", CharacterClass.Destroyer }
            };
        }

        public async Task<BladeAndSoulCharacter> GetBladeAndSoulCharacter(string name)
        {
            var profileHtml = await _client.GetAsync($"/ingame/bs/character/profile?c={HttpUtility.UrlDecode(name)}");

            if (!profileHtml.IsSuccessStatusCode)
            {
                return null;
            }

            var htmlDocument = new HtmlDocument();
            htmlDocument.Load(await profileHtml.Content.ReadAsStreamAsync());

            int.TryParse(htmlDocument.DocumentNode.SelectSingleNode("//span[@class=\"masteryLv\"]")?.InnerText.Split(' ')[1], out var hmLevel);
            var guild = htmlDocument.DocumentNode.SelectSingleNode("//li[@class=\"guild\"]")?.InnerText;
            var @class = htmlDocument.DocumentNode.SelectSingleNode("//dd[@class=\"desc\"]")?.SelectSingleNode("//li")?.InnerHtml;

            var abilities = await _client.GetAsync($"/ingame/bs/character/data/abilities.json?c={HttpUtility.UrlEncode(name)}");
            var result = JsonConvert.DeserializeObject<AbilitiesResult>(await abilities.Content.ReadAsStringAsync());

            if (result.Result.ToLower() != "success")
            {
                return null;
            }

            return new BladeAndSoulCharacter
            {
                AbilitiesResult = result,
                Name = name,
                Class = _classDict[@class],
                Clan = guild,
                Level = 55,
                HongmoonLevel = hmLevel
            };
        }
    }
}
