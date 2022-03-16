using Lombiq.TrainingDemo.Indexes;
using Lombiq.TrainingDemo.Models;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using System;
using YesSql.Sql;

namespace Lombiq.TrainingDemo.Migrations;

// Here's another migrations file but specifically for Person-related operations, including how to define content
// types and configure content parts. Don't forget to register this class with the service provider (see
// Startup.cs). You can also generate such migration steps with the Code Generation feature of our Helpful
// Extensions module, check it out here: https://github.com/Lombiq/Helpful-Extensions
public class PersonMigrations : DataMigration
{
    private readonly IContentDefinitionManager _contentDefinitionManager;

    public PersonMigrations(IContentDefinitionManager contentDefinitionManager) => _contentDefinitionManager = contentDefinitionManager;

    public int Create()
    {
        // Now you can configure PersonPart. For example you can add content fields (as mentioned earlier) here.
        _contentDefinitionManager.AlterPartDefinition(nameof(PersonPart), part => part
            // Each field has its own configuration. Here you will give a display name for it and add some
            // additional settings like a hint to be displayed in the editor.
            .WithField(nameof(PersonPart.Biography), field => field
                .OfType(nameof(TextField))
                .WithDisplayName("Biography")
                .WithEditor("TextArea")
                .WithSettings(new TextFieldSettings
                {
                    Hint = "Person's biography",
                }))
        );

        // This one will create an index table for the PersonPartIndex as explained in the BookMigrations file.
        SchemaBuilder.CreateMapIndexTable<PersonPartIndex>(table => table
            .Column<DateTime>(nameof(PersonPartIndex.BirthDateUtc))
            .Column<Handedness>(nameof(PersonPartIndex.Handedness))
            // The content item ID is always 26 characters.
            .Column<string>(nameof(PersonPartIndex.ContentItemId), column => column.WithLength(26))
        );

        // We create a new content type. Note that there's only an alter method: this will create the type if it
        // doesn't exist or modify it if it does. Make sure you understand what content types are:
        // https://docs.orchardcore.net/en/latest/docs/glossary/#content-type. The content type's name is arbitrary but
        // choose a meaningful one. Notice how we use a class with constants to store the type name so we prevent
        // risky copy-pasting.
        _contentDefinitionManager.AlterTypeDefinition(ContentTypes.PersonPage, type => type
            .Creatable()
            .Listable()
            // We attach parts by specifying their name. For our own parts we use nameof(): This is not mandatory
            // but serves great if we change the part's name during development.
            .WithPart(nameof(PersonPart))
        );

        // We can even create a widget with the same content part. Widgets are just content types as usual but with
        // the Stereotype set as "Widget". You'll notice that our site's configuration includes three zones on the
        // frontend that you can add widgets to, as well as two layers. Check them out on the admin!
        _contentDefinitionManager.AlterTypeDefinition("PersonWidget", type => type
            .Stereotype("Widget")
            .WithPart(nameof(PersonPart))
        );

        return 1;
    }
}

// NEXT STATION: Indexes/PersonPartIndex
