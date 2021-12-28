using GraphQL.Types;
using OrchardCore.ContentManagement.GraphQL.Queries.Types;
using OrchardCore.ContentManagement.Metadata.Models;
using System.Linq;

namespace Lombiq.TrainingDemo.GraphQL.Services
{
    // Services that implement IContentTypeBuilder extend the features of existing ContentItem type fields, including
    // the top level fields automatically created by Orchard Core for every content type. You can use this to add new
    // sub-fields or filter attributes to existing ContentItem type fields.
    public class ContentItemTypeBuilder : IContentTypeBuilder
    {
        // It's a good practice to make the argument name a const because you will reuse it in the IGraphQLFilter.
        public const string AgeFilterName = "age";

        // Here you can add arguments to every Content Type (top level) field.
        public void Build(
            FieldType contentQuery,
            ContentTypeDefinition contentTypeDefinition,
            ContentItemType contentItemType)
        {
            // You can check to see if the field has any specific sub-field, if you want to rely on its features. For
            // example if you only want to apply to ContentItem fields that have the "person" sub-field (i.e. those that
            // have a PersonPart). This is useful if you want to expand your content part field in another module.
            if (contentItemType.Fields.All(field => field.Name != "person")) return;

            // The resolved type can be anything that can be represented with JSON and has a known graph type, but we
            // stick with numbers for simplicity's sake. This one filters for equation.
            contentQuery.Arguments.Add(new QueryArgument<IntGraphType>
            {
                Name = AgeFilterName,
                ResolvedType = new IntGraphType(),
            });

            // You can't use special characters in the argument names so by GraphQL convention these two letter suffixes
            // that represent the relational operators. Except equation which customarily gets no suffix.
            AddFilter(contentQuery, "_lt");
            AddFilter(contentQuery, "_le");

            AddFilter(contentQuery, "_ge");
            AddFilter(contentQuery, "_gt");

            AddFilter(contentQuery, "_ne");
        }

        private static void AddFilter(FieldType contentQuery, string suffix) =>
            contentQuery.Arguments.Add(new QueryArgument<IntGraphType>
            {
                Name = AgeFilterName + suffix,
                ResolvedType = new IntGraphType(),
            });
    }
}

// NEXT STATION: Services/PersonAgeGraphQLFilter.cs
