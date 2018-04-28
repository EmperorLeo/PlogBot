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
            // Yes, character view is actually spelled wrong on the website. This could potentially break if they fix it.
            var profileImageUrl = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='charaterView']//img")?.Attributes.AttributesWithName("src").ToList()[0].Value;

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

        public async Task<BladeAndSoulItems> GetBladeAndSoulItemsAsync(string name)
        {
            var itemsPartial = await _client.GetAsync($"ingame/bs/character/data/equipments?c={HttpUtility.UrlEncode(name)}");
            if (!itemsPartial.IsSuccessStatusCode)
            {
                return null;
            }

            var htmlDocument = new HtmlDocument();
            htmlDocument.Load(await itemsPartial.Content.ReadAsStreamAsync());

            var root = htmlDocument.DocumentNode;

            // Weapons
            var weaponWrapper = root.SelectSingleNode("//div[@class='wrapWeapon']");
            var weaponDiv = weaponWrapper.SelectSingleNode("//p[@class='thumb']//img");
            var weapon = weaponDiv.Attributes.FirstOrDefault(a => a.Name == "alt").Value;
            var weaponImg = weaponDiv.Attributes.FirstOrDefault(a => a.Name == "src").Value;
            var gemDivs = weaponWrapper.SelectNodes("//div[@class='enchant']//span[@class='iconGemSlot']//img");
            var gems = gemDivs.Select(x => new
            {
                Name = x.Attributes.FirstOrDefault(a => a.Name == "alt")?.Value,
                Img = x.Attributes.FirstOrDefault(a => a.Name == "src")?.Value,
            }).ToList();

            // Accessories html elements
            var ringDiv = root.SelectSingleNode("//div[@class='accessoryArea']//div[@class='wrapAccessory ring']//img");
            var earringDiv = root.SelectSingleNode("//div[@class='accessoryArea']//div[@class='wrapAccessory earring']//img");
            var necklaceDiv = root.SelectSingleNode("//div[@class='accessoryArea']//div[@class='wrapAccessory necklace']//img");
            var braceletDiv = root.SelectSingleNode("//div[@class='accessoryArea']//div[@class='wrapAccessory bracelet']//img");
            var beltDiv = root.SelectSingleNode("//div[@class='accessoryArea']//div[@class='wrapAccessory belt']//img");
            var glovesDiv = root.SelectSingleNode("//div[@class='accessoryArea']//div[@class='wrapAccessory gloves']//img");
            var soulDiv = root.SelectSingleNode("//div[@class='accessoryArea']//div[@class='wrapAccessory soul']//img");
            var heartDiv = root.SelectSingleNode("//div[@class='accessoryArea']//div[@class='wrapAccessory soul-2']//img");
            var petDiv = root.SelectSingleNode("//div[@class='accessoryArea']//div[@class='wrapAccessory guard']//img");
            var soulBadgeDiv = root.SelectSingleNode("//div[@class='accessoryArea']//div[@class='wrapAccessory singongpae']//img");
            var mysticBadgeDiv = root.SelectSingleNode("//div[@class='accessoryArea']//div[@class='wrapAccessory rune']//img");

            // Set accessory values
            var ring = ringDiv?.Attributes.FirstOrDefault(a => a.Name == "alt")?.Value;
            var ringImg = ringDiv?.Attributes.FirstOrDefault(a => a.Name == "src")?.Value;
            var earring = earringDiv?.Attributes.FirstOrDefault(a => a.Name == "alt")?.Value;
            var earringImg = earringDiv?.Attributes.FirstOrDefault(a => a.Name == "src")?.Value;
            var necklace = necklaceDiv?.Attributes.FirstOrDefault(a => a.Name == "alt")?.Value;
            var necklaceImg = necklaceDiv?.Attributes.FirstOrDefault(a => a.Name == "src")?.Value;
            var bracelet = braceletDiv?.Attributes.FirstOrDefault(a => a.Name == "alt")?.Value;
            var braceletImg = braceletDiv?.Attributes.FirstOrDefault(a => a.Name == "src")?.Value;
            var belt = beltDiv?.Attributes.FirstOrDefault(a => a.Name == "alt")?.Value;
            var beltImg = beltDiv?.Attributes.FirstOrDefault(a => a.Name == "src")?.Value;
            var gloves = glovesDiv?.Attributes.FirstOrDefault(a => a.Name == "alt")?.Value;
            var glovesImg = glovesDiv?.Attributes.FirstOrDefault(a => a.Name == "src")?.Value;
            var soul = soulDiv?.Attributes.FirstOrDefault(a => a.Name == "alt")?.Value;
            var soulImg = soulDiv?.Attributes.FirstOrDefault(a => a.Name == "src")?.Value;
            var heart = heartDiv?.Attributes.FirstOrDefault(a => a.Name == "alt")?.Value;
            var heartImg = heartDiv?.Attributes.FirstOrDefault(a => a.Name == "src")?.Value;
            var pet = petDiv?.Attributes.FirstOrDefault(a => a.Name == "alt")?.Value;
            var petImg = petDiv?.Attributes.FirstOrDefault(a => a.Name == "src")?.Value;
            var soulBadge = soulBadgeDiv?.Attributes.FirstOrDefault(a => a.Name == "alt")?.Value;
            var soulBadgeImg = soulBadgeDiv?.Attributes.FirstOrDefault(a => a.Name == "src")?.Value;
            var mysticBadge = mysticBadgeDiv?.Attributes.FirstOrDefault(a => a.Name == "alt")?.Value;
            var mysticBadgeImg = mysticBadgeDiv?.Attributes.FirstOrDefault(a => a.Name == "src")?.Value;

            return new BladeAndSoulItems
            {
                Weapon = weapon,
                WeaponImg = weaponImg,
                Gem1 = gems.Count > 0 ? gems[0].Name : null,
                Gem1Img = gems.Count > 0 ? gems[0].Img : null,
                Gem2 = gems.Count > 1 ? gems[1].Name : null,
                Gem2Img = gems.Count > 1 ? gems[1].Img : null,
                Gem3 = gems.Count > 2 ? gems[2].Name : null,
                Gem3Img = gems.Count > 2 ? gems[2].Img : null,
                Gem4 = gems.Count > 3 ? gems[3].Name : null,
                Gem4Img = gems.Count > 3 ? gems[3].Img : null,
                Gem5 = gems.Count > 4 ? gems[4].Name : null,
                Gem5Img = gems.Count > 4 ? gems[4].Img : null,
                Gem6 = gems.Count > 5 ? gems[5].Name : null,
                Gem6Img = gems.Count > 5 ? gems[5].Img : null,
                Ring = ring,
                RingImg = ringImg,
                Earring = earring,
                EarringImg = earringImg,
                Necklace = necklace,
                NecklaceImg = necklaceImg,
                Bracelet = bracelet,
                BraceletImg = braceletImg,
                Belt = belt,
                BeltImg = beltImg,
                Gloves = gloves,
                GlovesImg = glovesImg,
                Soul = soul,
                SoulImg = soulImg,
                Heart = heart,
                HeartImg = heartImg,
                Pet = pet,
                PetImg = petImg,
                SoulBadge = soulBadge,
                SoulBadgeImg = soulBadgeImg,
                MysticBadge = mysticBadge,
                MysticBadgeImg = mysticBadgeImg
            };
        }

        public string GetClassEmojiByClass(CharacterClass @class)
        {
            return _emojiDict[@class];
        }
    }
}
