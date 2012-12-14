using System.Configuration;

namespace EC2Backup.Common.Config
{
    public class InstanceElement : ConfigurationElement
    {
        [ConfigurationProperty("instance", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Instance
        {
            get
            {
                return ((string)(base["instance"]));
            }

            set
            {
                base["instance"] = value;
            }
        }
    }
}