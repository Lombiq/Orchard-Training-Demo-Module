/*
 * In this controller you will see again how to query items but this time items will be of the newly created Person
 * content type. It doesn't make too much difference but you need to keep in mind that the ContentItems are stored in
 * the documents (which contain the parts and fields serialized) and can have multiple index records referencing a
 * content item (e.g. the previously created PersonPartIndex that indexes data in from PersonPart).
 *
 * Note that there is no custom controller or action demonstrated for displaying the editor for the Person. Go to the
 * administration page (/Admin) and create a few Person content items, including ones with ages above 30.
 */

using Lombiq.TrainingDemo.Indexes;
using Lombiq.TrainingDemo.Models;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.Modules;
using System.Linq;
using System.Threading.Tasks;
using YesSql;

namespace Lombiq.TrainingDemo.Controllers
{
    public class PersonListController : Controller
    {
        private readonly ISession _session;
        private readonly IClock _clock;
        private readonly IContentItemDisplayManager _contentItemDisplayManager;
        private readonly IUpdateModelAccessor _updateModelAccessor;
        private readonly IContentManager _contentManager;


        public PersonListController(
            ISession session,
            IClock clock,
            IContentItemDisplayManager contentItemDisplayManager,
            IUpdateModelAccessor updateModelAccessor,
            IContentManager contentManager)
        {
            _session = session;
            _clock = clock;
            _contentItemDisplayManager = contentItemDisplayManager;
            _updateModelAccessor = updateModelAccessor;
            _contentManager = contentManager;
        }


        // See it under /Lombiq.TrainingDemo/PersonList/OlderThan30.
        public async Task<ActionResult> OlderThan30()
        {
            var thresholdDate = _clock.UtcNow.AddYears(-30);
            var people = await _session
                // This will query for content items where the related PersonPartIndex.BirthDateUtc is lower than the
                // threshold date. Notice that there is no Where method. The Query method has an overload for that
                // which can be useful if you don't want to filter in multiple indexes.
                .Query<ContentItem, PersonPartIndex>(index => index.BirthDateUtc < thresholdDate)
                .ListAsync();

            // Now let's build the display shape for a content item! Notice that this is not the IDisplayManager
            // service. The IContentItemDisplayManager is an abstraction over that and it's specifically for content
            // items. The reason we need that is that a ContentItem doesn't have a DisplayDriver but the ContentParts
            // and ContentFields attached to the ContentItem have. This service will handle generating all the drivers
            // created for these parts and fields.
            // NEXT STATION: Drivers/PersonPartDisplayDriver
            var shapes = await Task.WhenAll(people.Select(async person =>
            {
                // When you retrieve content items via ISession then you also need to run LoadAsync() on them to
                // initialize everything.
                await _contentManager.LoadAsync(person);

                return await _contentItemDisplayManager.BuildDisplayAsync(person, _updateModelAccessor.ModelUpdater, "Summary");
            }));

            // Now assuming that you've already created a few Person content items on the dashboard and some of these
            // persons are more than 30 years old then this query will contain items to display.
            // NEXT STATION: Go to Views/PersonList/OlderThan30.cshtml and then come back here please.

            return View(shapes);
        }

        // Check out the result under /Lombiq.TrainingDemo/PersonList/FountainOfEternalYouth.
        public async Task<string> FountainOfEternalYouth()
        {
            // Here we'll modify content items directly from code and in the meantime we'll learn a lot of new things.

            // Again we'll fetch content items with PersonPart but this time we'll retrieve old people and we'll make
            // them younger!
            var thresholdDate = _clock.UtcNow.AddYears(-90);
            var oldPeople = (await _session
                .Query<ContentItem, PersonPartIndex>(index => index.BirthDateUtc < thresholdDate)
                .ListAsync())
                .ToList();

            foreach (var person in oldPeople)
            {
                // Have to run LoadAsync() here too.
                await _contentManager.LoadAsync(person);

                var eighteenYearOld = _clock.UtcNow.AddYears(-18);

                // Don't just overwrite the part's property directly! That'll change the index record but not the
                // document! Don't just do this:
                ////person.As<PersonPart>().BirthDateUtc = eighteenYearOld;
                // Instead, use Alter() as we do below:
                person.Alter<PersonPart>(part => part.BirthDateUtc = eighteenYearOld);

                // Once you're done you have to save the content item explicitly. Remember when we saved Books with
                // ISession.Save()? This is something similar for content items.
                await _contentManager.UpdateAsync(person);

                // After saving the content item with UpdateAsync() you also need to publish it to make sure that even
                // a draftable content item gets updated.
                await _contentManager.PublishAsync(person);
            }

            // If you want to manage just one content item or a couple of them that you know by ID then fetch them
            // with IContentManager.GetAsync() instead.

            return "People modified: " +
                (oldPeople.Any() ?
                    string.Join(", ", oldPeople.Select(person => person.As<PersonPart>().Name)) :
                    "Nobody. Did you create people older than 90?");

            // That was a quick intro to modifying content items from code. It's a lot more involved than this but
            // this should get you going!

            // There is one final piece missing to make what we need to know about content items complete. NEXT
            // STATION: Check out Handlers/PersonPartHandler.
        }
    }
}
