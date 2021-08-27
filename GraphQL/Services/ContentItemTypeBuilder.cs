using GraphQL.Types;
using OrchardCore.ContentManagement.GraphQL.Queries.Types;
using OrchardCore.ContentManagement.Metadata.Models;
using System.Linq;

namespace Lombiq.TrainingDemo.GraphQL.Services
{
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
            // You can check the sub-fields if your argument relies on certain content parts or content fields.
            if (contentItemType.Fields.All(field => field.Name != "person")) return;

            // The resolved type can be anything that can be represented with JSON and has a known graph type, but we
            // tick with numbers for simplicity's sake.
            contentQuery.Arguments.Add(new QueryArgument<IntGraphType>
            {
                Name = AgeFilterName,
                ResolvedType = new IntGraphType(),
            });

            // You can't use special characters in the argument names so by GraphQL convention these two letter
            // suffixes that represent the relational operators.
            AddFilter(contentQuery, "_lt");
            AddFilter(contentQuery, "_le");

            AddFilter(contentQuery, "_ge");
            AddFilter(contentQuery, "_gt");

            AddFilter(contentQuery, "_eq");
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
