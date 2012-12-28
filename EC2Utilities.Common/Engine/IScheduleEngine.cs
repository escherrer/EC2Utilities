namespace EC2Utilities.Common.Engine
{
    public interface IScheduleEngine
    {
        void ScheduleBackup();

        void UnScheduleBackup();
    }
}