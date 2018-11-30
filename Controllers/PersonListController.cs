using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lombiq.TrainingDemo.Indexes;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display;
using OrchardCore.ContentManagement.Records;
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


        public async Task<ActionResult> OlderThan30()
        {
            var thresholdDate = _clock.UtcNow.AddYears(-30);
            var people = await _session
                .Query<ContentItem, PersonPartIndex>(index => index.BirthDateUtc < thresholdDate)
                .ListAsync();

            var shapes = people
                .Select(async person => await _contentItemDisplayManager.BuildDisplayAsync(person, this, "Summary"))
                .Select(task => task.Result);

            return View(shapes);
        }
    }
}
