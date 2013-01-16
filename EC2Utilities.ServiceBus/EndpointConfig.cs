using EC2Utilities.Common.Factory;

namespace EC2Utilities.ServiceBus 
{
    using NServiceBus;

	/*
		This class configures this endpoint as a Server. More information about how to configure the NServiceBus host
		can be found here: http://nservicebus.com/GenericHost.aspx
	*/
	public class EndpointConfig : IConfigureThisEndpoint, AsA_Publisher
    {
	    public EndpointConfig()
	    {
            ContainerBootstrapper.BootstrapStructureMap();
	    }
    }
}