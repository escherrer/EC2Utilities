namespace EC2Backup.Common.Manager
{
    public interface IScheduleManager
    {
        void ScheduleBackupTasks();

        void RemoveScheduledBackupTasks();
    }
}
