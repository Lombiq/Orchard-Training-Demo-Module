/*
 * We'll learn a bit of unit testing here. If you don't really know what unit testing is about please study the topic
 * first. Integration testing is technically very similar for Orchard services just instead of testing only a single
 * service we add some more actual implementations into the mix.
 *
 * First, we'll create a service here that we'll then later test in a test project. This service won't be used anywhere
 * else, it's just an example to be tested.
 *
 * Why a service? Services are where usually most of the complex logic of an Orchard-based web app goes. You can test
 * anything as long as you've written it in a testable way (by for example, not utilizing hidden dependencies but
 * injecting them all), you can write tests for controllers, drivers, background tasks, you name it. However, we think
 * that unless you're aiming for 100% test coverage it's best to focus your unit and integration testing efforts on
 * services. Then, the rest of the app can further automatically tested via e.g. UI tests.
 */

using OrchardCore.ContentManagement;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Lombiq.TrainingDemo.Services
{
    // First we declare the interface of our service (as we've previously also done with IDateTimeCachingService). This
    // way other classes using the service will be able to inject it and depend on the interface instead of the
    // implementation, making them testable too!
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "Just a simple sample.")]
    public interface ITestedService
    {
        // Just a simple sample method that will retrieve a content item or throw an exception if it can't be found.
        Task<ContentItem> GetContentItemOrThrowAsync(string id);
    }


    // The implementation of the service follows.
    public class TestedService : ITestedService
    {
        private readonly IContentManager _contentManager;


        public TestedService(IContentManager contentManager) => _contentManager = contentManager;


        public Task<ContentItem> GetContentItemOrThrowAsync(string id)
        {
            // As you can see we rigorously check the input. Something we'll surely need to test later!
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id), "The supplied content item ID was null or empty.");
            }

            // This is factored out to adhere to the recommendations here: https://rules.sonarsource.com/csharp/RSPEC-4457.
            return GetContentItemOrThrowInternalAsync(id);
        }


        private async Task<ContentItem> GetContentItemOrThrowInternalAsync(string id)
        {
            // You already know how this works :).
            var contentItem = await _contentManager.GetAsync(id);

            // Checking content retrievals for null is always a good idea.
            if (contentItem == null)
            {
                throw new InvalidOperationException($"The content item with the ID {id} doesn't exist.");
            }

            return contentItem;
        }
    }
}

// NEXT STATION: Now that we have the service done, let's write some tests for it! These will be in a separate project
// because usually you don't want to mix tests and production code. So head over to the Lombiq.TrainingDemo.Tests
// project!
