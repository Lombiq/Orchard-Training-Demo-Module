/*
 * Now it's time to save something to the database. Orchard Core uses YesSql to store data in database which is
 * document database interface for relational databases. Simply put, you need to design your database as a document
 * database but it will be stored in your favorite SQL database. If you want to learn more go to
 * https://github.com/sebastienros/yessql and read the documentation.
 *
 * Here you will see how to store simple data in the database and then query it without actually using Orchard Core
 * content management features and practices (i.e. you can store non-Orchard Core content items).
 *
 * This demonstration will be really simple because more features will be shown later and you can also learn more from
 * the YesSql documentation.
 */

using Lombiq.TrainingDemo.Indexes;
using Lombiq.TrainingDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using OrchardCore.DisplayManagement;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Notify;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YesSql;

namespace Lombiq.TrainingDemo.Controllers
{
    public class DatabaseStorageController : Controller
    {
        private readonly ISession _session;
        private readonly IDisplayManager<Book> _bookDisplayManager;
        private readonly INotifier _notifier;
        private readonly IHtmlLocalizer H;
        private readonly IUpdateModelAccessor _updateModelAccessor;


        public DatabaseStorageController(
            ISession session,
            IDisplayManager<Book> bookDisplayManager,
            INotifier notifier,
            IHtmlLocalizer<DatabaseStorageController> htmlLocalizer,
            IUpdateModelAccessor updateModelAccessor)
        {
            _session = session;
            _bookDisplayManager = bookDisplayManager;
            _notifier = notifier;
            _updateModelAccessor = updateModelAccessor;
            H = htmlLocalizer;
        }


        // A page with a button that will call the CreateBooks POST action.
        // See it under /Lombiq.TrainingDemo/DatabaseStorage/CreateBooks.
        [HttpGet]
        public ActionResult CreateBooks() => View();

        // Note the ValidateAntiForgeryToken attribute too: This validates the XSRF-prevention token automatically
        // added in the form (check for the input field named __RequestVerificationToken in the HTML output) of the
        // CreateBooks view.
        [HttpPost, ActionName(nameof(CreateBooks)), ValidateAntiForgeryToken]
        public ActionResult CreateBooksPost()
        {
            // For demonstration purposes this will create 3 books and store them in the database one-by-one using the
            // ISession service. Note that you can even go to the database directly, circumventing YesSql too, by
            // injecting the IDbConnectionAccessor service and access the underlying connection.

            // Since storing them in the documents is not enough we need to index them to be able to
            // filter them in a query.
            // NEXT STATION: Indexes/BookIndex.cs
            foreach (var book in CreateDemoBooks())
            {
                // So now you understand what will happen in the background when this service is being called.
                _session.Save(book);
            }

            _notifier.Information(H["Books have been created in the database."]);

            return RedirectToAction(nameof(CreateBooks));
        }

        // This page will display the books written by J.K. Rowling.
        // See it under /Lombiq.TrainingDemo/DatabaseStorage/JKRowlingBooks.
        public async Task<ActionResult> JKRowlingBooks()
        {
            // ISession service is used for querying items.
            var jkRowlingBooks = await _session
                // First, we define what object (document) we want to query and what index should be used for
                // filtering.
                .Query<Book, BookIndex>()
                // In the .Where() method you can describe a lambda where the object will be the index object.
                .Where(index => index.Author == "J.K. (Joanne) Rowling")
                // When the query is built up you can call ListAsync() to execute it. This will return a list of books.
                .ListAsync();

            // Now this is what we possibly understand now, we will create a list of display shapes from the previously
            // fetched books.
            var bookShapes = await Task.WhenAll(jkRowlingBooks.Select(async book =>
                // We'll need to pass an IUpdateModel (used for model validation) to the method, which we can access
                // via its accessor service. Later you'll also see how we'll use this to run validations in drivers.
                await _bookDisplayManager.BuildDisplayAsync(book, _updateModelAccessor.ModelUpdater)));

            // You can check out Views/DatabaseStorage/JKRowlingBooks.cshtml and come back here.
            return View(bookShapes);
        }

        // END OF TRAINING SECTION: Storing data in document database and index records

        // NEXT STATION: Models/PersonPart.cs


        private static IEnumerable<Book> CreateDemoBooks() =>
            new[]
            {
                new Book
                {
                    CoverPhotoUrl = "/Lombiq.TrainingDemo/Images/HarryPotter.jpg",
                    Title = "Harry Potter and The Sorcerer's Stone",
                    Author = "J.K. (Joanne) Rowling",
                    Description = "Harry hasn't had a birthday party in eleven years - but all that is about to " +
                        "change when a mysterious letter arrives with an invitation to an incredible place.",
                },
                new Book
                {
                    Title = "Fantastic Beasts and Where To Find Them",
                    Author = "J.K. (Joanne) Rowling",
                    Description = "With his magical suitcase in hand, Magizoologist Newt Scamander arrives in New " +
                        "York in 1926 for a brief stopover. However, when the suitcase is misplaced and some of his " +
                        "fantastic beasts escape, there will be trouble for everyone.",
                },
                new Book
                {
                    Title = "The Hunger Games",
                    Author = "Suzanne Collins",
                    Description = "The nation of Panem, formed from a post-apocalyptic North America, is a country " +
                        "that consists of a wealthy Capitol region surrounded by 12 poorer districts.",
                },
            };
    }
}
