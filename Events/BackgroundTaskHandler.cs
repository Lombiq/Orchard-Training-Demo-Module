namespace OrchardHUN.TrainingDemo.Events
{
    public class BackgroundTaskHandler : IBackgroundEventHandler
    {
        public void BackgroundTaskFired()
        {
            // We hope you're entertained :-).
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