using System;
using System.Collections;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using NLog;

namespace EC2Backup.Common.Installer
{
    public class InstallHelper<T> where T : ServiceBase
    {
        private readonly string _serviceName;
        private readonly Logger _logger;

        public InstallHelper(string serviceName, Logger logger)
        {
            _serviceName = serviceName;
            _logger = logger;
        }

        private bool IsInstalled()
        {
            ServiceController ctl = ServiceController.GetServices().FirstOrDefault(s => s.ServiceName == _serviceName);
            if (ctl == null)
                return false;

            return true;
        }

        private bool IsRunning()
        {
            using (var controller = new ServiceController(_serviceName))
            {
                if (!IsInstalled()) return false;
                return (controller.Status == ServiceControllerStatus.Running);
            }
        }

        private AssemblyInstaller GetInstaller()
        {
            var installer = new AssemblyInstaller(typeof(T).Assembly, null);
            installer.UseNewContext = true;
            return installer;
        }

        public void InstallService()
        {
            if (IsInstalled())
            {
                _logger.Info("Cannot install service - already installed.");
                return;
            }

            _logger.Info("Installing service.");

            using (AssemblyInstaller installer = GetInstaller())
            {
                IDictionary state = new Hashtable();
                try
                {
                    installer.Install(state);
                    installer.Commit(state);

                    _logger.Info("Service successfully installed.");
                }
                catch (Exception e)
                {
                    _logger.FatalException("Failed to install service - rolling back.", e);
                    installer.Rollback(state);
                    _logger.Info("Service install rollback complete.");
                }
            }
        }

        public void UninstallService()
        {
            if (!IsInstalled())
            {
                _logger.Info("Cannot un-install service - not installed.");
                return;
            }

            _logger.Info("Un-Installing service.");

            using (AssemblyInstaller installer = GetInstaller())
            {
                IDictionary state = new Hashtable();
                installer.Uninstall(state);
            }
        }

        public void StartService()
        {
            if (!IsInstalled()) return;

            using (var controller = new ServiceController(_serviceName))
            {
                if (controller.Status != ServiceControllerStatus.Running)
                {
                    controller.Start();
                    controller.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(10));
                }
            }
        }

        public void StopService()
        {
            if (!IsInstalled()) return;
            using (var controller = new ServiceController(_serviceName))
            {
                if (controller.Status != ServiceControllerStatus.Stopped)
                {
                    controller.Stop();
                    controller.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(10));
                }
            }
        }
    }
}
