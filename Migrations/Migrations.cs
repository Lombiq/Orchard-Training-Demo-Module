using Orchard.ContentManagement.MetaData;
using Orchard.Data.Migration;

namespace OrchardHUN.TrainingDemo
{
    public class Migrations : DataMigration
    {
        IContentDefinitionManager _contentDefinitionManager;

        public Migrations(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }

        public int Create()
        {
            //SchemaBuilder.CreateTable()
            _contentDefinitionManager.AlterTypeDefinition("Foo", builder => builder
                .WithPart("TestContentPartA")
                .WithPart("TestContentPartB")
            );

            return 1;
        }
    }
}