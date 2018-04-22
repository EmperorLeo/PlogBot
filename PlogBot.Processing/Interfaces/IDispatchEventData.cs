namespace PlogBot.Processing.Interfaces
{
    public interface IDispatchEventData : IEventData
    {
        void Initialize(string data);
    }
}
