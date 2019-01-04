using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lombiq.TrainingDemo.Indexes;
using Lombiq.TrainingDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using OrchardCore.DisplayManagement;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Notify;
using YesSql;

namespace Lombiq.TrainingDemo.Controllers
{
    public class StoreController : Controller, IUpdateModel
    {
        private readonly ISession _session;
        private readonly IDisplayManager<Book> _bookDisplayManager;
        private readonly INotifier _notifier;
        private readonly IHtmlLocalizer H;


        public StoreController(
            ISession session,
            IDisplayManager<Book> bookDisplayManager,
            INotifier notifier,
            IHtmlLocalizer<StoreController> htmlLocalizer)
        {
            _session = session;
            _bookDisplayManager = bookDisplayManager;
            _notifier = notifier;
            H = htmlLocalizer;
        }


        [HttpGet]
        public ActionResult CreateBooks() => View();

        [HttpPost, ActionName(nameof(CreateBooks))]
        public ActionResult CreateBooksPost()
        {
            foreach (var book in CreateDemoBooks())
            {
                _session.Save(book);
            }

            _notifier.Information(H["Books have been created in the database."]);

            return RedirectToAction(nameof(CreateBooks));
        }

        public async Task<ActionResult> JKRowlingBooks()
        {
            var jkRowlingBooks = await _session
                .Query<Book, BookIndex>()
                .Where(book => book.Author == "J.K. (Joanne) Rowling")
                .ListAsync();

            var bookShapes = await Task.WhenAll(jkRowlingBooks.Select(async book =>
                await _bookDisplayManager.BuildDisplayAsync(book, this, "Description")));

            return View(bookShapes);
        }


        private IEnumerable<Book> CreateDemoBooks() =>
            new Book[]
            {
                new Book
                {
                    CoverPhotoUrl = "/Lombiq.TrainingDemo/Images/HarryPotter.jpg",
                    Title = "Harry Potter and The Sorcerer's Stone",
                    Author = "J.K. (Joanne) Rowling",
                    Description = "Harry hasn't had a birthday party in eleven years - but all that is about to " +
                        "change when a mysterious letter arrives with an invitation to an incredible place."
                },
                new Book
                {
                    Title = "Fantastic Beasts and Where To Find Them",
                    Author = "J.K. (Joanne) Rowling",
                    Description = "With his magical suitcase in hand, Magizoologist Newt Scamander arrives in New " +
                        "York in 1926 for a brief stopover. However, when the suitcase is misplaced and some of his " +
                        "fantastic beasts escape, there will be trouble for everyone."
                },
                new Book
                {
                    Title = "The Hunger Games",
                    Author = "Suzanne Collins",
                    Description = "The nation of Panem, formed from a post-apocalyptic North America, is a country " +
                        "that consists of a wealthy Capitol region surrounded by 12 poorer districts."
                }
            };
    }
}