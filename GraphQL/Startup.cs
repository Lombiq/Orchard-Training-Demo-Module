/* Orchard Core's GraphQL module gives you a lot of features out-of-the-box. Content types, fields and some built-in
 * parts are automatically included. So all you have to worry about is adding your custom content parts and filter
 * arguments to the GraphQL "schema".
 * Warning: GraphQL calls its properties "fields". To minimize confusion with Orchard Core's fields, we refer to the
 * latter exclusively as "content fields" throughout this training section.
 */

using Lombiq.TrainingDemo.GraphQL.Services;
using Lombiq.TrainingDemo.Models;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Apis;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.GraphQL.Queries;
using OrchardCore.ContentManagement.GraphQL.Queries.Types;
using OrchardCore.Modules;

namespace Lombiq.TrainingDemo.GraphQL;

// By convention the GraphQL specific services should be placed inside the GraphQL directory of their module. Don't
// forget the RequireFeatures attribute: the schema is built at startup in the singleton scope, so it would be wasteful
// to let it run when GraphQL is disabled. When the GraphQL feature is enabled you can go to Configuration > GraphiQL to
// inspect and play around with the queries without needing an external query editor.
[RequireFeatures("OrchardCore.Apis.GraphQL")]
public class Startup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        // The first 3 lines here add the "person" field to any ContentItem type field that has a PersonPart. Implement
        // ObjectGraphType<TPart> to display a content part-specific field. This is required.
        services.AddObjectGraphType<PersonPart, PersonPartObjectGraphType>();
        // Optionally, if you have a content part index, implement WhereInputObjectGraphType<TPart> and
        // PartIndexAliasProvider<TPartIndex>. These will give you database-side filtering via the "where" argument.
        services.AddInputObjectGraphType<PersonPart, PersonPartWhereInputObjectGraphType>();
        services.AddTransient<IIndexAliasProvider, PersonPartIndexAliasProvider>();

        // Sometimes you want more advanced filter logic or filtering that can't be expressed with YesSql queries. In
        // this case you can add custom filter attributes by implementing IContentTypeBuilder and then evaluate their
        // values in a class that implements IGraphQLFilter<ContentItem>.
        services.AddScoped<IContentTypeBuilder, ContentItemTypeBuilder>();
        services.AddTransient<IGraphQLFilter<ContentItem>, PersonAgeGraphQLFilter>();
    }
}

// NEXT STATION: Services/PersonPartObjectGraphType.cs
