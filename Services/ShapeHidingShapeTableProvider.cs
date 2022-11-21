using Lombiq.TrainingDemo.ViewModels;
using Microsoft.AspNetCore.Html;
using OrchardCore.DisplayManagement.Descriptors;

namespace Lombiq.TrainingDemo.Services;

// Now that you've learned a lot of tricks about shapes, let's see how you can hook into their lifecycle. A common task
// is to hide shapes conditionally. Usually, you'd use some built-in placement mechanisms for this (see:
// https://docs.orchardcore.net/en/latest/docs/reference/modules/Placements/) but sometimes you just need to apply some
// custom logic. How can you do that? By implementing an IShapeTableProvider!
internal class ShapeHidingShapeTableProvider : IShapeTableProvider
{
    // The whole thing starts with Discover(), and then you can target specific shapes with Describe(). Hiding a shape
    // can happen in one of the two ways demonstrated below but you don't need both.
    public void Discover(ShapeTableBuilder builder) => builder
        .Describe("PersonPart")
        // One option is to overwrite the shape's output from an OnDisplaying event. Here you have the full context
        // available, like the content part and content item for a part's display shape, as with PersonPart. However, at
        // this point, the shape's factory has already been executed, thus if it's slow to compute (like it fetches data
        // from a slow web service), then we've already waited for it.
        .OnDisplaying(displaying =>
        {
            // Check out what happens if you change the Name of a Person Page to contain the word "Hidden" and then view
            // it on the frontend!
            if (displaying.Shape is PersonPartViewModel viewModel && viewModel.Name.Contains("Hidden"))
            {
                displaying.ChildContent = HtmlString.Empty;
            }

            // You can also do a lot more in such an event, e.g. changing the shape's metadata via
            // displaying.Shape.Metadata (see:
            // https://docs.orchardcore.net/en/latest/docs/reference/core/Placement/#manipulating-shape-metadata), like
            // adding custom wrappers or alternates
            // (https://docs.orchardcore.net/en/latest/docs/reference/modules/Themes/#alternates).
        })
        // Another option is to override the shape's placement, completely disabling the shape. This way, its factory
        // won't be executed either. However, there's only very little context available. We target the "Hidden"
        // DisplayType here, otherwise all our PersonParts would be hidden.
        .Placement(context => context.DisplayType == "Hidden" ? new PlacementInfo { Location = "-" } : null);

    // It's also possible to extend placement.json file processing with IPlacementNodeFilterProvider and
    // IPlacementInfoResolver implementations. Check them out in the Orchard source code if you're interested! Also, if
    // you're just interested in shape events, you can implement IShapeDisplayEvents instead.
}

// END OF TRAINING SECTION: Shape tables

// NEXT STATION: Controllers/CacheController.cs
