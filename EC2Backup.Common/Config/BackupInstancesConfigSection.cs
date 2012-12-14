using System.Configuration;

namespace EC2Backup.Common.Config
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
