using Lombiq.TrainingDemo.Indexes;
using Lombiq.TrainingDemo.Models;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Records;
using OrchardCore.DisplayManagement;
using OrchardCore.DisplayManagement.ModelBinding;
using System;
using System.Threading.Tasks;
using YesSql;

namespace Lombiq.TrainingDemo.Controllers
{
    public class PersonListController : Controller, IUpdateModel
    {
        private readonly ISession _session;


        public PersonListController(ISession session)
        {
            _session = session;
        }
        
        public async Task<ActionResult> Index()
        {
            var persons = await _session
                .Query<ContentItem, PersonPartIndex>(index => index.BirthDateUtc < new DateTime(1990, 1, 1), true)
                .With<ContentItemIndex>(index => index.ContentType == "Person")
                .ListAsync();

            //var personShapesFactory = new 

            return View(persons);
        }
    }
}