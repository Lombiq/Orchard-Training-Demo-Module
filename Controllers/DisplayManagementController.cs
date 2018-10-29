/*
 * In this section you will learn how Orchard Core deals with displaying different so-called shapes used for displaying various
 * information to the users. This is a very huge and powerful part of Orchard Core, here you will learn the basics of Display
 * Management.
 * 
 * Firstly, to demonstrate this basic functionality, we will create a page for displaying some basic info which is part of a
 * very basic object and will also add an editor for that.
 * Secondly, we will see how we can split the page for multiple reusable views (i.e. shapes) using a more-or-less real-life
 * example.
 * 
 * NEXT STATION: Check the Book class to see what properties it contains. Go to Models/Book.
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


        private Book CreateDemoBook() =>
            new Book
            {
                Title = "Harry Potter and The Sorcerer's Stone",
                Author = "J.K. (Joanne) Rowling",
                Summary = "Harry hasn't had a birthday party in eleven years - but all that is about to change when a mysterious " +
                    "letter arrives with an invitation to an incredible place.",
                Excerpt = "Nearly ten years had passed since the Dursleys had woken up to find their nephew on the front step, " +
                    "but Privet Drive had hardly changed at all. The sun rose on the same tidy front gardens and lit up the brass " +
                    "number four on the Dursleys' front door; it crept into their living room, which was almost exactly the same " +
                    "as it had been on the night when Mr. Dursley had seen that fateful news report about the owls. Only the " +
                    "photographs on the mantelpiece really showed how much time had passed. Ten years ago, there had been lots of " +
                    "pictures of what looked like a large pink beach ball wearing different-colored bonnets - but Dudley Dursley " +
                    "was no longer a baby, and now the photographs showed a large blond boy riding his first bicycle, on a " +
                    "carousel at the fair, playing a computer game with his father, being hugged and kissed by his mother. The " +
                    "room held no sign at all that another boy lived in the house, too. "
            };
    }
}

/*
 * If you've finished with both actions (and their .cshtml files as well), then
 * NEXT STATION: Controllers/BasicOrchardCoreServicesController is what's next.
 */
