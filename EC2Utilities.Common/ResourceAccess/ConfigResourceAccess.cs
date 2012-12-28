using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using EC2Utilities.Common.Config;

namespace EC2Utilities.Common.ResourceAccess
{
    public class ConfigResourceAccess : IConfigResourceAccess
    {
        public List<string> GetInstances()
        {
            var section = (BackupInstancesConfigSection)ConfigurationManager.GetSection("BackupInstances");

            IEnumerable<string> result = section.InstanceItems.Cast<InstanceElement>().Select(x => x.Instance);

            return result.ToList();
        }

        public Ec2Key GetEc2Key()
        {
            var result = new Ec2Key();

            result.AccessKeyId = ConfigurationManager.AppSettings["AWSAccessKey"];
            result.SecretAccessKey = ConfigurationManager.AppSettings["AWSSecretKey"];

            return result;
        }

        public string GetServiceName()
        {
            string result = ConfigurationManager.AppSettings["ServiceName"];

            return result;
        }

        public int GetBackupRetentionDays()
        {
            int result = int.Parse(ConfigurationManager.AppSettings["BackupRetentionDays"]);

            return result;
        }
    }
}
