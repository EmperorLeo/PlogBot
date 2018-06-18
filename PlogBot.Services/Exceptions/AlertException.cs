using System;

namespace PlogBot.Services.Exceptions
{
    public class AlertException : Exception
    {
        public AlertException() : base() { }

        public AlertException(string message) : base(message) { }
    }
}