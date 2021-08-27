using GraphQL.Types;
using OrchardCore.ContentManagement.GraphQL.Queries.Types;
using OrchardCore.ContentManagement.Metadata.Models;
using System.Linq;

namespace Lombiq.TrainingDemo.GraphQL.Services
{
    public class ContentItemTypeBuilder : IContentTypeBuilder
    {
        public const string AgeFilterName = "age";

        // Here you can add arguments to every Content Type (top level) field.
        public void Build(
            FieldType contentQuery,
            ContentTypeDefinition contentTypeDefinition,
            ContentItemType contentItemType)
        {
            // You can check the sub-fields if you want to only add a filter that matches a field in one of them.
            if (contentItemType.Fields.All(field => field.Name != "person")) return;

            contentQuery.Arguments.Add(new QueryArgument<IntGraphType>
            {
                Name = AgeFilterName,
                ResolvedType = new IntGraphType(),
            });

            // You shouldn't use special characters in the argument names so by GraphQL convention these two letter
            // suffixes are used instead.
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
