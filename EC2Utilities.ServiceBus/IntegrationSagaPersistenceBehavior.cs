using NServiceBus;
using NServiceBus.Hosting.Profiles;

namespace EC2Utilities.ServiceBus
{
    public class IntegrationSagaPersistenceBehavior : IHandleProfile<Integration>
    {
        public void ProfileActivated()
        {
            Configure.Instance.InMemorySagaPersister();
        }
    }

}
