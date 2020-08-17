using Lombiq.TrainingDemo.Services;
using Moq;
using Moq.AutoMock;
using OrchardCore.ContentManagement;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.TrainingDemo.Tests.Services
{
    // This will be our test class. If you're familiar with unit testing in xUnit then not much new will be here:
    // Testing services from Orchard modules is just any unit testing.
    public class TestedServiceTests
    {
        private const string TestContentId = "content ID";


        // Here we have a nice data-driven test. If you're new to this check out this blog post:
        // https://andrewlock.net/creating-parameterised-tests-in-xunit-with-inlinedata-classdata-and-memberdata/
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void NullOrEmptyArgumentsShouldThrow(string id)
        {
            var service = CreateTestedService(out _);

            // First we'll test that the service checks the input properly. Note that we're using the Shouldly library
            // for nice assertions.

            Should.Throw<ArgumentNullException>(() => service.GetContentItemOrThrowAsync(id));
        }

        [Fact]
        public void NonExistingContentItemsShouldThrow()
        {
            var service = CreateTestedService(out var mocker);

            // Let's see if the service properly throws an exception if there is no matching content item. Note that
            // AutoMocker will inject an IContentManager mock that'll by default return the default values of every
            // type, so IContentManager.GetAsync() will return null wrapped into a Task.

            Should.Throw<InvalidOperationException>(() => service.GetContentItemOrThrowAsync(TestContentId));

            // Let's also make sure that the content manager method was actually called, and with the correct
            // parameter.

            mocker
                .GetMock<IContentManager>()
                .Verify(contentManager => contentManager.GetAsync(It.Is<string>(id => id == TestContentId)));
        }

        [Fact]
        public async Task ContentItemsAreRetrieved()
        {
            // In this test we'll mock IContentManager so it actually returns something we can then verify.

            var service = CreateTestedService(out var mocker);

            // Setting up an IContentManager mock that'll return a basic ContentItem placeholder.
            mocker
                .GetMock<IContentManager>()
                .Setup(contentManager => contentManager.GetAsync(It.IsAny<string>()))
                .ReturnsAsync<string, IContentManager, ContentItem>(id => new ContentItem { ContentItemId = id });

            var contentItem = await service.GetContentItemOrThrowAsync(TestContentId);

            contentItem.ContentItemId.ShouldBe(TestContentId);
        }


        private static TestedService CreateTestedService(out AutoMocker mocker)
        {
            // We're using a library called AutoMocker here. It extends the Moq mocking library with the ability to
            // automatically substitute injected dependencies with a mocked instance. It's a bit like a special
            // dependency injection container. This way, your tested classes will get all their dependencies injected
            // even if you don't explicitly register a mock or stub for the.

            mocker = new AutoMocker();
            return mocker.CreateInstance<TestedService>();
        }
    }
}

// END OF TRAINING SECTION: Unit and integration testing

// NEXT STATION: Head back to the module project and check out Controllers/VueJsController.cs
