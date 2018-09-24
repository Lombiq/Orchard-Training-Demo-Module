/*
 * In this controller we'll dive a bit deeper into the built-in services of Orchard as well as learn how to create and 
 * edit content items.
 * This controller is an admin controller: the user will be able to only access it with sufficient privilages to access 
 * the dashboard.
 * 
 * Note that although we demonstrate content management in an admin controller here you can use the exact same techniques 
 * in a frontend
 * controller too.
 */

using System.Linq;
using System.Web.Mvc;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Core.Common.Models;
using Orchard.Data;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Mvc;
using Orchard.Security;
using Orchard.UI.Admin;
using Orchard.UI.Notify;
using OrchardHUN.TrainingDemo.Models;

namespace OrchardHUN.TrainingDemo.Controllers
{
    [OrchardFeature("OrchardHUN.TrainingDemo.Contents")]
    /*
     * The Admin attribute tells Orchard that this is and admin controller (or we could use the convention of naming it
     * "AdminController" for the same effect).
     * This means that some authorization automatically happens: only users with sufficient privilages to access the
     * dashboard will be able to access this controller. If we want more fine-grained access control we have to implement 
     * our own permissions, which we will!
     * 
     * Implementing IUpdateModel is needed so we can use model binding for content items through this controller. You'll 
     * see it down here.
     */
    [Admin]
    public class ContentsAdminController : Controller, IUpdateModel
    {
        private readonly IOrchardServices _orchardServices;
        private readonly IAuthorizer _authorizer;
        private readonly IContentManager _contentManager;
        private readonly ITransactionManager _transactionManager;

        public Localizer T { get; set; }


        /*
         * Injecting dependencies
         * 
         * IOrchardServices is a collection of the most important services. It also includes the Notifier we've used in 
         * the beginning.
         * 
         * TransactionManager - and if you aren't sitting, sit down now, because this will get you by surprise - is used 
         * to manage transactions.
         * As you may know every request in Orchard is one big transaction. If anything fails badly, everything will be 
         * rolled back. We'll use it when saving content items.
         */
        public ContentsAdminController(IOrchardServices orchardServices)
        {
            _orchardServices = orchardServices;
            // If we use a service multiple times it's convenient to store its reference individually
            _authorizer = orchardServices.Authorizer;
            _contentManager = orchardServices.ContentManager;
            _transactionManager = orchardServices.TransactionManager;

            T = NullLocalizer.Instance;
        }


        /*
         * We'll display a small dashboard for Person Lists and a form for editing a new or existing content item from 
         * this action.
         * 
         * NEXT STATION: we could access it through the default conventional route but we want a nicer one: that's why 
         * we create a route provider.
         * Check out the Routes class and come back here!
         * 
         * NEXT STATION: (don't worry, there won't be a call stack deeper than one :-)) we also need some purrmissions 
         * for access control. Take a look at Permissions and don't forget to get back here!
         * 
         * NEXT STATION: also we want to have an admin menu item for this action. Check out AdminMenu and come back!
         */
        public ActionResult PersonListDashboard(int id = 0)
        {
            // IsAuthorized() is our method, check it out!
            if (!IsAuthorized()) return new HttpUnauthorizedResult();

            // Check out the GetItem() method!
            var item = GetItem(id);
            if (item == null) return new HttpNotFoundResult(); // No item with the specified id exists...

            // Go to this method's definition to see what we have there!
            return PersonListDashboardShapeResult(item);
        }

        [HttpPost, ActionName(nameof(ContentsAdminController.PersonListDashboard))]
        public ActionResult PersonListDashboardPost(int id = 0)
        {
            if (!IsAuthorized()) return new HttpUnauthorizedResult();

            /* 
             * The workflow you see below for content item editing is standard practice:
             * 1) Fetch existing item from the ContentManager or instantiate new one
             * 2) Create item if it's new. Important: the item should be created as Draft.
             * 3) Update
             * 4) Check for validity:
             *      - Cancel transaction if model state is invalid
             *      - Do nothing else if model state is valid
             */

            var item = GetItem(id);
            if (item == null) return new HttpNotFoundResult();

            // If the item's id is 0 then it's not yet persisted; Create() actually persists the item.
            // The version should be Draft so any handler (e.g the one of Autoroute) that depends on the item's content 
            // being set runs only when the item is already filled with data and published then.
            if (id == 0) _contentManager.Create(item, VersionOptions.Draft);

            // Notice that there's also a _contentManager.Remove(item) method you can use for removing content item. 
            // Beware though that removals in Orchard are soft deletes: really nothing is deleted from the database.

            // Updating the item with the model binder through this controller from POST data. The method returns an
            // updated editor shape (that is filled out with the input the user gave) what we can use to display if there
            // were validation errors.
            var editorShape = _contentManager.UpdateEditor(item, this);

            // Here we were updating a content item from POST data with the model binder. This is the case if you take
            // user input and want to update an item with it. However you can directly modify a content item's parts' 
            // data directly, by "casting" the item to a part, as following:
            //item.As<PersonListPart>().MaxCount = 5;
            //item.As<TitlePart>().Title = "Custom title"; It works with any other part too, of course!

            if (!ModelState.IsValid)
            {
                // This will prevent the item from saving. Otherwise if we alter a content item's parts' properties 
                // those modifications are automatically saved.
                _transactionManager.Cancel();

                // The user will see the filled out form with validation errors emphasized
                return PersonListDashboardShapeResult(item);
            }

            // Publish the item. This will also run all the handlers (e.g the one of Autoroute) that depend on the item's 
            // content being set.
            _contentManager.Publish(item);

            // We use the notifier again and access the current user's data from the WorkContext again
            _orchardServices.Notifier.Information(
                T("{0}, the Person List item was successfully saved.", _orchardServices.WorkContext.CurrentUser.UserName));

            return RedirectToAction(nameof(ContentsAdminController.PersonListDashboard));

            // NEXT STATION: After you're finished with this controller see the "filters" provided by MVC and how Orchard
            // extends this functionality in Filters/ResourceFilter.cs!
        }

        public ActionResult LatestPersonLists()
        {
            /*
             * All right, slow down here because this is some serious business. Now we query content items!
             * 
             * Content querying in Orchard goes through ContentQuery that you can access through the ContentManager. 
             * It's kind of a LINQ API. It's again very important that you understand the concepts behind content types: 
             * http://docs.orchardproject.net/Documentation/Content-types
             * 
             * As you surely know now, content types and thus content items instantiated from them consist of multiple 
             * parts. When making content queries we can use the properties of these parts' records for filtering, 
             * ordering, etc.
             * 
             * We'll use these items in the template to display a list of already existing Person List items.
             * 
             * So let's see this step by step. (ContentQuery of course has a bunch of other methods as well, check them 
             * out yourself!)
             */
            var latestPersonLists = _contentManager
                // First we open a query. The method has multiple overloads but here we want to fetch the latest versions
                // of "PersonList" content items
                                    .Query(VersionOptions.Latest, "PersonList")
                // This is like an inner join with the PersonListPartRecord table. By default Orchard loads parts'
                // records lazily, when needed. If we know we'll use a part's data it's better to prefetch the record
                // because this way we'll have one query instead of one per item. However, this is not really needed
                // here: PersonListPart is a "shifted" part (using infoset storage) so it doesn't need this optimization,
                // it's fast out of the box.
                                    .Join<PersonListPartRecord>()
                // Also joining in CommonPartRecord. You remember we added CommonPart to PersonList, right? Go back to
                // ContentsMigrations if you'd like to refresh your memories on how we built our type.
                                    .Join<CommonPartRecord>()
                // We use CommonPartRecord's ModifiedUtc property for ordering.
                                    .OrderByDescending(record => record.ModifiedUtc)
                // Query hints are also a tool for optimization: by specifying what to eagerly load we can also cut down
                // on the number of DB queries if we know what we'll use later. Here we tell Orchard to eagerly load
                // AutoroutePartRecord and TitlePartRecord too; we've already told it to join in PersonListPartRecord and
                // CommonPartRecord. Again this is something not needed for these parts as they're shifted. Should you
                // deal with unshifted parts this comes handy.
                                    .WithQueryHints(new QueryHints().ExpandRecords(new string[] { "AutoroutePartRecord", "TitlePartRecord" }))
                // Skipping the first item and only taking the first 10 items. This method also executes the query and
                // returns the items. If we would like all the items we would have used List().
                                    .Slice(1, 10);

            // We're passing the queried objects to the ad-hoc created shape, which will use the template in
            // Views/LatestPersonLists.cshtml
            return new ShapeResult(this, _orchardServices.New.LatestPersonLists(LatestPersonLists: latestPersonLists));
        }

        public ActionResult LatestPersonList()
        {
            // We've skipped the first (latest) item from LatestPersonLists, but why?
            // Because we want to display it individually! So let's build its display shape.
            var latestPersonList = _contentManager
                                        .Query(VersionOptions.Latest, "PersonList")
                // IContentQuery<TPart, TRecord> is needed for WithQueryHintsFor() in the next step. This is returned
                // from OrderBy* among others.
                                        .OrderByDescending<CommonPartRecord>(record => record.ModifiedUtc)
                // Another option to specify query hints. This essentially tells Orchard to eagerly load everything that
                // corresponds to our content items.
                                        .WithQueryHintsFor("PersonList")
                                        .Slice(1)
                // This will return null if there are no existing PersonList items.
                                        .FirstOrDefault();

            // If there is no item yet, we just display a message from a template that corresponds to an ad-hoc shape we
            // here create through the shape factory.
            if (latestPersonList == null) return new ShapeResult(this, _orchardServices.New.LatestPersonListEmpty());

            // Building the display shape for the item, here for the Summary display type what is also used when the item
            // is displayed in a list. Note that we use Summary here as it's already implemented and we also use it on
            // the frontend. For admin lists however the widely used display type is SummaryAdmin (and BTW the third
            // common one is Detail: this is if you open an item from the frontend).
            return new ShapeResult(this, _contentManager.BuildDisplay(latestPersonList, "Summary"));
        }

        #region IUpdateModel members
        // Model binding of content items will use these two methods
        bool IUpdateModel.TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties, string[] excludeProperties) =>
            TryUpdateModel(model, prefix, includeProperties, excludeProperties);

        void IUpdateModel.AddModelError(string key, LocalizedString errorMessage) =>
            ModelState.AddModelError(key, errorMessage.ToString());
        #endregion


        private ShapeResult PersonListDashboardShapeResult(ContentItem item)
        {
            // The editor shape is the tree of shapes containing every shape needed to build the editor of the item. E.g.
            // it contains all the parts' editor shapes that build up the item.
            var itemEditorShape = _contentManager.BuildEditor(item);

            /* 
             * Now this is interesting! We've created a new ad-hoc shape here, called PersonListDashboard and passed it
             *  some data.
             * 
             * Essentially we instantiated a new dynamic shape object here (called PersonListDashboard) and added 
             * properties to it. We use the shape here as a kind of view model. However since this shape, inside a 
             * ShapeResult, is returned from our action it will be used like a view too: Orchard will display it (just 
             * as we call Display() on the Person List item's editor shape in the editor template as you'll soon see), 
             * meaning Orchard will try to find a corresponding template for the shape.
             * 
             * Orchard will try to find a corresponding template by probing for conventionally named files. We happen to 
             * have one under Views/TrainingDemo.PersonListDashboard. Notice two things: first, dots in template names 
             * are converted to underscores in shape names (and dashes to double underscores) and that we use a kind of 
             * namespacing by prefixing the name with the module's name. Since shape names are global, this way the 
             * uniqueness can be ensured.
             * 
             * You could also create a statically typed view model and use standard MVC views too of course.
             * 
             * NEXT STATION: Check out Views/PersonListDashboard and come back here!
             * 
             * NEXT STATION: Check out the two other actions in this controller: LatestPersonLists and LatestPersonList!
             * 
             */
            var editorShape = _orchardServices.New.TrainingDemo_PersonListDashboard(EditorShape: itemEditorShape);

            return new ShapeResult(this, editorShape);
        }

        private bool IsAuthorized() =>
            // Authorizing the current user against a permission
            _authorizer.Authorize(
                Permissions.AccessPersonListDashboard, T("You're not allowed to access the Person List dashboard!"));

        private ContentItem GetItem(int id) =>
            // Instantiating a new content item of type PersonList. We'll use this new item object that only resides in
            // the memory for now (nothing persisted yet) to build an editor shape (it's form).
            id == 0 ? _contentManager.New("PersonList") : _contentManager.Get(id);  // Fetching an existing content item with the id
    }
}
