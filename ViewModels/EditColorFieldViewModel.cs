using Lombiq.TrainingDemo.Fields;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata.Models;

namespace Lombiq.TrainingDemo.ViewModels
{
    public class EditColorFieldViewModel
    {
        public string ColorName { get; set; }
        public string Value { get; set; }
        public ColorField Field { get; set; }
        public ContentPart Part { get; set; }
        public ContentPartFieldDefinition PartFieldDefinition { get; set; }
    }
}
