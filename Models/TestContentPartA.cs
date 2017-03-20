using Orchard.ContentManagement;
using Orchard.DisplayManagement.Shapes;

namespace OrchardHUN.TrainingDemo.Models
{
    public class TestContentPartA : ContentPart
    {
        public ShapeMetadata Metadata { get; set; }
        public string Line { get; set; }
    }
}