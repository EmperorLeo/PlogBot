namespace PlogBot.Processing.Interfaces
{
    interface ISessionService
    {
        void SetSessionId(string sessionId);

        string GetSessionId();
    }
}
