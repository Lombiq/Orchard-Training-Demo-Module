using System;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace Lombiq.TrainingDemo.Models
{
    public class PersonPart : ContentPart
    {
        public string Name { get; set; }
        public Handedness Handedness { get; set; }
        public DateTime? BirthDateUtc { get; set; }
        public TextField Biography { get; set; }
    }

    public enum Handedness
    {
        Right,
        Left
    }
}