using System.Collections.Generic;
using EC2Utilities.Common.Contract;

namespace EC2Utilities.Common.Manager
{
    public interface IInstanceManager
    {
        List<Ec2UtilityInstance> GetInstances();

        Ec2UtilityInstance GetInstance(string instanceId);

        void StartUpInstance(string instanceId);

        void AssignInstanceIp(string instanceId);

        void SendStartUpEmail(string instanceId, string notificationEmailAddress, string subject, string body);

        List<string> GetAvailableInstanceSizes(string instanceId);

        void ChangeInstanceType(string instanceId, string instanceType);
    }
}