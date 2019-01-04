using Lombiq.TrainingDemo.Indexes;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.Data.Migration;

namespace Lombiq.TrainingDemo.Migrations
{
    public class BookMigrations : DataMigration
    {
        IContentDefinitionManager _contentDefinitionManager;


        public BookMigrations(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }


        public int Create()
        {
            SchemaBuilder.CreateMapIndexTable(nameof(BookIndex), table => table
                .Column<string>(nameof(BookIndex.Author))
                .Column<string>(nameof(BookIndex.Title))
            );

            return 1;
        }
    }
}