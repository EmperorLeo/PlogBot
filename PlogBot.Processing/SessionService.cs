using PlogBot.Processing.Interfaces;
using System;

namespace PlogBot.Processing
{
    public class SessionService : ISessionService
    {
        public string SessionId { get; set; }
        public DateTime? LastHeartbeatAck { get; set; }
        public int? LastSequenceNumber { get; set; }
    }
}
