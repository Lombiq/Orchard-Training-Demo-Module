/*
 * We'll write our first service, what is a dependency BTW.
 * We name it "PersonManager" because it sounds enterprisy and management will buy the idea of moving to Orchard. And also because it helps
 * discover business-critical sinergies ASAP.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard;
using Orchard.Data;
using OrchardHUN.TrainingDemo.Models;

namespace OrchardHUN.TrainingDemo.Services
{
    /*
     * We've seen that dependencies are requested through their interfaces (this way implementations can be changed). So we create our own
     * and derive it from the IDependency marker interface. This means it will get instantiated on each http request where it's requested
     * in a ctor. For 99% of cases you'll need IDependency.
     * Note: we've declared interface and implementation in the same code file here for the sake of simplicity, but normally you'd place them
     * into separate files.
     */
    public interface IPersonManager : IDependency
    {
        /// <summary>
        /// Retrieves all persons
        /// </summary>
        /// <remarks>
        /// If there's one thing not to learn from the Orchard code base it's the lack of inline documentation. Write comments, comments rock!
        /// BTW normally we wouldn't directly return records from a service. For the sake of our brain cells we no use this simplification.
        /// </remarks>
        IEnumerable<PersonRecord> GetPersons();

        /// <summary>
        /// Saves a person; if the person with the same name exists, modifies its data
        /// </summary>
        void SavePerson(string name, Sex sex, DateTime birthDateUtc, string biography);
    }

    public class PersonManager : IPersonManager
    {
        // IRepository<T> is the standard way of interacting with records. This is a low-level data access service.
        private readonly IRepository<PersonRecord> _personRepository;


        // IRepository is also a dependency
        public PersonManager(IRepository<PersonRecord> personRepository)
        {
            _personRepository = personRepository;
        }


        public IEnumerable<PersonRecord> GetPersons()
        {
            // Normally service methods are a bit more complex...
            // Feel freee to discover what IRepository offers. In the end you can use the Table property that's IQueryable so you have full
            // LINQ support.
            return _personRepository.Table;
        }

        public void SavePerson(string name, Sex sex, DateTime birthDateUtc, string biography)
        {
            // Let's also practice exception handling.
            if (String.IsNullOrEmpty(name)) throw new ArgumentNullException("name");

            var person = _personRepository.Fetch(record => record.Name == name).FirstOrDefault();

            if (person == null)
            {
                person = new PersonRecord();
                _personRepository.Create(person);
            }

            person.Name = name;
            person.Sex = sex;
            person.BirthDateUtc = birthDateUtc;
            person.Biography = biography;

            // NEXT STATION: Controllers/PersonController
        }
    }
}