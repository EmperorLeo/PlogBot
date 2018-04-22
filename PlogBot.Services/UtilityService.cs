using System;
using System.Text;
using PlogBot.Services.Interfaces;

namespace PlogBot.Services
{
    public class UtilityService : IUtilityService
    {
        public string FromArraySegmentBytes(ArraySegment<byte> bytes)
        {
            return Encoding.UTF8.GetString(bytes.Array).Replace("\0", "");
        }
    }
}
