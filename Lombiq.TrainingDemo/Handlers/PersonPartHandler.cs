using Lombiq.TrainingDemo.Models;
using OrchardCore.ContentManagement.Handlers;
using System.Threading.Tasks;

namespace Lombiq.TrainingDemo.Handlers;

// Handlers are basically event handlers for content parts and content items. When you ask something like "how can I run
// my code when my content part is published?" most possibly the answer will be to write a handler. This one here is a
// handler for a content part, but you could similarly have a handler for whole content items by inheriting from
// ContentHandlerBase.
public class PersonPartHandler : ContentPartHandler<PersonPart>
{
    // Did you notice that when you list Person content items on the dashboard the title of the list item also says the
    // person's name? This is because what's displayed there is the content item's DisplayText (you can think of it as a
    // universal title) that we actually set here. Here we implement UpdatedAsync() which runs every time after a
    // content item is updated. Check out all the other events that you can use!
    public override Task UpdatedAsync(UpdateContentContext context, PersonPart part)
    {
        // Note that the Title Part already does this specific thing. So if that's all you wanted from a handler than
        // using Title Part instead of a custom field and handler combination makes more sense. 
        context.ContentItem.DisplayText = part.Name;

        return Task.CompletedTask;
    }

    // If you've ever used Orchard Core before 3.0, "UpdatedAsync" used to be called during content item creation as
    // well. Since 3.0 it's intentionally not the case, so you have to apply such a bridge code if you use "Updating*"
    // or "Updated*" methods. Alternatively, put the shared code in a private method and call it from both.
    public override Task CreatedAsync(CreateContentContext context, PersonPart part) =>
        UpdatedAsync(new UpdateContentContext(context.ContentItem), part);
}

// END OF TRAINING SECTION: Content Item display management and queries
// NEXT STATION: Fields/ColorField.cs*@
