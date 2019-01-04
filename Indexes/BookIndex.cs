using Lombiq.TrainingDemo.Models;
using YesSql.Indexes;

namespace Lombiq.TrainingDemo.Indexes
{
    public class BookIndex : MapIndex
    {
        public string Author { get; set; }
        public string Title { get; set; }
    }


    public class BookIndexProvider : IndexProvider<Book>
    {
        public override void Describe(DescribeContext<Book> context)
        {
            context.For<BookIndex>()
                .Map(book =>
                    new BookIndex
                    {
                        Author = book.Author,
                        Title = book.Title,
                    });
        }
    }
}