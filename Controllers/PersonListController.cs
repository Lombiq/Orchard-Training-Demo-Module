using System.Linq;
using System.Threading.Tasks;
using Lombiq.TrainingDemo.Indexes;
using Lombiq.TrainingDemo.Models;
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
        private readonly IContentManager _contentManager;


        public PersonListController(
            ISession session,
            IClock clock,
            IContentItemDisplayManager contentItemDisplayManager,
            IContentManager contentManager)
        {
            _session = session;
            _clock = clock;
            _contentItemDisplayManager = contentItemDisplayManager;
            _contentManager = contentManager;
        }


        public async Task<ActionResult> OlderThan30()
        {
            var thresholdDate = _clock.UtcNow.AddYears(-30);
            var people = await _session
                .Query<ContentItem, PersonPartIndex>(index => index.BirthDateUtc < thresholdDate)
                .ListAsync();

            var shapes = await Task.WhenAll(people.Select(async person =>
                await _contentItemDisplayManager.BuildDisplayAsync(person, this, "Summary")));

            // Testing 1..2..3...

            var person1 = await _contentManager.NewAsync("Person");
            var personPart1 = person1.As<PersonPart>();
            personPart1.Name = "Person 1";

            var person2 = await _contentManager.NewAsync("Person");
            var personPart2 = person2.As<PersonPart>();
            personPart2.Name = "Person 2";


            return View(shapes);
        }
    }
}