using Orchard.Events;

namespace OrchardHUN.TrainingDemo.Events
{
    /*
     * Events are special dependencies that should implement/derive from the IEventHandler marker interface.
     * 
     * Remember when we injected an IEnumerable<IPersonFilter> to the PersonManager service in order to run all the 
     * implementations of an interface? Event handlers can be used to achieve the same but are much more than that.
     * We have two implementations here for this interface, BackgroundTaskHandler and ScheduledTaskHandler. Take a quick 
     * look at them and come back!
     * Now go back to Services/BackgroundTask to see how deep the rabbit's hole is!
     */
    public interface IBackgroundEventHandler : IEventHandler
    {
        void BackgroundTaskFired();
        // You can also pass objects to events of course.
        void ScheduledTaskFired(ScheduledTaskFiredContext context);
    }


    // A very simple context object, but you get the idea :-).
    public class ScheduledTaskFiredContext
    {
        public string TaskType { get; set; }
    }
}
