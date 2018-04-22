using System;

namespace PlogBot.Services.Interfaces
{
    public interface IUtilityService
    {
        string FromArraySegmentBytes(ArraySegment<byte> bytes);
    }
}
