using Orchard.ContentManagement;
using Orchard.Environment.Extensions;

/* Just like with content part development, we'll start with creating a model class.
 * We don't need anything too complex, just store a string for a YouTube video ID, which will be used to identify YouTube 
 * videos for embedding.
 * 
 * If we have many fields and other models we normally place field models into the Fields namespace.
 */
namespace OrchardHUN.TrainingDemo.Models
{
    [OrchardFeature("OrchardHUN.TrainingDemo.Contents")]
    public class YouTubeEmbedField : ContentField // Note that the class inherits from ContentField
    {
        /* 
         * Parts can decide how the data they need is stored (if they are backed by some data at all) and most of the
         * time they're stored as records in the database. Fields however are always stored as serialized XMLs together 
         * with the item's record. 
         * 
         * This means that if the data stored should be queried in various ways you should use parts (since they can be 
         * queried freely
         * with LINQ and indexed in the DB); but if there can be an unknown number of pieces attached to a content type 
         * then fields are the way to go. You have to keep in mind though that due to the serialization process, fields 
         * can potentially perform worse if there are many items to be fetched. On the other hand parts, since they're 
         * stored in separate tables and thus are joined in or even sometimes loaded lazily one by one may also have 
         * negative impact on performance (since the content record, where fields are stored, is always loaded for a 
         * content item there's no subsequent DB call for fetching fields).
         * 
         * All in all there's no real rule of thumb: deciding how to model a piece of functionality should be decided on 
         * a case by case basis, weighting the drawbacks and advantages. When in doubt, use a part.
         * 
         * The call to Storage we see here is the usage of the underlying serialized storage.
         */
        public string VideoId
        {
            // Get and Set have overrides without requiring a name. If you don't specify a name here the field's default
            // (single) value storage will be used.
            get => Storage.Get<string>(nameof(VideoId));
            set => Storage.Set(nameof(VideoId), value ?? string.Empty);
        }
    }
}

// NEXT STATION: ContentsMigrations.cs -> UpdateFrom1()