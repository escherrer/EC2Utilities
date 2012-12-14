using CommandLine;
using CommandLine.Text;

namespace EC2Backup.Host.Console
{
    public class ConsoleOptions : CommandLineOptionsBase
    {
        [Option("i", "info", HelpText = "Prints configured backups.")]
        public bool Info { get; set; }

        [Option("b", "runBackups", HelpText = "Run the backups..")]
        public bool RunBackups { get; set; }

        [Option("s", "scheduleBackupTask", HelpText = "Creates a scheduled task to run and purge backups.")]
        public bool ScheduleBackupTasks { get; set; }

        [Option("u", "unScheduleBackupTask", HelpText = "Removes the backup scheduled task.")]
        public bool UnScheduleBackupTasks { get; set; }

        [Option("r", "runSilent", HelpText = "Closes after execution.")]
        public bool RunSilent { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
