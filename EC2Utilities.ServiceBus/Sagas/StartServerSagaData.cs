using System;
using NServiceBus.Saga;

namespace EC2Utilities.ServiceBus.Sagas
{
    public class StartServerSagaData : IContainSagaData
    {
        public Guid Id { get; set; }
        public string Originator { get; set; }
        public string OriginalMessageId { get; set; }
    }
}