using Lombiq.TrainingDemo.Models;
using YesSql.Indexes;

namespace Lombiq.TrainingDemo.Indexes
{
    // The BookIndex objects will be stored in the database. Since this is actually a relational database these will be
    // records in a table specifically created for this index.
    public class BookIndex : MapIndex
    {
        public string Author { get; set; }
        public string Title { get; set; }
    }


    // These IndexProvider services will provide the mappings between the objects stored in the document database and
    // the index objects stored in records. When a Book object is being saved by the ISession service an index record
    // will also be stored in the related index table. Don't forget to register this class with the service provider
    // (see: Startup.cs).
    // Note that this IndexProvider is registered as a singleton which is the good choice usually. However, if you want
    // to inject other services into it (like any of the basic Orchard services you've seen until now) then you need to
    // register it with AddScoped<IScopedIndexProvider, BookIndexProvider>() and make the class also implement
    // IScopedIndexProvider.
    public class BookIndexProvider : IndexProvider<Book>
    {
        public override void Describe(DescribeContext<Book> context) =>
            context.For<BookIndex>()
                .Map(book =>
                    new BookIndex
                    {
                        Author = book.Author,
                        Title = book.Title,
                    });
    }

    // NEXT STATION: Migrations/BookMigrations.
}
