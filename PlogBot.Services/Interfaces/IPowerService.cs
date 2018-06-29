using PlogBot.Services.Models;
using PlogBot.Services.WebModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlogBot.Services.Interfaces
{
    public interface IPowerService
    {
        Task<int> CalculateScore(AbilitiesResultAbility abilities);
        //Task<int> GetWhaleScoreByCharacterName(string name);
        Task<List<WhaleField>> GetWhales(int numWhales);
        Task<List<WhaleField>> GetWhaleScoresForUser(ulong discordUserId);
    }
}
