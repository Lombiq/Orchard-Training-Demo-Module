using OrchardCore.ContentManagement;

namespace Lombiq.TrainingDemo.Fields
{
    /*
     * Now we will develop a content field. Just like with content part development, we'll start with creating a model
     * class. We want to store a color name and the actual color in HEX so we need two strings for that.
     *
     * Conventionally we put these objects in the Fields folder.
    */
    public class ColorField : ContentField
    {
        public string Value { get; set; }
        public string ColorName { get; set; }
    }
}

// NEXT STATION: Drivers/ColorFieldDisplayDriver