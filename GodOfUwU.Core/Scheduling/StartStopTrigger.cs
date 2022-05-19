namespace GodOfUwU.Core.Scheduling
{
    using System;
    using System.Threading.Tasks;

    public class StartStopTrigger : ITrigger
    {
        public event Func<Task>? Triggered;

        public bool TriggerOnStart { get; set; }

        public bool TriggerOnStop { get; set; }

        public void OnStart()
        {
            if (TriggerOnStart)
                Triggered?.Invoke();
        }

        public void OnStop()
        {
            if (TriggerOnStop)
                Triggered?.Invoke();
        }
    }
}