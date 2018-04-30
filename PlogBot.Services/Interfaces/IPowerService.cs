using PlogBot.Services.WebModels;
using System.Threading.Tasks;

namespace PlogBot.Services.Interfaces
{
    public interface IPowerService
    {
        Task<int> CalculateScore(AbilitiesResultAbility abilities);
        //Task<int> GetWhaleScoreByCharacterName(string name);
    }
}
