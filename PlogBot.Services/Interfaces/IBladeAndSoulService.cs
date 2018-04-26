using PlogBot.Services.Models;
using System.Threading.Tasks;

namespace PlogBot.Services.Interfaces
{
    public interface IBladeAndSoulService
    {
        Task<BladeAndSoulCharacter> GetBladeAndSoulCharacter(string name);
    }
}
