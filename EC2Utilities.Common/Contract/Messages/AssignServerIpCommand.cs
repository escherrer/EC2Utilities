using NServiceBus;

namespace EC2Utilities.Common.Contract.Messages
{
    public class AssignServerIpCommand : ICommand
    {
        public string InstanceId { get; set; }
    }
}