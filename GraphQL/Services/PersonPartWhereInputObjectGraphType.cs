using GraphQL.Types;
using Lombiq.TrainingDemo.Indexes;
using Lombiq.TrainingDemo.Models;
using OrchardCore.Apis.GraphQL.Queries;
using static Lombiq.TrainingDemo.GraphQL.Services.PersonPartObjectGraphType;

namespace Lombiq.TrainingDemo.GraphQL.Services
{
    public class PersonPartWhereInputObjectGraphType : WhereInputObjectGraphType<PersonPart>
    {
        // The "where" filter gets automatically generated YesSql query based on your part index. You can add other
        // filters too but that's more complicated and less effective. See PersonAgeGraphQLFilter for that.
        public PersonPartWhereInputObjectGraphType()
        {
            // Since filters depend on the index, we use their "nameof" as reference.
            AddScalarFilterFields<DateTimeGraphType>(nameof(PersonPartIndex.BirthDateUtc), BirthDateDescription);
            AddScalarFilterFields<HandednessEnumerationGraphType>(nameof(PersonPartIndex.Handedness), HandednessDescription);
        }
    }
}
