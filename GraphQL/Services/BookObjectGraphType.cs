using GraphQL.Types;
using Lombiq.TrainingDemo.Models;

namespace Lombiq.TrainingDemo.GraphQL.Services;

public class BookObjectGraphType : ObjectGraphType<Book>
{
    public BookObjectGraphType()
    {
        Field(x => x.Title);
        Field(x => x.Author);
        Field(x => x.CoverPhotoUrl);
        Field(x => x.Description);
    }
}
