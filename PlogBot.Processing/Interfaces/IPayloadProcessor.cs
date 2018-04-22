using System;

namespace PlogBot.Processing.Interfaces
{
    public interface IPayloadProcessor
    {
        IEventData Process(string streamData);

        int GetLastSequenceNumber();
    }
}
