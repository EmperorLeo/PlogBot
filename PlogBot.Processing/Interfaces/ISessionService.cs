using System;

namespace PlogBot.Processing.Interfaces
{
    public interface ISessionService
    {
        string SessionId { get; set; }
        DateTime? LastHeartbeatAck { get; set; }
        int? LastSequenceNumber { get; set; }
        bool Reconnect { get; set; }
    }
}
