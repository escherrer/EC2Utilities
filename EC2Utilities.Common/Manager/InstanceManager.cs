﻿using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.EC2.Model;
using EC2Utilities.Common.Contract;
using EC2Utilities.Common.ResourceAccess;
using EC2Utilities.Common.Utility;
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

            Ec2Key ec2Key = _configResourceAccess.GetEc2Key();
            DescribeInstancesResult ec2Instances = _ec2ResourceAccess.GetInstances(ec2Key);

            var returnInstances = new List<Ec2UtilityInstance>();
            foreach (var reservation in ec2Instances.Reservation)
            {
                foreach (var runningInstance in reservation.RunningInstance)
                {
                    var returnInstance = new Ec2UtilityInstance();

                    returnInstance.InstanceId = runningInstance.InstanceId;
                    returnInstance.InstanceName = runningInstance.Tag.GetTagValueByKey("Name");
                    returnInstance.Status = (Ec2UtilityInstanceStatus)Enum.Parse(typeof(Ec2UtilityInstanceStatus), runningInstance.InstanceState.Name, true);
                    returnInstance.DefaultIp = runningInstance.Tag.GetTagValueByKey("DefaultIp");

                    returnInstances.Add(returnInstance);   
                }
            }

            _logger.Trace("Get Instances End.");

            return returnInstances;
        }

        public Ec2UtilityInstance GetInstance(string instanceid)
        {
            return GetInstances().SingleOrDefault(x => x.InstanceId == instanceid);
        }

        public void StartUpInstance(string instanceId)
        {
            _logger.Trace("StartUpInstance Start.");

            Ec2Key ec2Key = _configResourceAccess.GetEc2Key();

            _ec2ResourceAccess.StartUpInstance(ec2Key, instanceId);

            _logger.Trace("StartUpInstance End.");
        }

        public void AssignInstanceIp(string instanceId)
        {
            _logger.Trace("AssignInstanceIp Start.");

            Ec2Key ec2Key = _configResourceAccess.GetEc2Key();

            Ec2UtilityInstance instance = GetInstances().Single(x => x.InstanceId == instanceId);

            if (!string.IsNullOrWhiteSpace(instance.DefaultIp))
            {
                _ec2ResourceAccess.AssociateIpToInstance(ec2Key, instanceId, instance.DefaultIp);
            }

            _logger.Trace("AssignInstanceIp End.");
        }

        public void SendServerAvailableNotification(string instanceId, string notificationEmailAddress)
        {
            _logger.Trace("AssignInstanceIp Start.");

            Ec2Key ec2Key = _configResourceAccess.GetEc2Key();

            Ec2UtilityInstance instance = GetInstances().Single(x => x.InstanceId == instanceId);

            string subject = string.Format("Instance {0} Started", instance.InstanceName);
            string body = string.Format("Instance {0} has been started and assigned to IP {1}.", instance.InstanceName, instance.DefaultIp);

            _ec2ResourceAccess.SendEmail(ec2Key, "mike@czarmetrics.com", new List<string> { notificationEmailAddress, "mike@czarmetrics.com", "escherrer@gmail.com" }, subject, body);

            _logger.Trace("AssignInstanceIp End.");
        }
    }
}
