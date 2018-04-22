namespace PlogBot.Processing.Interfaces
{
    public interface IEventDataFactory
    {
        IEventData BuildEventData(int opcode, string data);
    }
}
