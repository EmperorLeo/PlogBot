using PlogBot.Data.Enums;
using PlogBot.Services.Models;
using PlogBot.Services.WebModels;
using System.Threading.Tasks;

namespace PlogBot.Services.Interfaces
{
    public interface IBladeAndSoulService
    {
        Task<BladeAndSoulCharacter> GetBladeAndSoulCharacter(string name);
        Task<AbilitiesResult> GetBladeAndSoulCharacterAbilities(string name);
        Task<BladeAndSoulItems> GetBladeAndSoulItemsAsync(string name);
        string GetClassEmojiByClass(CharacterClass @class);
    }
}
