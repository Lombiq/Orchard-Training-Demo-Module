using System;
using System.Threading;
using System.Threading.Tasks;
using Orchard.BackgroundTasks;

namespace OrchardHUN.TrainingDemo.Services
{
    public class TestBackgroundTask : IBackgroundTask
    {
        private int _count;

        public Task DoWorkAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            _count++;

            return Task.CompletedTask;
        }
    }
}
