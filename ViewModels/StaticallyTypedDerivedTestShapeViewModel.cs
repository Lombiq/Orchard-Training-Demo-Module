using OrchardCore.DisplayManagement.Views;

namespace Lombiq.TrainingDemo.ViewModels;

public class StaticallyTypedDerivedTestShapeViewModel : ShapeViewModel
{
    public int SomeValue { get; set; }
    public int SomeOtherValue { get; set; }

    public StaticallyTypedDerivedTestShapeViewModel() => Metadata.Type = "StaticallyTypedDerivedTestShape";
}
