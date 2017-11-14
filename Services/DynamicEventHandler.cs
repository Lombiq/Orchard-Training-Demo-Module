/*
 * Events are more than just a convenient way of calling into all the registered dependencies. Actually calls to event 
 * go through the so-called Orchard Event Bus (see: http://docs.orchardproject.net/Documentation/How-Orchard-works#EventBus). 
 * A method call is not just a simple method call but really a message broadcasted on the event bus. This makes it 
 * possible such crazy things like handling an event without having a reference to it!
 */

using Orchard.Events;

namespace OrchardHUN.TrainingDemo.Services
{
    // We're in the services namespace and have no using declaration for the Events namespace. But still we can handle
    // IBackgroundEventHandler events!

    // To hook into events that should be handled by IBackgroundEventHandler we don't necessarily have to implement 
    // OrchardHUN.TrainingDemo.Events.IBackgroundEventHandler. It's enough if we have an interface with the same name!
    public interface IBackgroundEventHandler : IEventHandler
    {
        void BackgroundTaskFired();
        // Remember that the context is of type ScheduledTaskFiredContext? Since we - supposedely - don't have a
        // reference to ScheduledTaskFiredContext we can simply use dynamic as a type here.
        void ScheduledTaskFired(dynamic context);
    }


    // Despite that DynamicEventHandler doesn't implement the original IBackgroundEventHandler the event methods will be
    // called.
    public class DynamicEventHandler : IBackgroundEventHandler
    {
        public void BackgroundTaskFired()
        {
            var fired = true;
            var notFired = !fired;
        }

        public void ScheduledTaskFired(dynamic context)
        {
            var fired = true;
            var notFired = !fired;
        }
    }



    /*
     * This is not very useful here as the event handler interface is in the same module, but imagine a scenario where 
     * the event handler interfaces are in a separate module. Now in your own module you could reference that other 
     * module - but you don't have to! Using the pattern described here you can handle event that are raised from a module 
     * you don't even have a reference for! Such very loose coupling is useful e.g. if you don't want to force the users 
     * of your module to install your module's dependency: one of your module's feature can hook into the other module 
     * and thus depend on it but if the user doesn't want to use that feature it's not mandatory for her/him to install 
     * that other module (in contrary to having a .NET project reference).
    */


    // NEXT STATION: check out how to write Owin middlewares with OwinMiddleware!
}