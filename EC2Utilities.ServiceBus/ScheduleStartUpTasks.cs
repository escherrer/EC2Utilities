using System;
using EC2Utilities.Common.Contract.Messages;
using EC2Utilities.Common.Manager;
using NServiceBus;
using NServiceBus.Unicast;
using StructureMap;

namespace EC2Utilities.ServiceBus
{
    public class ScheduleStartUpTasks : IWantToRunWhenTheBusStarts
    {
        private readonly IBus _bus;
        private readonly IInstanceManager _instanceManager;

        public ScheduleStartUpTasks(IBus bus)
        {
            _bus = bus;
            _instanceManager = ObjectFactory.GetInstance<IInstanceManager>();
        }

        public void Run()
        {
            Schedule.Every(new TimeSpan(0, 0, 0, 5)).Action(CheckServerStatus);
        }

        private void CheckServerStatus()
        {
            var servers = _instanceManager.GetInstances();

            foreach (var ec2UtilityInstance in servers)
            {
                var command = new CheckServerStatusCommand
                {
                    InstanceId = ec2UtilityInstance.InstanceId
                };

                _bus.SendLocal(command);   
            }
        }
    }
}
