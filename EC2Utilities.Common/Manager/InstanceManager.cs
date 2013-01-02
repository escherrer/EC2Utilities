using System.Collections.Generic;
using EC2Utilities.Common.Contract;
using EC2Utilities.Common.ResourceAccess;
using NLog;

namespace EC2Utilities.Common.Manager
{
    public class InstanceManager : IInstanceManager
    {
        private readonly IConfigResourceAccess _configResourceAccess;
        private readonly IEc2ResourceAccess _ec2ResourceAccess;
        private readonly Logger _logger;

        public InstanceManager(IConfigResourceAccess configResourceAccess, IEc2ResourceAccess ec2ResourceAccess, Logger logger)
        {
            _configResourceAccess = configResourceAccess;
            _ec2ResourceAccess = ec2ResourceAccess;
            _logger = logger;
        }

        public List<Ec2UtilityInstance> GetInstances()
        {
            _logger.Trace("Get Instances Start.");

            var ec2Key = _configResourceAccess.GetEc2Key();
            var ec2Instances = _ec2ResourceAccess.GetInstances(ec2Key);

            var returnInstances = new List<Ec2UtilityInstance>();
            foreach (var reservation in ec2Instances.Reservation)
            {
                foreach (var runningInstance in reservation.RunningInstance)
                {
                    var returnInstance = new Ec2UtilityInstance();

                    returnInstance.InstanceId = runningInstance.InstanceId;
                    returnInstance.InstanceName = "ToDo";
                    returnInstance.Status = Ec2UtilityInstanceStatus.Stopped;

                    returnInstances.Add(returnInstance);   
                }
            }

            _logger.Trace("Get Instances End.");

            return returnInstances;
        }
    }
}
