using GraphQL.Types;
using Lombiq.TrainingDemo.Models;

namespace Lombiq.TrainingDemo.GraphQL.Services;

// We add a model for the content part to the GraphQL schema. Content Types are added by Orchard Core automatically.
public class PersonPartObjectGraphType : ObjectGraphType<PersonPart>
{
    // These fields have counterparts in the index so we should include the same text in the
    // PersonPartWhereInputObjectGraphType without duplication.
    internal const string BirthDateDescription = "The person's date of birth, if any.";

    internal const string HandednessDescription = "The person's dominant hand.";

    public PersonPartObjectGraphType()
    {
        // Usually it's fine to permit null values. However, they aren't enabled for these simple fields by default so
        // make sure to set the nullable parameter to true.
        Field(part => part.Name, nullable: true).Description("The person's name.");
        Field(part => part.BirthDateUtc, nullable: true).Description(BirthDateDescription);

        // You can return computed types as well, when your property is not a primitive type. In practice this one isn't
        // necessary because Orchard Core automatically creates fields on the content item for your content fields too.
        // But it's a good example of how you can drill down in a complex type to expose its members directly.

        // Note the nameof: GraphQL.Net automatically turns field and argument to camelCase in the schema. But if you
        // need to compare or look up names, use the name.toCamelCase() extension method to avoid hard to diagnose bugs.
        Field<StringGraphType>(nameof(PersonPart.Biography))
            .Description("This is the person's life story.")
            .Resolve(context => context.Source.Biography.Text);

        // You have to add a separate GraphQL type to enumerations so they are converted to GraphQL enum types. This way
        // they are returned as UPPERCASE text instead of their integer representation. Before moving on, check out the
        // GraphQL/Models/HandednessEnumerationGraphType.cs file to see how this is done.

        // Note: This type is explicitly referenced whenever it is used, you don't have to add it to your dependency
        // injection service collection.
        Field<HandednessEnumerationGraphType>(nameof(PersonPart.Handedness))
            .Description(HandednessDescription)
            .Resolve(context => context.Source.Handedness);
    }
}

// Warning: There seems to be a bug when your query includes the built-in "biography" field before the "person" field.
// If you do it the other way around it works though. You can track the issue here:
// https://github.com/OrchardCMS/OrchardCore/issues/10181

// NEXT STATION: Services/PersonPartWhereInputObjectGraphType.cs
