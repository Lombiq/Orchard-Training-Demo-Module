using System;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace Lombiq.TrainingDemo.Models
{
    // Now let's see what practices Orchard Core provides when it stores data. Here you can see a ContentPart. Each
    // ContentPart can be part of one or more content types. Using the content type you can create ContentItems that is
    // the most important part of the Orchard Core content management. Here is a PersonPart containing some properties
    // of a person. You also need to register this class to the service provider (see: Startup.cs).
    public class PersonPart : ContentPart
    {
        // A ContentPart is serialized as a JSON object so you need to keep this in mind when creating properties. For
        // further information check the Json.NET documentation:
        // https://www.newtonsoft.com/json/help/html/Introduction.htm
        public string Name { get; set; }
        public Handedness Handedness { get; set; }
        public DateTime? BirthDateUtc { get; set; }

        // This is a ContentField. ContentFields are similar to ContentParts, however, fields are a bit more smaller
        // components encapsulating simple editor and display for a single data and ContentParts could have a more
        // complex functionality and also can contain a set of fields.
        public TextField Biography { get; set; }
    }

    public enum Handedness
    {
        Right,
        Left
    }
}

// NEXT STATION: Migrations/PersonMigrations