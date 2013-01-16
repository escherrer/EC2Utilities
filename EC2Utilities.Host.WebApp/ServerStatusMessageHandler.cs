using EC2Utilities.Common.Contract.Messages;
using NServiceBus;

namespace EC2Utilities.Host.WebApp
{
    public class ServerStatusMessageHandler : IHandleMessages<ServerStatusMessage>
    {
        public void Handle(ServerStatusMessage message)
        {
            InstanceData.SetStatus(message.InstanceId, message.StartUpStatus);
        }
    }
}