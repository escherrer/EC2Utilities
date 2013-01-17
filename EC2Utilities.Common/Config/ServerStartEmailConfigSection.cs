using System.Configuration;

namespace EC2Utilities.Common.Config
{
    public class ServerStartEmailConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("EmailAddresses")]
        public EmailAddressCollection InstanceItems
        {
            get { return ((EmailAddressCollection)(base["EmailAddresses"])); }
        }
    }
}
