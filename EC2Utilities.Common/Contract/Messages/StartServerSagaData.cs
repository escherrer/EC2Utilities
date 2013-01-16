using System;
using NServiceBus.Saga;

namespace EC2Utilities.Common.Contract.Messages
{
    public class StartServerSagaData : IContainSagaData
    {
        public Guid Id { get; set; }
        public string Originator { get; set; }
        public string OriginalMessageId { get; set; }
        public string InstanceId { get; set; }
        public ServerStartUpStatus ServerStartUpStatus { get; set; }
        public string NotificationEmailAddress { get; set; }
    }
}