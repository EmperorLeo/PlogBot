using System;
using System.Threading.Tasks;

namespace PlogBot.Services.Interfaces
{
    public interface IClanLogService
    {
        Task<string> GetCsv(DateTime since);
    }
}
