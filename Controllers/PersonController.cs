using Microsoft.AspNetCore.Modules;
using Microsoft.AspNetCore.Mvc;
using Orchard.ContentFields.Fields;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Display;
using Orchard.DisplayManagement.ModelBinding;
using OrchardHUN.TrainingDemo.Constants;
using OrchardHUN.TrainingDemo.Models;
using OrchardHUN.TrainingDemo.Services;
using System.Threading.Tasks;

namespace OrchardHUN.TrainingDemo.Controllers
{
    public class PersonController : Controller, IUpdateModel
    {
        private readonly IContentManager _contentManager;
        private readonly IContentItemDisplayManager _contentDisplay;
        private readonly IClock _clock;
        private readonly IPersonManager _personManager;


        public PersonController(IContentManager contentManager, IContentItemDisplayManager contentDisplay, IClock clock, IPersonManager personManager)
        {
            _contentManager = contentManager;
            _contentDisplay = contentDisplay;
            _clock = clock;
            _personManager = personManager;
        }


        public ActionResult Index()
        {
            return View();
        }

        public string CreateAddresses()
        {
            _personManager.SaveAddress("Redmond", "98052", "Washington Street");
            _personManager.SaveAddress("Budapest", "1051", "Orchard Road");

            return "Addresses saved.";
        }

        public string CreateGoodPersons()
        {
            _personManager.SavePerson("John Doe", Sex.Male, "https://lombiq.com", "Lombiq.com", _clock.UtcNow, "He is John Doe!");
            _personManager.SavePerson("Jane Doe", Sex.Female, "https://lombiq.com", "Lombiq.com", _clock.UtcNow, "John Does wife!");

            //var johnDoePersonContentItem = _contentManager.New(nameof(Person));

            //_contentManager.Create(johnDoePersonContentItem);

            //// Explicit syntax
            //var workAddress = johnDoePersonContentItem.Get<AddressPart>("WorkAddress");
            //workAddress.City = "Redmond";
            //workAddress.ZipCode = "98052";
            //workAddress.Address = "Washington Street";

            //// Dynamic syntax
            //johnDoePersonContentItem.Content.HomeAddress.City = "Budapest";
            //johnDoePersonContentItem.Content.HomeAddress.ZipCode = "1051";
            //johnDoePersonContentItem.Content.HomeAddress.Address = "Orchard Road";

            //// "Alter" syntax
            //johnDoePersonContentItem.Alter<PersonPart>(person => person.Name.Text = "John Doe");
            //johnDoePersonContentItem.Alter<PersonPart>(person => person.Sex.Text = Sex.Male.ToString());
            //johnDoePersonContentItem.Alter<PersonPart>(person => person.Biography.Text = "He is John Doe!");

            //johnDoePersonContentItem.Apply(workAddress);

            //var janeDoeContentItem = _contentManager.New(nameof(Person));

            //janeDoeContentItem.Alter<PersonPart>(person => person.BirthDateUtc = _clock.UtcNow);
            //janeDoeContentItem.Alter<PersonPart>(person => person.Name.Text = "Jane Doe");

            //var homeAddress = janeDoeContentItem.Get<AddressPart>("HomeAddress");
            //homeAddress.City = "Budapest";
            //homeAddress.ZipCode = "1021";
            //homeAddress.Address = "Moscow Square";

            //janeDoeContentItem.Apply(homeAddress);

            //// Adding a new field
            //janeDoeContentItem.Alter<PersonPart>(x =>
            //{
            //    x.GetOrCreate<TextField>(ContentFieldNames.Comment);
            //    x.Alter<TextField>(ContentFieldNames.Comment, f => f.Text = "The famous Jane Doe!");
            //});

            //_contentManager.Create(janeDoeContentItem);

            return "Persons saved.";
        }

        public string AddAddressToPerson()
        {
            _personManager.AddAddressToPerson("45ze6r2c3n22r4wpthk1y4a35z", "4n07mf7vef24ky6hymr1msa6mw", AddressType.HomeAddress);
            _personManager.AddAddressToPerson("47t1fg95h7t2d54bxckyq06xnm", "4n07mf7vef24ky6hymr1msa6mw", AddressType.WorkAddress);

            return "Addresses added to person.";
        }

        public string ListPersons()
        {
            return "";
        }

        public string CreateBadPersons()
        {
            return "";
        }

        public async Task<ActionResult> Display(string contentItemId)
        {
            var contentItem = await _contentManager.GetAsync(contentItemId);

            if (contentItem == null)
            {
                return NotFound();
            }

            var shape = await _contentDisplay.BuildDisplayAsync(contentItem, this);
            return View(shape);
        }
    }
}
