/*
 * Now we'll see something very interesting!
 * Sometimes you want a specific job to be run automatically periodically or at a given time. Orchard has background 
 * tasks for this.
 */

using Orchard.Tasks;

namespace OrchardHUN.TrainingDemo.Services
{
    // IBackgroundTask implementations have one method: Sweep(). Orchard calls this every minute, so you can use this to
    // implement periodic background tasks.
    public class BackgroundTask : IBackgroundTask
    {
        // An event! Orchard also has an awesome event handling framework that's quite different from standard .NET
        // events and is loosely coupled. Check out the interface and come back here!
        private readonly IBackgroundEventHandler _eventHandler;


        // Notice that we inject a single IBackgroundEventHandler here although there can be (actually are) multiple
        // implementations. As we'll see in a moment, there is a good amount of magic involved.
        public BackgroundTask(IBackgroundEventHandler eventHandler) => _eventHandler = eventHandler;


        public void Sweep() =>
            // Calling into the event. Although we call the method on a single IBackgroundEventHandler object really this
            // method of all the implementations is called! Check out: this will fire every minute, so the respective
            // method of ScheduledTaskHandler and BackgroundTaskHandler will be called too.
            _eventHandler.BackgroundTaskFired();

        // NEXT STATION: Let's see Services/ScheduledTask! Some more background tasks will follow with more event
        // handling awesomeness.
    }
}