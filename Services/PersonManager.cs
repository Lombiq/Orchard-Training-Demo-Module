/*
 * We'll write our first service, what is a dependency BTW.
 * We name it "PersonManager" because it sounds enterprisy and management will buy the idea of moving to Orchard. And 
 * also because it helps discover business-critical sinergies ASAP.
 */

using Orchard;
using Orchard.Data;
using OrchardHUN.TrainingDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrchardHUN.TrainingDemo.Services
{
    /*
     * We've seen that dependencies are requested through their interfaces (this way implementations can be changed). 
     * So we create our own and derive it from the IDependency marker interface. This means it will get instantiated on 
     * each HTTP-request where it's requested in a ctor. For 99% of cases you'll need IDependency.
     * Note: we've declared interface and implementation in the same code file here for the sake of simplicity, but 
     * normally you'd place them into separate files.
     */
    public interface IPersonManager : IDependency
    {
        /// <summary>
        /// Retrieves all persons
        /// </summary>
        /// <remarks>
        /// If there's one thing not to learn from the Orchard code base it's the lack of inline documentation. Write
        /// comments, comments rock! BTW normally we wouldn't directly return records from a service. For the sake of our
        /// brain cells we no use this simplification.
        /// </remarks>
        IEnumerable<PersonRecord> GetPersons();

        /// <summary>
        /// Retrieves all persons having the specified sex
        /// </summary>
        /// <param name="sex">Sex to filter on</param>
        /// <param name="maxCount">Maximal number of persons retrieved</param>
        IEnumerable<PersonRecord> GetPersons(Sex sex, int maxCount);

        /// <summary>
        /// Saves a person; if the person with the same name exists, modifies its data
        /// </summary>
        void SavePerson(string name, Sex sex, DateTime birthDateUtc, string biography);
    }


    public class PersonManager : IPersonManager
    {
        // IRepository<T> is the standard way of interacting with records. This is a low-level data access service.
        private readonly IRepository<PersonRecord> _personRepository;
        private readonly IEnumerable<IPersonFilter> _filters;


        /* IRepository is also a dependency
         * 
         * Take a look at IPersonFilter and its implementations at the bottom. As you can see we have multiple 
         * implementations for the same interface. If we would inject just an IPersonFilter we'd get an instance of the 
         * last implementation registered; with injecting an IEnumerable<IPersonFilter> we get all the implementations.
         * Later we'll also see another technique Orchard makes possible for such event handlers.
         */
        public PersonManager(IRepository<PersonRecord> personRepository, IEnumerable<IPersonFilter> filters)
        {
            _personRepository = personRepository;
            _filters = filters;
        }


        public IEnumerable<PersonRecord> GetPersons() =>
            // Normally service methods are a bit more complex... Feel freee to discover what IRepository offers. In the
            // end you can use the Table property that's IQueryable so you have full LINQ support.
            _personRepository.Table;

        // We'll need this later for PersonListPart.
        public IEnumerable<PersonRecord> GetPersons(Sex sex, int maxCount) =>
            // _personRepository.Fetch(record => record.Sex == sex).Take(maxCount) would produce the same result. However
            // since Fetch() returns an IEnumerable, not an IQueryable, Take() would run on objects, not translated to
            // SQL. Hence the below version can perform better.
            _personRepository.Table.Where(record => record.Sex == sex).Take(maxCount);

        public void SavePerson(string name, Sex sex, DateTime birthDateUtc, string biography)
        {
            // Let's also practice exception handling.
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");

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

            // Running filters
            // Normally we don't persist the result of filters though.
            foreach (var filter in _filters)
            {
                person.Biography = filter.FilterBiography(person.Biography);
            }
        }
    }


    public interface IPersonFilter : IDependency
    {
        string FilterBiography(string biography);
    }


    public class BadwordFilter : IPersonFilter
    {
        public string FilterBiography(string biography) => biography.Replace("damn", "cute");
    }


    public class ShortBiographyFilter : IPersonFilter
    {
        public string FilterBiography(string biography) =>
            biography.Length < 10 ? "This person has too short biography." : biography;
    }

    // NEXT STATION: Controllers/PersonController
}