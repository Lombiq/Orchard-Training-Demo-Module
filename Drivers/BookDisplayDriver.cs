using Lombiq.TrainingDemo.Models;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lombiq.TrainingDemo.Drivers
{
    // A DisplayDriver is an abstraction over all the functionality for displaying or editing a specific object that is usually
    // done in the Controllers. You can create a driver for any object or contents (i.e. ContentParts or ContentFields) where
    // you can implement their very specific logic for generating shapes or validating them after model binding. Furthermore,
    // you can create multiple drivers for one object where all the driver methods will be executed even if the original driver
    // is implemented in another (e.g. Orchard Core) module.
    public class BookDisplayDriver : DisplayDriver<Book>
    {
        // This method gets called when building the display shape of object. If you need to call async methods here you can
        // override DisplayAsync instead of this. Please note, that only one can be used since if the DisplayAsync is not
        // overriden then it will be called asynchronously - either way, it will be async.
        public override IDisplayResult Display(Book model) =>
            // For the sake of demonstration we use Combined() here. It makes it possible to return multiple shapes from
            // a driver method. Use this if you'd like to return different shapes that can be used e.g. with different
            // display types or you need to display specific shapes in different zones. Zones will be described really soon!
            Combine(
                View("Book_Title", model)
                    .Location("Header: 1"),
                View("Book_Summary", model)
                    .Location("Content: 1"));

        public override IDisplayResult Edit(Book model)
        {
            return base.Edit(model);
        }
    }
}
