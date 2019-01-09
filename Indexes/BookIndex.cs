using Lombiq.TrainingDemo.Models;
using YesSql.Indexes;

namespace Lombiq.TrainingDemo.Indexes
{
    // The BookIndex objects will be stored in the database. Since this is actually a relational database these will be
    // actual records in a table specifically created for this index.
    public class BookIndex : MapIndex
    {
        public string Author { get; set; }
        public string Title { get; set; }
    }


    // These IndexProvider services will provide the mappings between the objects stored in the document database and
    // the index objects stored in records. When a Book object is being saved by the ISession service an index record
    // will also be stored in the related index table.
    // You need to register this service to the service provider.
    public class BookIndexProvider : IndexProvider<Book>
    {
        public override void Describe(DescribeContext<Book> context)
        {
            context.For<BookIndex>()
                .Map(book =>
                    new BookIndex
                    {
                        Author = book.Author,
                        Title = book.Title
                    });
        }
    }

    // NEXT STATION: Migrations/BookMigrations.
}