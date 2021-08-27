using GraphQL.Types;
using Lombiq.TrainingDemo.Models;

namespace Lombiq.TrainingDemo.GraphQL.Services
{
    // We add a model for the content part to the GraphQL Schema. Content Types are added by Orchard Core automatically.
    public class PersonPartObjectGraphType : ObjectGraphType<PersonPart>
    {
        // These fields have counterparts in the index so we should include the same text in the
        // PersonPartWhereInputObjectGraphType without duplication.
        internal static string BirthDateDescription = "The person's date of birth, if any.";
        internal static string HandednessDescription = "The person's dominant hand.";

        public PersonPartObjectGraphType()
        {
            // Usually we want to allow null values, but these primitive fields are by default so we have to permit it.
            Field(part => part.Name, nullable: true).Description("The person's name.");
            Field(part => part.BirthDateUtc, nullable: true).Description(BirthDateDescription);

            // You can return computed types as well, when your property is not a primitive type. In practice this one
            // isn't necessary because Orchard Core automatically creates fields on the content item for your content
            // fields.
            Field<StringGraphType>(
                nameof(PersonPart.Biography), // GraphQL.Net automatically turns this to camelCase.
                description: "This is the person's life story.",
                resolve: context => context.Source.Biography.Text);

            // You have to add a separate GraphQL type to enumerations so they are converted to GraphQL enum types.
            Field<HandednessEnumerationGraphType>(
                nameof(PersonPart.Handedness),
                HandednessDescription,
                resolve: context => context.Source.Handedness);
        }
    }
}
