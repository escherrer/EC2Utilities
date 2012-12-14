namespace EC2Backup.Common.Engine
{
    public interface IScheduleEngine
    {
        void ScheduleBackup();

        void UnScheduleBackup();
    }
}