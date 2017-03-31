using Orchard.ContentFields.Fields;
using Orchard.ContentManagement.Metadata.Settings;
using Orchard.ContentManagement.MetaData;
using Orchard.Data.Migration;
using OrchardHUN.TrainingDemo.Constants;
using OrchardHUN.TrainingDemo.Models;

namespace OrchardHUN.TrainingDemo.Migrations
{
    public class PersonMigration : DataMigration
    {
        IContentDefinitionManager _contentDefinitionManager;


        public PersonMigration(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }


        public int Create()
        {
            _contentDefinitionManager.AlterPartDefinition(nameof(PersonPart), builder => builder
                .Attachable()
                .WithField(nameof(PersonPart.Name), cfg => cfg
                    .OfType(nameof(TextField))
                    .WithDisplayName("The name of the person."))
                .WithField(nameof(PersonPart.Sex), cfg => cfg
                    .OfType(nameof(TextField))
                    .WithDisplayName("The sex of the person."))
                .WithField(nameof(PersonPart.ProfessionalProfile), cfg => cfg
                    .OfType(nameof(LinkField))
                    .WithDisplayName("The professional profile URL of the person.")
                    .WithSetting("LinkFieldSettings.DefaultUrl", "https://lombiq.com")
                    .WithSetting("LinkFieldSettings.DefaultText", "Lombiq.com"))
                .WithField(nameof(PersonPart.Biography), cfg => cfg
                    .OfType(nameof(TextField))
                    .WithDisplayName("The short biography of the person."))
            );

            // Creating a Person content type without the reusable AddressParts.
            _contentDefinitionManager.AlterTypeDefinition(nameof(ContentTypeNames.Person), builder => builder
                .Creatable()
                .WithPart(nameof(PersonPart))
            );

            // Creating the PersonWithAddresses content type with the reusable AddressParts.
            _contentDefinitionManager.AlterTypeDefinition(nameof(ContentTypeNames.PersonWithAddresses), builder => builder
                .Creatable()
                .WithPart(nameof(PersonPart))
                .WithPart("HomeAddress", nameof(AddressPart), cfg => cfg
                    .WithDisplayName("The home address of the person."))
                .WithPart("WorkAddress", nameof(AddressPart), cfg => cfg
                    .WithDisplayName("The work address of the person."))
            );

            SchemaBuilder.CreateMapIndexTable(nameof(PersonPartIndex), table => table
                .Column<string>(nameof(PersonPartIndex.Name), col => col.WithLength(1024))
            );

            return 1;
        }
    }
}