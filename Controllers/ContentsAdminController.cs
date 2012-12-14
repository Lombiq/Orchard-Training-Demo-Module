/*
 * In this controller we'll dive a bit deeper into the built-in services of Orchard as well as learn how to create and edit content items.
 * This controller is an admin controller: the user will be able to only access it with sufficient privilages to access the dashboard.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
using OrchardHUN.TrainingDemo.Models;
using Orchard.UI.Notify;

namespace OrchardHUN.TrainingDemo.Controllers
{
    [OrchardFeature("OrchardHUN.TrainingDemo.Contents")]
    /*
     * The Admin attribute tells Orchard that this is and admin controller (or we could use the convention of naming it "AdminController" for the
     * same effect).
     * This means that some authorization automatically happens: only users with sufficient privilages to access the dashboard will be able to
     * access this controller. If we want more fine-grained access control we have to implement our own permissions, what we will!
     * 
     * Implementing IUpdateModel is needed so we can use model binding for content items through this controller. You'll see it down here.
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
         * IOrchardServices is a collection of the most important services. It also includes the Notifier we've used in the beginning.
         * 
         * ITransactionManager - and if you aren't sitting, sit down now, because this will get you by surprise - is used to manage transactions.
         * As you may know every request in Orchard is one big transaction. If anything fails badly, everything will be rolled back. We'll
         * use it when saving content items.
         */
        public ContentsAdminController(IOrchardServices orchardServices, ITransactionManager transactionManager)
        {
            _orchardServices = orchardServices;
            _authorizer = orchardServices.Authorizer; // If we use a service multiple times it's convenient to store its reference individually
            _contentManager = orchardServices.ContentManager;

            _transactionManager = transactionManager;

            T = NullLocalizer.Instance;
        }


        /*
         * We'll display a small dashboard for Person Lists and a form for editing a new or existing content item from this action.
         * 
         * NEXT STATION: we could access it through the default conventional route but we want a nicer one: that's why we create a route provider.
         * Check out the Routes class and come back here!
         * 
         * NEXT STATION: (don't worry, there won't be a call stack deeper than one :-)) we also need some purrmissions for access control. Take
         * a look at Permissions and don't forget to get back here!
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

        [HttpPost, ActionName("PersonListDashboard")]
        public ActionResult PersonListDashboardPost(int id = 0)
        {
            if (!IsAuthorized()) return new HttpUnauthorizedResult();

            /* 
             * The workflow you see below for content item editing is standard practice:
             * 1) Fetch existing item from the ContentManager or isntantiate new one
             * 2) Create item if it's new
             * 3) Update
             * 4) Check for validity:
             *      - Cancel transaction if model state is invalid
             *      - Do nothing else if model state is valid
             */

            var item = GetItem(id);
            if (item == null) return new HttpNotFoundResult();

            // Create() actually persists the item
            if (id == 0) _contentManager.Create(item);

            // Updating the item with the model binder through this controller from POST data.
            // The method returns an updated editor shape (that is filled out with the input the user gave) what we can use to display if
            // there were validation errors.
            var editorShape = _contentManager.UpdateEditor(item, this);

            if (!ModelState.IsValid)
            {
                // This will prevent the item from saving. Otherwise if we alter a content item's parts' properties those modifications are
                // automatically saved.
                _transactionManager.Cancel();

                // The user will see the filled out form with validation errors emphasized
                return PersonListDashboardShapeResult(item);
            }

            // We use the notifier again and access the current user's data from the WorkContext again
            _orchardServices.Notifier.Information(T("{0}, the Person List item was successfully saved.", _orchardServices.WorkContext.CurrentUser.UserName));

            return RedirectToAction("PersonListDashboard");

            // NEXT STATION: 
        }

        #region IUpdateModel members
        // Model binding of content items will use these two methods
        bool IUpdateModel.TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties, string[] excludeProperties)
        {
            return TryUpdateModel(model, prefix, includeProperties, excludeProperties);
        }

        void IUpdateModel.AddModelError(string key, LocalizedString errorMessage)
        {
            ModelState.AddModelError(key, errorMessage.ToString());
        }
        #endregion


        private ShapeResult PersonListDashboardShapeResult(ContentItem item)
        {
            // The editor shape is the tree of shapes containing every shape needed to build the editor of the item. E.g. it contains all the parts'
            // editor shapes that build up the item.
            var itemEditorShape = _contentManager.BuildEditor(item);

            /*
             * All right, slow down here because this is some serious business. Now we query content items!
             * 
             * Content querying in Orchard goes through ContentQuery that you can access through the ContentManager. It's kind of a LINQ API.
             * It's again very important that you understand the concepts behind content types: http://docs.orchardproject.net/Documentation/Content-types
             * 
             * As you surely know now, content types and thus content items instantiated from them consist of multiple parts. When making
             * content queries we can use the properties of these parts' records for filtering, ordering, etc.
             * 
             * We'll use these items in the template to display a list of already existing Person List items.
             * 
             * So let's see this step by step. (ContentQuery of course has a bunch of other methods as well, check them out yourself!)
             */
            var latestPersonLists = _contentManager
                // First we open a query. The method has multiple overloads but here we want to fetch the latest versions of "PersonList" content
                // items
                                    .Query(VersionOptions.Latest, "PersonList")
                // This is like an inner join with the PersonListPartRecord table. By default Orchard loads parts' records lazily, when needed.
                // If we know we'll use a part's data it's better to prefetch the record because this way we'll have one query instead of one
                // per item.
                                    .Join<PersonListPartRecord>()
                // Also joining in CommonPartRecord. You remember we added CommonPart to PersonList, right? Go back to ContentsMigrations
                // if you'd like to refresh your memories on how we built our type.
                                    .Join<CommonPartRecord>()
                // We also use CommonPartRecord' ModifiedUtc property for ordering.
                                    .OrderByDescending(record => record.ModifiedUtc)
                // Query hints are also a tool for optimization: by specifying what to eagerly load we can also cut down on the number of DB queries
                // if we know what we'll use later. Here we tell Orchard to eagerly load AutoroutePartRecord and TitlePartRecord too; we've already
                // told it to join in PersonListPartRecord and CommonPartRecord.
                                    .WithQueryHints(new QueryHints().ExpandRecords(new string[] { "AutoroutePartRecord", "TitlePartRecord" }))
                // Skipping the first item and only taking the first 10 items. This method also executes the query and returns the items.
                // If we would like all the items we would have used List().
                                    .Slice(1, 10);

            // We've skipped the first (latest) item, but why? Because we want to display it individually! So let's build its display shape.
            var latestPersonList = _contentManager
                                        .Query(VersionOptions.Latest, "PersonList")
                // IContentQuery<TPart, TRecord> is needed for WithQueryHintsFor() in the next step. This is returned from OrderBy* among others.
                                        .OrderByDescending<CommonPartRecord>(record => record.ModifiedUtc)
                // Another option to specify query hints. This essentially tells Orchard to eagerly load everything that corresponds to our
                // content items.
                                        .WithQueryHintsFor("PersonList")
                                        .Slice(1)
                // We here assume that there is at least one PersonList item. Otherwise we'd use FirstOrDefault() and check for null, but you
                // created some PersonList items, didn't you?
                                        .First();

            // Building the display shape for the item, here for the Summary display type what is also used when the item is displayed in a list.
            var latestPersonListDisplayShape = _contentManager.BuildDisplay(latestPersonList, "Summary");


            /* 
             * Now this is interesting! We've created a new ad-hoc shape here, called PersonListDashboard and passed it some data.
             * 
             * Essentially we instantiated a new dynamic shape object here (called PersonListDashboard) and added properties to it. We use the shape
             * here as a kind of view model here. However since this shape, inside a ShapeResult, is returned from our action it will be used
             * like a view too: Orchard will display it (just as we call Display() on the Person List item's editor shape in the editor template
             * as you'll soon see), meaning Orchard will try to find a corresponding template for the shape.
             * 
             * Orchard will try to find a corresponding template by probing for conventionally named files. We happen to have one under 
             * Views/PersonListDashboard.
             * 
             * You could also create a statically typed view model and use standard MVC views too of course.
             * 
             * NEXT STATION: Check out Views/PersonListDashboard and come back here!
             */
            var editorShape = _orchardServices.New.PersonListDashboard(
                                EditorShape: itemEditorShape,
                                LatestPersonListDisplayShape: latestPersonListDisplayShape,
                                LatestPersonLists: latestPersonLists);

            return new ShapeResult(this, editorShape);
        }

        private bool IsAuthorized()
        {
            // Authorizing the current user against a permission
            return _authorizer.Authorize(Permissions.AccessPersonListDashboard, T("You're not allowed to access the Person List dashboard!"));
        }

        private ContentItem GetItem(int id)
        {
            // Instantiating a new content item of type PersonList.
            // We'll use this new item object that only resides in the memory for now (nothing persisted yet) to build an editor shape (it's form).
            if (id == 0) return _contentManager.New("PersonList");
            else return _contentManager.Get(id);  // Fetching an existing content item with the id
        }
    }
}