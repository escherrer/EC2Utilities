using System.Configuration;

namespace EC2Utilities.Common.Config
{
    public class EmailAddressElement : ConfigurationElement
    {
        [ConfigurationProperty("emailAddress", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string EmailAddress
        {
            get
            {
                return ((string)(base["emailAddress"]));
            }

            set
            {
                base["emailAddress"] = value;
            }
        }
    }
}