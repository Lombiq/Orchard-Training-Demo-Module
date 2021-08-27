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
        // filters too but that's more complicated and outside of the scope of this tutorial. You can learn more about
        // it on GraphQL.Net's documentation at: TODO
        public PersonPartWhereInputObjectGraphType() =>
            // Since filters depend on the index, we use their "nameof" as reference.
            AddScalarFilterFields<StringGraphType>(nameof(PersonPartIndex.BirthDateUtc), BirthDateDescription);
    }
}
