using HtmlAgilityPack;
using Newtonsoft.Json;
using PlogBot.Data.Enums;
using PlogBot.Services.Constants;
using PlogBot.Services.Interfaces;
using PlogBot.Services.Models;
using PlogBot.Services.WebModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace PlogBot.Services
{
    public class BladeAndSoulService : IBladeAndSoulService
    {
        private readonly HttpClient _client;
        private readonly Dictionary<string, CharacterClass> _classDict;
        private readonly IDictionary<CharacterClass, string> _emojiDict;

        private readonly ILoggingService _loggingService;

        public BladeAndSoulService(ILoggingService loggingService)
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("http://na-bns.ncsoft.com")
            };
            _classDict = new Dictionary<string, CharacterClass>
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
            _emojiDict = new Dictionary<CharacterClass, string>
            {
                { CharacterClass.Warlock, EmojiConstants.WarlockIcon },
                { CharacterClass.BladeMaster, EmojiConstants.BladeMasterIcon },
                { CharacterClass.BladeDancer, EmojiConstants.BladeDancerIcon },
                { CharacterClass.Soulfighter, EmojiConstants.SoulFighterIcon },
                { CharacterClass.ForceMaster, EmojiConstants.ForceMasterIcon },
                { CharacterClass.Gunner, EmojiConstants.GunnerIcon },
                { CharacterClass.KungfuMaster, EmojiConstants.KungFuMasterIcon },
                { CharacterClass.Assassin, EmojiConstants.AssassinIcon },
                { CharacterClass.Summoner, EmojiConstants.SummonerIcon },
                { CharacterClass.Destroyer, EmojiConstants.DestroyerIcon }
            };

            _loggingService = loggingService;
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

            var accountName = htmlDocument.DocumentNode.SelectSingleNode("//dl[@class=\"signature\"]")?.SelectSingleNode("//dt")?.ChildNodes.First().InnerHtml;
            int.TryParse(htmlDocument.DocumentNode.SelectSingleNode("//span[@class=\"masteryLv\"]")?.InnerText.Split(' ')[1], out var hmLevel);
            var guild = htmlDocument.DocumentNode.SelectSingleNode("//li[@class=\"guild\"]")?.InnerText;
            var @class = htmlDocument.DocumentNode.SelectSingleNode("//dd[@class=\"desc\"]")?.SelectSingleNode("//li")?.InnerHtml;
            int.TryParse(htmlDocument.DocumentNode.SelectSingleNode("//dd[@class=\"desc\"]")?.SelectNodes("//li")[1].InnerText.Split(' ')[1], out var lvl);
            var img = htmlDocument.DocumentNode.SelectSingleNode("//div[@class=\"charaterView\"]")?.SelectSingleNode("//img");
            // Yes, character view is actually spelled wrong on the website. This could potentially break if they fix it.
            var profileImageUrl = htmlDocument.DocumentNode.SelectSingleNode("//div[@class=\"charaterView\"]")?.SelectSingleNode("//img").Attributes.AttributesWithName("src").ToList()[0].Value;

            if (profileImageUrl == null)
            {
                var noImgSpan = htmlDocument.DocumentNode.SelectSingleNode("//div[@class=\"charaterView\"]")?.SelectSingleNode("span[@class=\"msgPhoto\"]");

                if (noImgSpan == null)
                {
                    // Something is wrong, send me an error log.
                    await _loggingService.LogErrorAsync("The profile image url is null and the noImg span is null.  Did they finally fix the spelling?");
                }

            }

            return new BladeAndSoulCharacter
            {
                AccountName = accountName,
                ProfileImageUrl = profileImageUrl,
                Name = name,
                Class = _classDict[@class],
                Clan = guild,
                Level = lvl,
                HongmoonLevel = hmLevel
            };
        }

        public async Task<AbilitiesResult> GetBladeAndSoulCharacterAbilities(string name)
        {
            var abilities = await _client.GetAsync($"/ingame/bs/character/data/abilities.json?c={HttpUtility.UrlEncode(name)}");
            var result = JsonConvert.DeserializeObject<AbilitiesResult>(await abilities.Content.ReadAsStringAsync());

            if (result.Result.ToLower() != "success")
            {
                return null;
            }

            return result;
        }

        public string GetClassEmojiByClass(CharacterClass @class)
        {
            return _emojiDict[@class];
        }
    }
}
