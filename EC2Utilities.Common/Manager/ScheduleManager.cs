using EC2Utilities.Common.Engine;
using NLog;
using StructureMap;

namespace EC2Utilities.Common.Manager
{
    public class ScheduleManager : IScheduleManager
    {
        private readonly Logger _logger;

        public ScheduleManager(Logger logger)
        {
            _logger = logger;
        }

        public void ScheduleBackupTasks()
        {
            _logger.Trace("ScheduleBackupTasks Start.");

            var scheduleEngine = ObjectFactory.GetInstance<IScheduleEngine>();

            scheduleEngine.UnScheduleBackup();

            scheduleEngine.ScheduleBackup();

            _logger.Trace("ScheduleBackupTasks End.");
        }

        public void RemoveScheduledBackupTasks()
        {
            _logger.Trace("RemoveScheduleBackupTasks Start.");

            var scheduleEngine = ObjectFactory.GetInstance<IScheduleEngine>();

            scheduleEngine.UnScheduleBackup();

            _logger.Trace("RemoveScheduleBackupTasks End.");
        }
    }
}
