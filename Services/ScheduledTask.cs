using Orchard.Environment;
using Orchard.Services;
using Orchard.Tasks.Scheduling;
using OrchardHUN.TrainingDemo.Events;
using System.Linq;

namespace OrchardHUN.TrainingDemo.Services
{
    // IScheduledTaskHandler implementations, unlike IBackgroundTasks, are not run periodically but at a given time.
    // See, now we also implement an external event handler, IOrchardShellEvents!
    // Interfaces are implemented explicitly only so you can see which method correpsond to which interface.
    public class ScheduledTask : IOrchardShellEvents, IScheduledTaskHandler
    {
        private readonly IBackgroundEventHandler _eventHandler;
        // Scheduled tasks are handled through this service. We need it as we now create a self-renewing task.
        private readonly IScheduledTaskManager _taskManager;
        // We'll need a clock for specifying a date. Remember, we use this back in DateTimeCachingService too!
        private readonly IClock _clock;

        // We have to give scheduled tasks a name. These names, like resource names should be globally unique, so such a
        // pattern will work.
        private const string TaskType = "OrchardHUN.TrainingDemo.ScheduledTask";


        public ScheduledTask(IBackgroundEventHandler eventHandler, IScheduledTaskManager taskManager, IClock clock)
        {
            _eventHandler = eventHandler;
            _taskManager = taskManager;
            _clock = clock;
        }


        // This runs very early when the Orchard instance spins up.
        void IOrchardShellEvents.Activated() =>
            // Scheduled tasks, as they are run at a given time, should be first registered. What we write now is a
            // self-renewing scheduled task, essentially a periodic background task where we choose the interval between
            // calls. Remember though that this is just a common scenario, but not the only one of course: another
            // scheduled task scenario would be to schedule a task for a future date and only run it once.
            CreateTaskIfNew(false);

        // When the Orchard instance is torn down, this event is fired. We don't need it now.
        void IOrchardShellEvents.Terminating() { }

        // This method will be called periodically (every minute) with the current task context. Because of this we have
        // to check if the current task is ours. (BTW this way the same taks handler can handle multiple tasks.) Remember
        // that although we scheduled the task to be run 3 minutes in the future it won't run in exactly 3 minutes, but
        // practically in a one minute time frame after the scheduled time, the worst case being one minute after what we
        // scheduled. Also if the Orchard instance is torn down before that (e.g. by an AppPool recycle) the task may run
        // at an even later date. Don't rely on backgroud tasks runnin exactly on schedule!
        void IScheduledTaskHandler.Process(ScheduledTaskContext context)
        {
            // Not our task.
            if (context.Task.TaskType != TaskType) return;

            // Check out in the debugger: this should be called every 3 minutes.
            _eventHandler.ScheduledTaskFired(new ScheduledTaskFiredContext { TaskType = TaskType });

            CreateTaskIfNew(true); // Renewing the task
        }


        // This helper method creates the task for us but only if is not already in the system. You should be aware that
        // the task type is not a unique key: multiple tasks with the same type can exist. Thus if there's an uncompleted
        // task in the system already (because e.g. Orchard was torn down before it could run) simply creating the task
        // would create a new one. This in turn would have the effect that our Process() method would run for both tasks,
        // resulting in more frequent execution than what we want.
        private void CreateTaskIfNew(bool calledFromTaskProcess)
        {
            var outdatedTaskCount = _taskManager.GetTasks(TaskType, _clock.UtcNow).Count();
            var taskCount = _taskManager.GetTasks(TaskType).Count();

            // calledFromTaskProcess is necessary as when this is called from Proces() the current task will still be in
            // the DB, thus there will be at least a single task there.
            if ((!calledFromTaskProcess || taskCount != 1) && taskCount != 0 && taskCount - outdatedTaskCount >= 0)
            {
                // If outdated tasks exists, don't create a new one.
                return;
            }

            // This task wil run in three minutes. The third parameter could be a content item that represents the
            // context for the task, but we don't need it now.
            _taskManager.CreateTask(TaskType, _clock.UtcNow.AddMinutes(3), null);

            // BTW this helper method and many more libraries aiding Orchard development are contained in the Helpful
            // Libraries module: https://github.com/Lombiq/Helpful-Libraries
        }
    }

    // NEXT STATION: Services/DynamicEventHandler! Wrapping up event handlers with this.
}