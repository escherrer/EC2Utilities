namespace EC2Utilities.Common.Manager
{
    public interface IScheduleManager
    {
        void ScheduleBackupTasks();

        void RemoveScheduledBackupTasks();
    }
}
