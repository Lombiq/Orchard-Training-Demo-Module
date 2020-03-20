/*
 * Now we will develop a content field. Just like with content part development, we'll start by creating a model class.
 * We want to store a color name and the actual color in HEX so we need two strings for that. To test this please
 * attach this field for a content part or a content type (you can create new ones if you want) on the Dashboard.
 *
 * Conventionally we put these objects in the Fields folder.
 *
 * To learn more about content fields see:
 * https://docs.orchardcore.net/en/dev/docs/reference/modules/ContentFields/
 */

using OrchardCore.ContentManagement;

namespace Lombiq.TrainingDemo.Fields
{
    // You also need to register this class with the service provider (see: Startup.cs).
    public class ColorField : ContentField
    {
        // This property will store the HEX value of the color.
        public string Value { get; set; }

        // This is a name of the color that the user will provide. This is only for demonstration purposes, it will be
        // indexed later.
        public string ColorName { get; set; }
    }
}

// Don't forget to register your content field and the related view models in the Startup.cs static constructor so
// these will be accessible in the Liquid markup.

// NEXT STATION: Drivers/ColorFieldDisplayDriver