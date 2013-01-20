using Orchard.ContentManagement;
using Orchard.ContentManagement.FieldStorage;
using Orchard.Environment.Extensions;
using System;

/* Just like with content part development, we'll start with creating a model class.
 * We don't need anything too complex, just store a string for a YouTube video ID,
 * which will be used to identify YouTube videos for embedding.
 */ 
namespace OrchardHUN.TrainingDemo.Models
{
    [OrchardFeature("OrchardHUN.TrainingDemo.Contents")]
    public class YouTubeEmbedField : ContentField
    {
        public string VideoID
        {
            get { return Storage.Get<string>(); }
            set { Storage.Set(value ?? String.Empty); }
        }
    }
}

// NEXT STATION: ContentsMigrations.cs -> UpdateFrom1()