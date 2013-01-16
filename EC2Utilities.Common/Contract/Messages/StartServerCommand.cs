using System;
using NServiceBus;
using NServiceBus.Saga;

namespace EC2Utilities.Common.Contract.Messages
{
    public class StartServerCommand : ICommand
    {
        public string InstanceId { get; set; }
    }

    public class CheckServerStatusCommand : ICommand
    {
        public string InstanceId { get; set; }
    }

    public class ServerStatusMessage : IMessage
    {
        public string InstanceId { get; set; }

        public ServerStartUpStatus StartUpStatus { get; set; }
    }

    public class AssignServerIpCommand : ICommand
    {
        public string InstanceId { get; set; }
    }

    public enum ServerStartUpStatus
    {
        Initialized,
        Starting,
        Started,
        IpAssigned,
        Complete
    }

    public class StartServerSagaData : IContainSagaData
    {
        public Guid Id { get; set; }
        public string Originator { get; set; }
        public string OriginalMessageId { get; set; }
        public string InstanceId { get; set; }
        public ServerStartUpStatus ServerStartUpStatus { get; set; }
    }
}
