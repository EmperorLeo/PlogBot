using System.Collections.Generic;

namespace PlogBot.Services.Models
{
    public class Raffle
    {
        public Raffle()
        {
            Participants = new HashSet<ulong>();
            Winners = new HashSet<ulong>();
        }
        
        public int MaxTickets { get; set; }
        public HashSet<ulong> Participants { get; set; }
        public HashSet<ulong> Winners { get; set; }
    }
}