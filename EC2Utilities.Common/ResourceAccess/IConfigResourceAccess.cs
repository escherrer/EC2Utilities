using System.Collections.Generic;

namespace EC2Utilities.Common.ResourceAccess
{
    public interface IConfigResourceAccess
    {
        List<string> GetNotificationEmailaddresses();

        Ec2Key GetEc2Key();

        string GetServiceName();

        string GetEmailAlertFromEmailAddress();

        int GetBackupRetentionDays();
    }
}