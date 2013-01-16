using NServiceBus;

namespace EC2Utilities.Common.Contract.Messages
{
    public class ServerStatusMessage : IMessage
    {
        public string InstanceId { get; set; }

        public ServerStartUpStatus StartUpStatus { get; set; }
    }
}