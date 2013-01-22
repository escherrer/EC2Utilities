using NServiceBus;

namespace EC2Utilities.Common.Contract.Messages
{
    public class StartServerCommand : ICommand
    {
        public string InstanceId { get; set; }

        public string NotificationEmailAddress { get; set; }

        public string RequestedInstanceType { get; set; }
    }
}
