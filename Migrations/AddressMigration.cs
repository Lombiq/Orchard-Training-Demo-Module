using Orchard.ContentManagement.Metadata.Settings;
using Orchard.ContentManagement.MetaData;
using Orchard.Data.Migration;
using OrchardHUN.TrainingDemo.Models;

namespace OrchardHUN.TrainingDemo.Migrations
{
    public class AddressMigration : DataMigration
    {
        IContentDefinitionManager _contentDefinitionManager;


        public AddressMigration(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }


        public int Create()
        {
            _contentDefinitionManager.AlterPartDefinition(nameof(AddressPart), builder => builder
                .Reusable()
                .Attachable()
                .WithDescription("Provides city, ZIP Code and address fields to your content item.")
            );

            _contentDefinitionManager.AlterTypeDefinition(nameof(Constants.ContentTypeNames.Address), builder => builder
                .Creatable()
                .WithPart(nameof(AddressPart))
            );

            return 1;
        }
    }
}
