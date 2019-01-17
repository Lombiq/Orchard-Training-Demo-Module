/*
 * Every content part and content field can have settings. If you have a PersonPart content part where you use
 * ColorField these settings will be applied on every content item where the PersonPart is present. However, if you
 * have multiple ColorFields, each fields will have their own settings. In our case we have three settings for the
 * field.
 */

namespace Lombiq.TrainingDemo.Settings
{
    public class ColorFieldSettings
    {
        // We'll use this setting to determine whether the value of the field is required to be given by the user.
        public bool Required { get; set; }

        // Hint text to be displayed on the editor.
        public string Hint { get; set; }

        // The label to be used on the editor and the display shape.
        public string Label { get; set; }
    }
}

// How can these settings be edited? If you attach a content field to a content part / content type from a migration
// you can set these values there (see: Migrations/PersonMigrations where the Biography field is being added to the
// PersonPart). If you attach the field on the dashboard yourself then Orchard Core will display its editor. Editor of
// an object like this? You know what you need to do... Create a DisplayDriver for this!

// NEXT STATION: Settings/ColorFieldSettingsDriver