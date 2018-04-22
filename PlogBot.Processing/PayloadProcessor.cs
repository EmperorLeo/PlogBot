using Newtonsoft.Json;
using PlogBot.Processing.Interfaces;
using System;
using System.Text;

namespace PlogBot.Processing
{
    public class PayloadProcessor : IPayloadProcessor
    {
        private readonly IEventDataFactory _eventDataFactory;
        private int _lastSequenceNumber;

        public PayloadProcessor(IEventDataFactory eventDataFactory)
        {
            _eventDataFactory = eventDataFactory;
        }

        public int GetLastSequenceNumber()
        {
            return _lastSequenceNumber;
        }

        public IEventData Process(string streamData)
        {
            var payload = JsonConvert.DeserializeObject<Payload>(streamData);
            if (payload.SequenceNumber.HasValue)
            {
                _lastSequenceNumber = payload.SequenceNumber.Value;
            }
            Console.WriteLine("Recieved opcode: " + payload.Opcode);
            Console.WriteLine("Processed Data: " + streamData);
            return _eventDataFactory.BuildEventData(payload.Opcode, JsonConvert.SerializeObject(payload.Data));
        }
    }
}
