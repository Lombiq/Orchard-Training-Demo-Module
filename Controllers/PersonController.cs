using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OrchardHUN.TrainingDemo.Models;
using OrchardHUN.TrainingDemo.Services;
using Orchard.Exceptions;

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
            // Catching the base Exception looks ugly, but strictly speaking you can't assume more: you only know the service's interface!
            // More info: http://english.orchardproject.hu/blog/orchard-gems-exception-fatality-check
            catch (Exception ex)
            {
                // Rethrow if e.g. OutOfMemoryException
                if (ex.IsFatal()) throw;

                return "This is embarrassing, but... Somebody messed this method up.";
            }

            return "Great success!";
        }

        public string ListPersons()
        {
            var persons = _personManager.GetPersons();
            return String.Join("<br><br>", persons.Select(person => person.Name + ", " + person.Sex + ", " + person.BirthDateUtc +  ", " + person.Biography));
        }

        // NEXT STATION: 
    }
}