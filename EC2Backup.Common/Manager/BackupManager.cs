using System;
using EC2Backup.Common.Engine;
using EC2Backup.Common.ResourceAccess;
using NLog;

namespace EC2Backup.Common.Manager
{
    public class BackupManager : IBackupManager
    {
        private readonly Logger _logger;
        private readonly IBackupEngine _backupEngine;
        private readonly IConfigResourceAccess _configResourceAccess;

        public BackupManager(Logger logger, IBackupEngine backupEngine, IConfigResourceAccess configResourceAccess)
        {
            _logger = logger;
            _backupEngine = backupEngine;
            _configResourceAccess = configResourceAccess;
        }

        public void RunBackups()
        {
            _logger.Trace("RunBackups Start.");

            _backupEngine.BackupInstances();

            int backupRetentionDays = _configResourceAccess.GetBackupRetentionDays();

            _backupEngine.PurgeBackups(backupRetentionDays);

            _logger.Trace("RunBackups End.");
        }
    }
}