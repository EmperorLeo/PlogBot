using System;
using System.Threading.Tasks;
using PlogBot.Data.Entities;

namespace PlogBot.Services.Interfaces
{
    public interface ITimeZoneService
    {
        bool IsValid(string abbreviation);
        Task<bool> HasTimeZoneSet(ulong discordUserId);
        Tuple<int?, int> GetTime(string abbreviation, int? localDays, int localTime);
        Task<Tuple<int?, int>> GetTime(ulong discordUserId, int? localDays, int localTime);
        Task SaveTimeZonePreference(string abbreviation, ulong discordUserId);
    }
}