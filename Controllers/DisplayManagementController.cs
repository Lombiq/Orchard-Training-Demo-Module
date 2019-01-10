/*
 * In this section you will learn how Orchard Core deals with displaying various information on the UI using reusable
 * components shapes. This is a very huge and powerful part of Orchard Core, here you will learn the basics of Display
 * Management.
 *
 * To demonstrate this basic functionality, we will create two slightly different pages for displaying information
 * about a book.
 */

using System.Threading.Tasks;
using Lombiq.TrainingDemo.Models;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.DisplayManagement;
using OrchardCore.DisplayManagement.ModelBinding;

namespace Lombiq.TrainingDemo.Controllers
{
    // Notice, that the controller implements the IUpdateModel interface. This interface encapsulates the properties
    // and methods related to ASP.NET Core MVC model binding. Orchard Core needs this model binding functionality
    // outside the controllers (you will see it later).
    public class DisplayManagementController : Controller, IUpdateModel
    {
        // The core display management features can be used by the IDisplayManagement service. The generic parameter
        // will be the object that needs to be displayed on the UI somehow. Don't forget to register this generic class
        // to the service provider (see: Startup.cs).
        private readonly IDisplayManager<Book> _bookDisplayManager;


        public DisplayManagementController(IDisplayManager<Book> bookDisplayManager)
        {
            _bookDisplayManager = bookDisplayManager;
        }


        // First, create a page that will display a summary and some additional data of the book.
        public async Task<ActionResult> DisplayBook()
        {
            // For demonstration purposes create a dummy book object.
            var book = CreateDemoBook();

            // This method will generate a shape primarily for displaying information about the given object.
            var shape = await _bookDisplayManager.BuildDisplayAsync(book, this);

            // We will see how this display shape is generated and what will contain but first let's see how is this
            // rendered in the MVC view.
            // NEXT STATION: Go to Views/DisplayManagement/DisplayBook.cshtml.

            return View(shape);
        }

        // Let's generate another Book display shape, but now with a display type.
        public async Task<ActionResult> DisplayBookDescription()
        {
            // Generate another book object to be used for demonstration purposes.
            var book = CreateDemoBook();

            // This time give an additional parameter which is the display type. If display type is given then Orchard
            // Core will search a cshtml file with a name [ObjectName].[DisplayType].cshtml.
            var shape = await _bookDisplayManager.BuildDisplayAsync(book, this, "Description");

            // NEXT STATION: Go to Views/Book.Description.cshtml

            return View(shape);
        }


        private Book CreateDemoBook() =>
            new Book
            {
                CoverPhotoUrl = "/Lombiq.TrainingDemo/Images/HarryPotter.jpg",
                Title = "Harry Potter and The Sorcerer's Stone",
                Author = "J.K. (Joanne) Rowling",
                Description = "Harry hasn't had a birthday party in eleven years - but all that is about to change " +
                    "when a mysterious letter arrives with an invitation to an incredible place.",
            };
    }
}

// If you've finished with both actions (and their .cshtml files as well), then
// NEXT STATION: Controllers/BasicOrchardCoreServicesController is what's next.