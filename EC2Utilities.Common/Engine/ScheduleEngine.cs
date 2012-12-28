using System;
using Microsoft.Win32.TaskScheduler;

namespace EC2Utilities.Common.Engine
{
    public class ScheduleEngine : IScheduleEngine
    {
        public void ScheduleBackup()
        {
            // Get the service on the local machine
            using (var ts = new TaskService())
            {
                // Create a new task definition and assign properties
                TaskDefinition td = ts.NewTask();
                td.RegistrationInfo.Description = "Runs EC2 backups.";

                // Create a trigger that will fire the task at this time every other day
                td.Triggers.Add(new DailyTrigger { DaysInterval = 1, StartBoundary = DateTime.Now.Date});

                // Create an action that will launch Notepad whenever the trigger fires
                td.Actions.Add(new ExecAction(GetExePath(), "-b -r"));

                // Register the task in the root folder
                ts.RootFolder.RegisterTaskDefinition(@"EC2 Backup", td);
            }
        }

        public void UnScheduleBackup()
        {
            // Get the service on the local machine
            using (var ts = new TaskService())
            {
                Task existingTask = ts.FindTask("EC2 Backup");

                if (null != existingTask)
                {
                    // Remove the task we just created
                    ts.RootFolder.DeleteTask(existingTask.Name);
                }
            }
        }

        private string GetExePath()
        {
            string result = System.Reflection.Assembly.GetEntryAssembly().Location;

            return result;
        }
    }
}
