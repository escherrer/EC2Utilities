using System.Collections.Generic;
using System.Reflection;
using System.Text;
using EC2Utilities.Common.Engine;
using EC2Utilities.Common.Manager;
using EC2Utilities.Common.ResourceAccess;
using NLog;
using StructureMap;

namespace EC2Utilities.Common.Factory
{
    public static class ContainerBootstrapper
    {
        public static void BootstrapStructureMap()
        {
            // Initialize the static ObjectFactory container
            ObjectFactory.Initialize(x =>
            {
                x.For<IBackupEngine>().Use<BackupEngine>();
                x.For<IConfigResourceAccess>().Use<ConfigResourceAccess>();
                x.For<IEc2ResourceAccess>().Use<Ec2ResourceAccess>();
                x.For<IScheduleEngine>().Use<ScheduleEngine>();
                x.For<IScheduleManager>().Use<ScheduleManager>();
                x.For<IBackupManager>().Use<BackupManager>();
                x.For<Logger>().Use(y => LogManager.GetLogger(""));
                x.For<IInstanceManager>().Use<InstanceManager>();
                x.RegisterInterceptor(new ResourceAccessTypeInterceptor());
            });
        }
    }
}
