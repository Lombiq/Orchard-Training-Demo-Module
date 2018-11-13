/*
 * In this section you will learn how Orchard Core deals with displaying different so-called shapes used for displaying various
 * information to the users. This is a very huge and powerful part of Orchard Core, here you will learn the basics of Display
 * Management.
 * 
 * To demonstrate this basic functionality, we will create a page for displaying information about a book in two different pages.
 */

using Lombiq.TrainingDemo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.DisplayManagement;
using OrchardCore.DisplayManagement.ModelBinding;
using System.Threading.Tasks;

namespace Lombiq.TrainingDemo.Controllers
{
    // Notice, that the controller implements the IUpdateModel interface. This interface encapsulates all the properties and
    // methods related to ASP.NET Core MVC model binding which is already there in the Controller object so further
    // implementations are not required. Orchard Core needs this model binding functionality outside the controllers (you will
    // see it later).
    public class DisplayManagementController : Controller, IUpdateModel
    {
        // For display management functionality we can use the IDisplayManager<T> service where the T generic parameter is the
        // object you want to create shapes for.
        private readonly IDisplayManager<Book> _bookDisplayManager;


        public DisplayManagementController(IHttpContextAccessor hca, IDisplayManager<Book> bookDisplayManager)
        {
            _bookDisplayManager = bookDisplayManager;
        }

        
        // First, let's see how the book summary page is generated.
        public async Task<ActionResult> DisplayBook()
        {
            // Since we don't store the book object in the database let's create one for demonstration purposes.
            var book = CreateDemoBook();

            // Here the shape is generated. Before going any further let's dig deeper and see what happens when this method
            // is called.
            // NEXT STATION: Go to Drivers/BookDisplayDriver.
            var shape = await _bookDisplayManager.BuildDisplayAsync(book, this);

            return View(shape);
        }
        
        // Let's generate another Book display shape, but now with a display type.
        public async Task<ActionResult> DisplayBookDescription()
        {
            // Generate another book object to be used for demonstration purposes.
            var book = CreateDemoBook();

            // We can add a display type when we generate display shape. This time it will be Description.
            // If display type is given then Orchard Core will search a cshtml file with a name [ObjectName].[DisplayType].cshtml.
            // NEXT STATION: Go to Views/Book.Description.cshtml
            var shape = await _bookDisplayManager.BuildDisplayAsync(book, this, "Description");

            return View(shape);
        }


        private Book CreateDemoBook() =>
            new Book
            {
                CoverPhotoUrl = "/Lombiq.TrainingDemo/HarryPotter.jpg",
                Title = "Harry Potter and The Sorcerer's Stone",
                Author = "J.K. (Joanne) Rowling",
                Description = "Harry hasn't had a birthday party in eleven years - but all that is about to change when a mysterious " +
                    "letter arrives with an invitation to an incredible place.",
            };
    }
}

/*
 * If you've finished with both actions (and their .cshtml files as well), then
 * NEXT STATION: Controllers/BasicOrchardCoreServicesController is what's next.
 */
