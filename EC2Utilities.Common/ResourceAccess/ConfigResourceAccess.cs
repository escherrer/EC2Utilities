using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using EC2Utilities.Common.Config;
using EC2Utilities.Common.DebugHelper;

namespace EC2Utilities.Common.ResourceAccess
{
    public class ConfigResourceAccess : IConfigResourceAccess
    {
        public List<string> GetNotificationEmailaddresses()
        {
            var section = (ServerStartEmailConfigSection)ConfigurationManager.GetSection("ServerStartEmail");

            IEnumerable<string> result = section.InstanceItems.Cast<EmailAddressElement>().Select(x => x.EmailAddress);

            return result.ToList();
        }

        public Ec2Key GetEc2Key()
        {
            var result = new Ec2Key();
            
            result.AwsAccessKey = ConfigurationManager.AppSettings["AWSAccessKey"];
            result.AwsSecretKey = ConfigurationManager.AppSettings["AWSSecretKey"];

#if DEBUG
            var debugCreds = CredentialHelper.GetDebugCredentials();
            result.AwsAccessKey = debugCreds.AccessKeyId;
            result.AwsSecretKey = debugCreds.SecretKey;
#endif

            return result;
        }

        public string GetEmailAlertFromEmailAddress()
        {
            string result = ConfigurationManager.AppSettings["EmailAlertFromEmailAddress"];

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
