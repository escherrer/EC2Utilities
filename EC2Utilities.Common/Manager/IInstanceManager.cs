using System.Collections.Generic;
using EC2Utilities.Common.Contract;

namespace EC2Utilities.Common.Manager
{
    public interface IInstanceManager
    {
        List<Ec2UtilityInstance> GetInstances();
    }
}