using EC2Utilities.Common.Engine;
using EC2Utilities.Common.ResourceAccess;
using log4net;

namespace EC2Utilities.Common.Manager
{
    public class BackupManager : IBackupManager
    {
        private static readonly ILog Logger = LogManager.GetLogger("BackupManager");
        private readonly IBackupEngine _backupEngine;
        private readonly IConfigResourceAccess _configResourceAccess;

        public BackupManager(IBackupEngine backupEngine, IConfigResourceAccess configResourceAccess)
        {
            _backupEngine = backupEngine;
            _configResourceAccess = configResourceAccess;
        }

        public void RunBackups()
        {
            Logger.Debug("RunBackups Start.");

            _backupEngine.BackupInstances();

            int backupRetentionDays = _configResourceAccess.GetBackupRetentionDays();

            _backupEngine.PurgeBackups(backupRetentionDays);

            Logger.Debug("RunBackups End.");
        }
    }
}