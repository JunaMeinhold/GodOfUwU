namespace GodOfUwU.Core.Scheduling
{
    using System.Collections.Generic;

    public class TaskScheduler
    {
        static TaskScheduler()
        {
            Current = new();
            AppDomain.CurrentDomain.ProcessExit += ProcessExit;
        }

        private static void ProcessExit(object? sender, EventArgs e)
        {
            foreach (var task in Current.Tasks)
            {
                task.Stop();
            }
        }

        public static TaskScheduler Current { get; }

        public List<ScheduledTask> Tasks { get; } = new();
    }
}