/*
 * Now we will develop a content field. Just like with content part development, we'll start by creating a model class.
 * We want to store a color name and the actual color in HEX so we need two strings for that. To test this please
 * attach this field for a content part or a content type (you can create new ones if you want) on the Dashboard.
 *
 * Conventionally we put these objects in the Fields folder.
 *
 * To learn more about content fields see:
 * https://orchardcore.readthedocs.io/en/latest/OrchardCore.Modules/OrchardCore.ContentFields/README/
 */

using OrchardCore.ContentManagement;

namespace Lombiq.TrainingDemo.Fields
{
    // You also need to register this class with the service provider (see: Startup.cs).
    public class ColorField : ContentField
    {
        public string Value { get; set; }
        public string ColorName { get; set; }
    }
}

// Don't forget to register your content field and the related view models in the Startup.cs static constructor so
// these will be accessible in the Liquid markup.

// NEXT STATION: Drivers/ColorFieldDisplayDriver