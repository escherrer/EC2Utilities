using System.Configuration;

namespace EC2Utilities.Common.Config
{
    public class BackupInstancesConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("Instances")]
        public InstancesCollection InstanceItems
        {
            get { return ((InstancesCollection)(base["Instances"])); }
        }
    }
}
