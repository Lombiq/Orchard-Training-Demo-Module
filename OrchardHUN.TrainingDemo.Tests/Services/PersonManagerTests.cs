using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using NUnit.Framework;
using Orchard.Tests.Modules;
using OrchardHUN.TrainingDemo.Models;
using Orchard.Tests.Utility;
using Moq;
using OrchardHUN.TrainingDemo.Services;
using Orchard.Data;
using Orchard.Services;

namespace OrchardHUN.TrainingDemo.Tests.Services
{
    // With this attribute we tell the framework that this class is a test suite.
    [TestFixture]
    // The PersonManager service uses the DB through IRepository (check back if you've forgotten!). DatabaseEnabledTestsBase has some setup for 
    // such tests so it's convenient to use it.
    public class PersonManagerTests : DatabaseEnabledTestsBase
    {
        private IPersonManager _personManager;

        // Building a testing dependency injection container with mocked and stubbed types. See: http://code.google.com/p/moq/
        public override void Register(ContainerBuilder builder)
        {
            // If any unregistered type is requested from the container a dynamic mock will be created for it.
            builder.RegisterAutoMocking(MockBehavior.Loose);

            // Despite that Repository is registered in DatabaseEnabledTestsBase it should be re-registered somehow!
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>));
            // Registering a collection. If we would just register an instance the injected IEnumerable<IPersonFilter> would have two items,
            // one mocked, what messes up biography...
            builder.RegisterInstance(new IPersonFilter[] { new StubPersonFilter() }).As<IEnumerable<IPersonFilter>>();

            builder.RegisterType<PersonManager>().As<IPersonManager>();
        }

        // Test initialisation
        // This method runs before every test method.
        public override void Init()
        {
            base.Init();

            _personManager = _container.Resolve<IPersonManager>();
        }

        // This property should return the types of the records used in the tested class
        protected override IEnumerable<Type> DatabaseTypes
        {
            get
            {
                return new[]
                {
                    typeof(PersonRecord)
                };
            }
        }


        // The Test attribute tells that this method is a test, surprisingly :-).
        [Test]
        public void SaveShouldBePersistent()
        {
            _personManager.SavePerson("Jacob Gips", Sex.Male, new DateTime(1977, 3, 14), "I was born on a damn farm in South-North Neverland.");
            _personManager.SavePerson("Naomi Concrete", Sex.Female, new DateTime(1979, 5, 12), "\"Always cite meaningful quotes.\" - Luke Skywalker");

            var persons = _personManager.GetPersons().ToList();

            Assert.That(persons.Count, Is.EqualTo(2));

            var jacobGips = persons[0];
            Assert.That(jacobGips.Name, Is.EqualTo("Jacob Gips"));
            Assert.That(jacobGips.Sex, Is.EqualTo(Sex.Male));
            Assert.That(jacobGips.BirthDateUtc.Value.ToFileTimeUtc(), Is.EqualTo(118716192000000000));
            Assert.That(jacobGips.Biography, Is.EqualTo("I was born on a damn farm in South-North Neverland."));

            var naomiConcrete = persons[1];
            Assert.That(naomiConcrete.Name, Is.EqualTo("Naomi Concrete"));
            Assert.That(naomiConcrete.Sex, Is.EqualTo(Sex.Female));
            Assert.That(naomiConcrete.BirthDateUtc.Value.ToFileTimeUtc(), Is.EqualTo(119397888000000000));
            Assert.That(naomiConcrete.Biography, Is.EqualTo("\"Always cite meaningful quotes.\" - Luke Skywalker"));

            // This clears the test "DB" so the tests don't affect each other.
            ClearSession();
        }

        [Test]
        public void PersonsAreFiltered()
        {
            _personManager.SavePerson("Jacob Gips", Sex.Male, new DateTime(1977, 3, 14), "I was born on a damn farm in South-North Neverland.");
            _personManager.SavePerson("Naomi Concrete", Sex.Female, new DateTime(1979, 5, 12), "\"Always cite meaningful quotes.\" - Luke Skywalker");
            _personManager.SavePerson("James Ytong", Sex.Male, new DateTime(1989, 12, 4), "<insert subject biography here>");
            _personManager.SavePerson("Maria Brick", Sex.Female, new DateTime(1969, 10, 6), "Not much.");

            var persons = _personManager.GetPersons(Sex.Female, 1).ToList();

            Assert.That(persons.Count, Is.EqualTo(1));
            Assert.That(persons[0].Sex, Is.EqualTo(Sex.Female));

            ClearSession();
        }


        // Unit testing is - ideally - about testing atomic entities. This means that any dependency used by the tested type should be mocked or
        // stubbed, i.e. subsituted with types not affecting how the test type behaves (this means that these mocks and stubs don't really do 
        // anything). Orchard services all have stubs in the built-in test projects so you don't have the tedious task of creating them.
        private class StubPersonFilter : IPersonFilter
        {
            public string FilterBiography(string biography)
            {
                return biography;
            }
        }
    }
}
