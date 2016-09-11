using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;
using OrchardHUN.TrainingDemo.Models;

namespace OrchardHUN.TrainingDemo
{
    [OrchardFeature("OrchardHUN.TrainingDemo.Contents")]
    public class ContentsMigrations : DataMigrationImpl
    {
        public int Create()
        {
            SchemaBuilder.CreateTable(typeof(PersonListPartRecord).Name, 
                table => table
                    // Since PersonListPartRecord is a ContentPartRecord we have to use this method here. For
                    // ContentPartVersionRecord we would simply use ContentPartVersionRecord()
                    .ContentPartRecord()
                    .Column<string>("Sex")
                    .Column<int>("MaxCount")
                );

            /* 
             * We make PersonListPart attachable. This means from the admin UI you'll be able to attach this part to 
             * any conent type. This step  is not necessary to attach the part to types from migrations like we do it
             * from here.
             * Default is not attachable.
             */
            ContentDefinitionManager.AlterPartDefinition(typeof(PersonListPart).Name,
                builder => builder.Attachable());

            /*
             * We create a new content type. Note that there's only an alter method: this will create the type if it 
             * doesn't exist or modify it if it does. Make sure you understand what content types are:
             *  http://docs.orchardproject.net/Documentation/Content-types
             * The content type's name is arbitrary, but choose a meaningful one.
             * Notice that we attach parts by specifying their name. For our own parts we use typeof().Name: this is 
             * not mandatory but serves great if we change the part's name during development. (The same goes for record 
             * name BTW.)
             */
            ContentDefinitionManager.AlterTypeDefinition("PersonList", 
                cfg => cfg
                    // Setting display name for the type. BTW the default is the technical name separated on capital
                    // letters, so the same here.
                    .DisplayedAs("Person List")
                    .WithPart("TitlePart") // So the list can have a title; TitlePart is a core part
                    // AutoroutePart so the list can have a friendly URL. That's why this feature depends on Orchard.Autoroute.
                    .WithPart("AutoroutePart", builder => builder
                        // These are TypePart settings: settings for a part on a specific type. I.e. AutoroutePart have
                        // the following settings for PersonList. Take a look at AutoroutePart settings on the type
                        // editor UI of PersonList to see what these mean.
                        .WithSetting("AutorouteSettings.AllowCustomPattern", "true")
                        .WithSetting("AutorouteSettings.AutomaticAdjustmentOnEdit", "false")
                        // Specifying a custom URL-pattern for PersonList items
                        .WithSetting("AutorouteSettings.PatternDefinitions", "[{Name:'Title', Pattern: 'person-lists/{Content.Slug}', Description: 'my-list'}]")
                        .WithSetting("AutorouteSettings.DefaultPatternIndex", "0"))
                    .WithPart(typeof(PersonListPart).Name)
                    // CommonPart includes e.g. creation date and owner. Take a look at it (search with Ctrl+comma). Also
                    // without it we can't list content types of this type on the admin UI (because the dates stored in
                    // it are needed for ordering).
                    .WithPart("CommonPart")
                    // This means users will be able to create such items from the admin UI. Default is the opposite.
                    .Creatable()
                );

            /*
             * With the same part we also create a widget. That's why this feature also depends on Orchard.Widgets!
             * Note that widgets should
             * - Have CommonPart attached
             * - Have WidgetPart attached
             * - Have the stereotype "Widget"
             */
            ContentDefinitionManager.AlterTypeDefinition("PersonListWidget",
                cfg => cfg
                    .WithPart(typeof(PersonListPart).Name)
                    .WithPart("WidgetPart")
                    .WithPart("CommonPart")
                    .WithSetting("Stereotype", "Widget")
                );


            return 1;

            // Please don't read UpdateFrom1() yet.

            // You read it, didn't you? Stop spoiling.

            // NEXT STATION: Handlers/PersonListPartHandler
        }



        public int UpdateFrom1()
        {
            // We're attaching the YouTubeEmbedField to the PersonListPart, which is already attached to the PersonList
            // type.
            ContentDefinitionManager.AlterPartDefinition(typeof(PersonListPart).Name,
                builder => builder
                    // This name distinguishes between fields if there are multiple ones of the same type on the part
                    .WithField("YouTubeVideoEmbed",
                        f => f
                            .WithDisplayName("YouTube Video Embed") // This will be displayed as the name
                            .OfType(typeof(YouTubeEmbedField).Name)));

            // We've attached the field here to an existing part, but we could have created a new part (just giving an
            // arbitrary name as the argument for AlterPartDefinition() would be enough) too.
            // Note that fields are always attached to parts. If you attach fields seemingly directly to a content type
            // fromt the admin UI in the background an invisible part is created, having the same name as the type.


            return 2;

            // NEXT STATION: Drivers/YouTubeEmbedFieldDriver.cs
        }
    }
}