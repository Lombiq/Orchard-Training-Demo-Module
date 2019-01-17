/*
 * In this controller you will see again how to query items but this time items will be of the newly created Person
 * content type. It doesn't make too much difference but you need to keep in mind that the ContentItems are stored in
 * the documents (which contain the parts and fields serialized) and can have multiple index records referencing a
 * content item (e.g. the previously created PersonPartIndex that indexes data in from PersonPart).
 *
 * Note that there is no custom controller or action demonstrated for displaying the editor for the Person. Go to the
 * administration page (/Admin) and create a few Person content items, including ones with ages above 30.
 */

using System.Linq;
using System.Threading.Tasks;
using Lombiq.TrainingDemo.Indexes;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.Modules;
using YesSql;

namespace Lombiq.TrainingDemo.Controllers
{
    public class PersonListController : Controller, IUpdateModel
    {
        private readonly ISession _session;
        private readonly IClock _clock;
        private readonly IContentItemDisplayManager _contentItemDisplayManager;


        public PersonListController(
            ISession session,
            IClock clock,
            IContentItemDisplayManager contentItemDisplayManager)
        {
            _session = session;
            _clock = clock;
            _contentItemDisplayManager = contentItemDisplayManager;
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
                await _contentItemDisplayManager.BuildDisplayAsync(person, this, "Summary")));

            // Now assuming that you've already created a few Person content items on the dashboard and some of
            // these persons are more than 30 years old then this query will contain items to display.
            // NEXT STATION: Views/PersonList/OlderThan30.cshtml
            
            return View(shapes);
        }
    }
}