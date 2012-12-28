using System;
using System.Collections.Generic;
using CommandLine;
using EC2Utilities.Common.Factory;
using EC2Utilities.Common.Manager;
using EC2Utilities.Common.ResourceAccess;
using NLog;
using StructureMap;

namespace EC2Utilities.Host.Console
{
    class Program
    {
        private static Logger _logger;

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;

            ContainerBootstrapper.BootstrapStructureMap();

            _logger = ObjectFactory.GetInstance<Logger>();

            ProcessArgs(args);
        }

        private static void ProcessArgs(string[] args)
        {
            _logger.Info("Processing args: {0}", string.Join(",", args));

            var options = new ConsoleOptions();
            ICommandLineParser parser = new CommandLineParser();

            if (parser.ParseArguments(args, options) && args.Length > 0)
            {
                if (options.ScheduleBackupTasks)
                {
                    var scheduleEngine = ObjectFactory.GetInstance<IScheduleManager>();

                    scheduleEngine.ScheduleBackupTasks();
                }

                if (options.UnScheduleBackupTasks)
                {
                    var scheduleEngine = ObjectFactory.GetInstance<IScheduleManager>();

                    scheduleEngine.RemoveScheduledBackupTasks();
                }

                if (options.RunBackups)
                {
                    var backupManager = ObjectFactory.GetInstance<IBackupManager>();

                    backupManager.RunBackups();
                }

                if (options.Info)
                {
                    var configResourceAccess = ObjectFactory.GetInstance<IConfigResourceAccess>();

                    System.Console.WriteLine("Amount of days to keep backups: {0}", configResourceAccess.GetBackupRetentionDays());
                    System.Console.WriteLine("Press any key to continue...");
                    System.Console.ReadKey();
                }
            }
            else
            {
                System.Console.WriteLine(options.GetUsage());
            }

            if (!options.RunSilent)
            {
                System.Console.WriteLine("Input arg or Enter to exit.");

                var input = System.Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(input))
                {
                    var newArgs = new List<string>();
                    newArgs.Add(input);

                    ProcessArgs(newArgs.ToArray());
                }
            }
        }

        static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                var logger = LogManager.GetLogger("Program");
                logger.Fatal("Unhandled exception: " + e.ExceptionObject);
            }
            catch
            { }

            System.Console.WriteLine(e.ExceptionObject.ToString());
            System.Console.WriteLine("Press Enter to continue");
            System.Console.ReadLine();
            Environment.Exit(1);
        }
    }
}

