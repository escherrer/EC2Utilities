namespace EC2Utilities.Common.Engine
{
    public interface IBackupEngine
    {
        void BackupInstances();

        void PurgeBackups(int backupRetentionDays);
    }
}