/*
 * Previously we've seen how to describe an index. We also have to declare how to store it as well. This is where we
 * need migrations. Migrations are automatically run by the framework (after you've registered it in the service
 * provider). You can use them to describe DB schema changes.
 */

using Lombiq.TrainingDemo.Indexes;
using OrchardCore.Data.Migration;

namespace Lombiq.TrainingDemo.Migrations
{
    // Don't forget to register this class with the service provider (see: Startup.cs).
    public class BookMigrations : DataMigration
    {
        // Migrations have Create() and UpdateFromX methods. When the module is first enabled the Create() is called so
        // it can set up DB tables.
        public int Create()
        {
            SchemaBuilder.CreateMapIndexTable(nameof(BookIndex), table => table
                .Column<string>(nameof(BookIndex.Author))
                // Titles of books can be really long sometimes (even as long as 26000 characters:
                // https://www.guinnessworldrecords.com/world-records/358711-longest-title-of-a-book) so we have to make
                // sure it'll fit into the column. By default, text columns have a limit of 255 characters so we have to
                // enforce that and make it unlimited in cases the space is indeed needed.
                .Column<string>(nameof(BookIndex.Title), column => column.Unlimited())
            );

            // Let's suppose that we'll store many books, tens of thousands in the database. In this case, it's also
            // advised to add SQL indices to columns that are frequently queried on. In our case, Author is like this so
            // we add an index below. Note that you can only add indices in AlterTable().
            SchemaBuilder.AlterTable(nameof(BookIndex), table => table
                .CreateIndex($"IDX_{nameof(BookIndex)}_{nameof(BookIndex.Author)}", nameof(BookIndex.Author))
            );

            // Here we return the version number of the migration. If there were no update methods we'd return 1. But we
            // have one, see it for more details.
            return 2;
        }

        // This is an update method. It is used to modify an existing schema. Update methods will be run when the module
        // was already enabled before and the create method was run (like when you update a module already running on an
        // Orchard site). The X in UpdateFromX is the version number of the update (the method's name is conventional).
        // It means: "run this update if the module's current migration version is X". This method will run if it's 1.
        public int UpdateFrom1()
        {
            // The initial version of our module did not store the book's title. We quickly fix the issue by pushing out
            // an update that modifies the schema to add the Name. Remember, we've returned 2 in the Create method so
            // this update method won't be executed in a fresh setup. This is why you need to include all these changes
            // in the Create method as well.
            SchemaBuilder.AlterTable(nameof(BookIndex), table => table
                .AddColumn<string>(nameof(BookIndex.Title))
            );

            return 2;
        }
    }
}

// NEXT STATION: Controllers/DatabaseStorageController and go to the CreateBooksPost action where we previously left.
