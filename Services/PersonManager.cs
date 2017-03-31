using Orchard.ContentManagement;
using OrchardHUN.TrainingDemo.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YesSql.Core.Query;
using YesSql.Core.Services;

namespace OrchardHUN.TrainingDemo.Services
{
    public class PersonManager : IPersonManager
    {
        private readonly IContentManager _contentManager;
        private readonly ISession _session;


        public PersonManager(IContentManager contentManager, ISession session)
        {
            _contentManager = contentManager;
            _session = session;
        }


        public async Task<PersonPart> GetPersonById(string contentItemId)
        {
            var persons = GetPersons();

            var result = await persons
                .Where(person => person.ContentItemId == contentItemId)
                .FirstOrDefault();

            return result;
            //var personContentItem = await _contentManager.GetAsync(contentItemId);
            //return await _contentManager.GetAsync(contentItemId);
        }

        public async Task<IEnumerable<PersonPart>> GetPersonsByName(string personName)
        {
            var persons = GetPersons();

            var results = await persons
                .Where(person => person.Name.Contains(personName))
                .List();

            return results;
        }

        public void SavePerson(string name, Sex sex, string professionalProfileUrl, string professionalProfileText, DateTime birthDateUtc, string biography)
        {
            var personContentItem = _contentManager.New(nameof(Constants.ContentTypeNames.Person));

            // Dynamic syntax
            //personContentItem.Content.PersonPart.Name.Text = name;
            personContentItem.Content.PersonPart.Sex = sex;
            //personContentItem.Content.PersonPart.ProfessionalProfile.Url = professionalProfileUrl;
            //personContentItem.Content.PersonPart.ProfessionalProfile.Text = professionalProfileText;
            personContentItem.Content.PersonPart.BirthDateUtc = birthDateUtc;
            //personContentItem.Content.PersonPart.Biography.Text = biography;

            var personPart = personContentItem.As<PersonPart>();

            personPart.Name = new Orchard.ContentFields.Fields.TextField
            {
                Text = name
            };

            personPart.ProfessionalProfile = new Orchard.ContentFields.Fields.LinkField
            {
                Text = professionalProfileText,
                Url = professionalProfileUrl
            };
            personPart.Biography = new Orchard.ContentFields.Fields.TextField
            {
                Text = biography
            };

            personContentItem.Apply(personPart);

            _contentManager.Create(personContentItem);
        }

        public void SaveAddress(string city, string zipCode, string address)
        {
            var addressContentItem = _contentManager.New(nameof(Constants.ContentTypeNames.Address));

            // Explicit syntax
            var addressPart = addressContentItem.As<AddressPart>();
            addressPart.City = city;
            addressPart.ZipCode = zipCode;
            addressPart.Address = address;
           
            addressContentItem.Apply(addressPart);

            _contentManager.Create(addressContentItem);
        }

        public async Task AddAddressToPerson(string addressContentItemId, string personContentItemId, AddressType addressType)
        {
            var personContentItem = await _contentManager.GetAsync(personContentItemId);
            var addressContentItem = await _contentManager.GetAsync(addressContentItemId);

            var addressPart = addressContentItem.As<AddressPart>();


            if (personContentItem != null && addressContentItem != null)
            {
                personContentItem.Weld(addressType.ToString(), addressPart);

                _contentManager.Create(personContentItem, VersionOptions.Latest);
            }
        }


        private IQuery<PersonPart, PersonPartIndex> GetPersons()
        {
            return _session.QueryAsync<PersonPart, PersonPartIndex>();
        }
    }
}
