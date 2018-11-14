using System;
using OrchardCore.ContentManagement;

namespace Lombiq.TrainingDemo.Models
{
    public class PersonPart : ContentPart
    {
        public string Name { get; set; }
        public Sex Sex { get; set; }
        public DateTime? BirthDateUtc { get; set; }
        public string Biography { get; set; }
    }


    public enum Sex
    {
        Male,
        Female
    }
}