/*
 * In this demonstration we'll implement a background task that will run 5 times every 2 minutes and will write a
 * message in the error log. In Orchard Core background tasks always run periodically meaning if you want them to stop
 * you need to explicitly disable them from code or from the Background Tasks admin page if the
 * OrchardCore.BackgroundTasks feature is enabled. For scheduling each task Orchard Core uses NCrontab
 * (https://github.com/atifaziz/NCrontab) providing functionality to specify crontab expressions. By default tasks are
 * set to be executed every 5 minutes - and as mentioned they'll run until you disable them. If you want to learn more
 * about crontab expressions you can use this online tool: https://crontab.guru/.
 */

using Microsoft.Extensions.Logging;
using OrchardCore.BackgroundTasks;
using System;
using System.Threading;
using System.Threading.Tasks;
using OrchardCore.BackgroundTasks.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Lombiq.TrainingDemo.Services
{
    // To set up the default settings for your background task you can use the BackgroundTask attribute. Here you can
    // specify the frequency using crontab expressions and setting a description which will be displayed on the
    // Background Tasks admin page. Also you can set Enabled to false if you don't want it start right after
    // application start. These settings can be updated entirely on the Background Tasks admin page.
    [BackgroundTask(Schedule = "*/2 * * * *", Description = "Demo background task that runs every 2 minutes.")]
    public class DemoBackgroundTask : IBackgroundTask
    {
        // Setting a maximum time this background task will be executed.
        private const int MaxCount = 5;

        // Storing execution times in a private field. Since background tasks are singleton objects this will keep its
        // value while the application runs.
        private int _count;
        private readonly ILogger<DemoBackgroundTask> _logger;


        public DemoBackgroundTask(ILogger<DemoBackgroundTask> logger)
        {
            _logger = logger;
        }


        public async Task DoWorkAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            // This is where the task is implemented. Increment _count and print it to the error log with a message.
            // Notice that there is a GetTaskName() extension method for IBackgroundTask which will return the
            // technical name of the task.
            _logger.LogError($"{++_count}/{MaxCount}: Hello from {this.GetTaskName()}!");

            if (_count == MaxCount)
            {
                // If it reached the maximum reset it to 0.
                _count = 0;

                // Here is an example of disabling a background task programmatically using BackgroundTaskManager. You
                // can use this method from anywhere in the code base. Here you need to get this service from the
                // service provider since it's a scoped service.
                var backroundTaskManager = serviceProvider.GetService<BackgroundTaskManager>();

                // The GetDefaultSettings() extension method will return the settings you defined in the BackgroundTask
                // attribute.
                var settings = this.GetDefaultSettings();

                // By setting Enabled to false and using IBackgroundTaskManager to update it the settings will be
                // stored in the database (or updated if it has already been stored) and from now on Orchard Core will
                // ignore this task and it won't be executed.
                settings.Enable = false;
                await backroundTaskManager.UpdateAsync(settings.Name, settings);
            }
        }
    }
}

// END OF TRAINING: Background tasks

// NEXT STATION: Controllers/VueJsController.cs