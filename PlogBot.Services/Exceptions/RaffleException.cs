using System;

namespace PlogBot.Services.Exceptions
{
    public class RaffleException : Exception
    {
        public RaffleException() : base() { }

        public RaffleException(string message) : base(message) { }
    }
}