/*
 * Previously we've seen how to describe an index. We also have to declare how to store it as well. This is where we
 * need migrations. Migrations are automatically run by the framework (after you've registered it in the service
 * provider). You can use them to describe DB schema changes.
 */

using Lombiq.TrainingDemo.Indexes;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.Data.Migration;

namespace Lombiq.TrainingDemo.Migrations
{
    // Don't forget to register this class to the service provider (see: Startup.cs).
    public class BookMigrations : DataMigration
    {
        // Migrations have Create() and UpdateFromX methods. When the module is first enabled the Create() is called so
        // it can set up DB tables.
        public int Create()
        {
            SchemaBuilder.CreateMapIndexTable(nameof(BookIndex), table => table
                .Column<string>(nameof(BookIndex.Author))
                .Column<string>(nameof(BookIndex.Title))
            );

            // Here we return the version number of the migration. If there were no update methods we'd return 1. But
            // we have one, see it for more details.
            return 2;
        }

        // This is an update method. It is used to modify an existing schema. Update methods will be run when the
        // module was already enabled before and the create method was run (like when you update a module already
        // running on an Orchard site). The X in UpdateFromX is the version number of the update (the method's name is
        // conventional). It means: "run this update if the module's current migration version is X". This method will
        // run if it's 1.
        public int UpdateFrom1()
        {
            // The initial version of our module did not store the book's title. We quickly fix the issue by pushing
            // out an update that modifies the schema to add the Name. Remember, we've returned 2 in the Create method
            // so this update method won't be executed in a fresh setup. This is why you need to include all these
            // changes in the Create method as well.
            SchemaBuilder.CreateMapIndexTable(nameof(BookIndex), table => table
                .Column<string>(nameof(BookIndex.Title))
            );

            return 2;
        }
    }
}

// NEXT STATION: Controllers/StoreController and go to the CreateBooksPost action where we previously left.