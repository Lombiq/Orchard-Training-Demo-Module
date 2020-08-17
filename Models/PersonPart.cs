/*
 * Now let's see what practices Orchard Core provides when it stores data. Here you can see a content part. Each
 * content part can be attached to one or more content types. From the content type you can create content items (so
 * you kind of "instantiate" content types as content items). This is the most important part of the Orchard Core's
 * content management.
 *
 * Here is a PersonPart containing some properties of a person. We are planning to attach this content part to a Person
 * content type so when you create a Person content item it will have a PersonPart attached to it. You also need to
 * register this class with the service provider (see: Startup.cs).
 */

using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;
using System;

namespace Lombiq.TrainingDemo.Models
{
    public class PersonPart : ContentPart
    {
        // A ContentPart is serialized as a JSON object so you need to keep this in mind when creating properties. For
        // further information check the Json.NET documentation:
        // https://www.newtonsoft.com/json/help/html/Introduction.htm
        public string Name { get; set; }
        public Handedness Handedness { get; set; }
        public DateTime? BirthDateUtc { get; set; }

        // This is a content field. Content fields are similar to content parts, however, fields are a bit smaller
        // components encapsulating a simple editor and display for a single piece of data. Content parts could provide
        // more complex functionality and also can contain a set of fields.
        // TextField is one of Orchard's many built-in fields. To utilize it you don't need to add a property for it to
        // the part (you just need to attach it to the content type, what we're doing from migrations) but having such
        // a property is a nice shortcut to it.
        public TextField Biography { get; set; }
    }

    public enum Handedness
    {
        Right,
        Left,
    }
}

// NEXT STATION: Migrations/PersonMigrations
