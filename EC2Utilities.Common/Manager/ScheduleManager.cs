using EC2Utilities.Common.Engine;
using StructureMap;
using log4net;

namespace EC2Utilities.Common.Manager
{
    public class ScheduleManager : IScheduleManager
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(ScheduleManager));

        public void ScheduleBackupTasks()
        {
            _logger.Debug("ScheduleBackupTasks Start.");

            var scheduleEngine = ObjectFactory.GetInstance<IScheduleEngine>();

            scheduleEngine.UnScheduleBackup();

            scheduleEngine.ScheduleBackup();

            _logger.Debug("ScheduleBackupTasks End.");
        }

        public void RemoveScheduledBackupTasks()
        {
            _logger.Debug("RemoveScheduleBackupTasks Start.");

            var scheduleEngine = ObjectFactory.GetInstance<IScheduleEngine>();

            scheduleEngine.UnScheduleBackup();

            _logger.Debug("RemoveScheduleBackupTasks End.");
        }
    }
}
