using GraphQL.Types;
using Lombiq.TrainingDemo.Models;

namespace Lombiq.TrainingDemo.GraphQL.Services
{
    // We add a model for the content part to the GraphQL Schema. Content Types are added by Orchard Core automatically.
    public class PersonPartObjectGraphType : ObjectGraphType<PersonPart>
    {
        internal static string BirthDateDescription = "The person's date of birth, if any.";

        public PersonPartObjectGraphType()
        {
            // Usually we want to allow null values, but these primitive fields are by default so we have to permit it.
            Field(part => part.Name, nullable: true).Description("The person's name.");
            Field(part => part.BirthDateUtc, nullable: true).Description(BirthDateDescription);

            // You can return computed types as well, when your property is not a primitive type.
            Field<StringGraphType>(
                nameof(PersonPart.Biography), // GraphQL.Net automatically turns this to camelCase.
                description: "This is the person's life story.",
                resolve: context => context.Source.Biography.Text);

            // You have to add a separate GraphQL type to enumerations so they are converted to GraphQL enum types.
            Field<HandednessEnumerationGraphType>(
                nameof(PersonPart.Handedness),
                "The person's dominant hand.",
                resolve: context => context.Source.Handedness);
        }
    }
}
