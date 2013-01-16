using NServiceBus;

namespace EC2Utilities.Common.Contract.Messages
{
    public class CheckServerStatusCommand : ICommand
    {
        public string InstanceId { get; set; }
    }
}