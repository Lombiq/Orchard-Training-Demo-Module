using Lombiq.TrainingDemo.Models;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.Views;

namespace Lombiq.TrainingDemo.Drivers
{
    // A DisplayDriver is an abstraction over all the functionality for displaying or editing a specific object that is usually
    // done in the Controllers. You can create a driver for any object or contents (i.e. ContentParts or ContentFields) where
    // you can implement their very specific logic for generating reusable shapes or validating them after model binding. 
    // Furthermore, you can create multiple drivers for one object where all the driver methods will be executed even if the 
    // original driver is implemented in another (e.g. Orchard Core) module.
    public class BookDisplayDriver : DisplayDriver<Book>
    {
        // This method gets called when building the display shape of object. If you need to call async methods here you can
        // override DisplayAsync instead of this. Please note, that only one can be used since if the DisplayAsync is not
        // overriden then it will be called asynchronously - either way, it will be async.
        public override IDisplayResult Display(Book model) =>
            // For the sake of demonstration we use Combined() here. It makes it possible to return multiple shapes from
            // a driver method. Use this if you'd like to return different shapes that can be used e.g. with different
            // display types or you need to display specific shapes in different zones. Zones will be described later.
            Combine(
                // Here we define a shape for the Title. The shapeType parameter will also define the default file name that
                // will contain the actual markup. For this one it will be Book.Display.Title.cshtml which is located in the
                // Views/Items folder. This shape will be placed in the first position of the Header zone.
                View("Book_Display_Title", model)
                    .Location("Header: 1"),
                // Same applies here. This shape will be displayed in the Header zone too but in the second position.
                // This way we make sure that the Title goes first.
                View("Book_Display_Author", model)
                    .Location("Header: 2"),
                // The cover photo will be in a different zone.
                View("Book_Display_Cover", model)
                    .Location("Cover: 1"),
                // The description, however, won't be displayed by default because its location targets a different display type.
                // Note, that the previous shapes had no display type parameter so those will be displayed in every display type.
                // This one will be displayed in the first position of the Content zone if the Book display shape will be
                // generated with the Description display type.
                View("Book_Display_Description", model)
                    .Location("Description", "Content: 1"));

        // NEXT STATION: Now let's see what are those zones and how these shapes will come together! Go to Views/Book.cshtml.
    }
}
