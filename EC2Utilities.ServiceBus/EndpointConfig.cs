using EC2Utilities.Common.Factory;

namespace EC2Utilities.ServiceBus 
{
    using NServiceBus;

	/*
		This class configures this endpoint as a Server. More information about how to configure the NServiceBus host
		can be found here: http://nservicebus.com/GenericHost.aspx
	*/
	public class EndpointConfig : IConfigureThisEndpoint, AsA_Server
    {
	    public EndpointConfig()
	    {
            ContainerBootstrapper.BootstrapStructureMap();

            //Configure.With()
            //    .DefaultBuilder()
            //    .Log4Net()
            //    .XmlSerializer()
            //    .MsmqTransport()
            //    .UnicastBus()
            //    .CreateBus()
            //    .Start(() => Configure.Instance.ForInstallationOn<NServiceBus.Installation.Environments.Windows>().Install());

	        //Configure.With().DefaultBuilder().InMemorySagaPersister();
	    }
    }
}