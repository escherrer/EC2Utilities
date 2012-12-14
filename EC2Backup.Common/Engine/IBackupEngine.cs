namespace EC2Backup.Common.Engine
{
    public interface IBackupEngine
    {
        void BackupInstances();

        void PurgeBackups(int backupRetentionDays);
    }
}