namespace OrchardHUN.TrainingDemo.Events
{
    // You could ask: what's a scheduled task? We'll soon see :-).
    public class ScheduledTaskHandler : IBackgroundEventHandler
    {
        public void BackgroundTaskFired()
        {
            var fired = true;
            var notFired = !fired;

            // The above code is here only so you can set a breakpoint there and check in the debugger when the method is
            // fired.
        }

        public void ScheduledTaskFired(ScheduledTaskFiredContext context)
        {
            var fired = true;
            var notFired = !fired;
        }
    }
}