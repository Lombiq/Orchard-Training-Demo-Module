using OrchardHUN.TrainingDemo.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrchardHUN.TrainingDemo.Services
{
    public interface IPersonManager
    {
        Task<PersonPart> GetPersonById(string contentItemId);

        Task<IEnumerable<PersonPart>> GetPersonsByName(string personName);

        void SavePerson(string name, Sex sex, string professionalProfileUrl, string professionalProfileText, DateTime birthDateUtc, string biography);

        void SaveAddress(string city, string zipCode, string address);

        Task AddAddressToPerson(string addressContentItemId, string personContentItemId, AddressType addressType);
    }
}
