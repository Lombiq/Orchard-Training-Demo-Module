using System;
using System.Linq;
using System.Web.Mvc;
using Orchard.Exceptions;
using OrchardHUN.TrainingDemo.Models;
using OrchardHUN.TrainingDemo.Services;

namespace OrchardHUN.TrainingDemo.Controllers
{
    public class PersonController : Controller
    {
        // We use our service, what is a dependency, as a dependency
        private readonly IPersonManager _personManager;


        public PersonController(IPersonManager personManager)
        {
            _personManager = personManager;
        }


        // Simple actions here, just for sake of demonstration

        // Here and a few times elsewhere in this module we have GET action that modify the state of the application
        // (e.g. writing to the DB or to the file system). Of course this is purely for the sake of simple demonstration
        // (so you can try them out quickly), in real life GETs should never alter the state of the application.
        public string CreateGoodPersons()
        {
            _personManager.SavePerson("Jacob Gips", Sex.Male, new DateTime(1977, 3, 14), "I was born on a damn farm in South-North Neverland.");
            _personManager.SavePerson("Naomi Concrete", Sex.Female, new DateTime(1979, 5, 12), "\"Always cite meaningful quotes.\" - Luke Skywalker");
            _personManager.SavePerson("James Ytong", Sex.Male, new DateTime(1989, 12, 4), "<insert subject biography here>");
            _personManager.SavePerson("Maria Brick", Sex.Female, new DateTime(1969, 10, 6), "Not much.");

            return "Persons saved.";
        }

        public string CreateBadPerson()
        {
            // Thinking about exceptions is a good practice!
            try
            {
                // Something obviously has gone wrong here...
                _personManager.SavePerson(null, Sex.Male, new DateTime(2077, 1, 1), "asdf");
            }
            // Catching the base Exception looks ugly, but strictly speaking you can't assume more: you only know the
            // service's interface! More info:
            // http://english.orchardproject.hu/blog/orchard-gems-exception-fatality-check
            // It will bubble through if it's e.g. an OutOfMemoryException.
            catch (Exception ex) when (!ex.IsFatal())
            {
                return "This is embarrassing, but... Somebody messed this method up.";
            }

            return "Great success!";
        }

        public string ListPersons()
        {
            var persons = _personManager.GetPersons();
            return string.Join("<br><br>", persons.Select(person => person.Name + ", " + person.Sex + ", " + person.BirthDateUtc +  ", " + person.Biography));
        }

        // NEXT STATION: let's create a content part and a content type! Go to Models/PersonListPart
    }
}