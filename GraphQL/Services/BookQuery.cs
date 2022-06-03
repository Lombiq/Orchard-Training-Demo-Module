using System;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Resolvers;
using GraphQL.Types;
using Lombiq.TrainingDemo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Primitives;
using OrchardCore.Apis.GraphQL;
using OrchardCore.ContentManagement;
using OrchardCore.Documents;
using OrchardCore.Queries;

namespace Lombiq.TrainingDemo.GraphQL.Services;

public class BookQuery : ISchemaBuilder
{
    private readonly IHttpContextAccessor _hca;
    private readonly IStringLocalizer<BookQuery> T;

    public BookQuery(IHttpContextAccessor hca, IStringLocalizer<BookQuery> localizer)
    {
        _hca = hca;
        T = localizer;
    }

    public Task BuildAsync(ISchema schema)
    {
        var field = new FieldType
        {
            Name = "Book",
            Description = T["Content items are instances of content types, just like objects are instances of classes."],
            Type = typeof(BookObjectGraphType),
            Arguments = new QueryArguments(
                new QueryArgument<NonNullGraphType<StringGraphType>>
                {
                    Name = "contentItemId",
                    Description = S["Content item id"]
                }
            ),
            Resolver = new AsyncFieldResolver<ContentItem>(ResolveAsync)
        };

        schema.Query.AddField(field);

        return Task.CompletedTask;
    }

    public Task<string> GetIdentifierAsync() => Task.FromResult(string.Empty);
}
