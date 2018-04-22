﻿using System;
using System.Net.WebSockets;
using System.Threading.Tasks;
using PlogBot.Processing.Interfaces;

namespace PlogBot.Processing.EventData
{
    public class HeartbeatAck : IEventData
    {
        public static DateTime? LastHeartbeatRecieved { get; set; }

        public Task Respond(ClientWebSocket ws, int sequenceNum, string token)
        {
            LastHeartbeatRecieved = DateTime.UtcNow;
            Console.WriteLine("Processed heartbeat ACK.");
            return Task.CompletedTask;
        }
    }
}
