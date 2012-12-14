/*
 * In this controller we'll dive a bit deeper into the built-in services of Orchard as well as learn how to create content items.
 * This controller is an admin controller: the user will be able to only access it with sufficient privilages to access the dashboard.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.Mvc;
using Orchard.Security;
using Orchard.UI.Admin;

namespace OrchardHUN.TrainingDemo.Controllers
{
    [OrchardFeature("OrchardHUN.TrainingDemo.Contents")]
    /*
     * The Admin attribute tells Orchard that this is and admin controller (or we could use the convention of naming it "AdminController" for the
     * same effect).
     * This means that some authorization automatically happens: only users with sufficient privilages to access the dashboard will be able to
     * access this controller. If we want more fine-grained access control we have to implement our own permissions, what we will!
     */
    [Admin]
    public class ContentsAdminController : Controller
    {
        // IOrchardServices is a collection of the most important services. It also includes the Notifier we've used in the beginning.
        private readonly IOrchardServices _orchardServices;
        private readonly IAuthorizer _authorizer;
        private readonly IContentManager _contentManager;


        public ContentsAdminController(IOrchardServices orchardServices)
        {
            _orchardServices = orchardServices;
            _authorizer = orchardServices.Authorizer; // If we use a service multiple times it's convenient to store its reference individually
            _contentManager = orchardServices.ContentManager;
        }


        /*
         * We'll display a form for creating a new content item from this action.
         * 
         * NEXT STATION: we could access it through the default conventional route but we want a nicer one: that's why we create a route provider.
         * Check out the Routes class and come back here!
         * 
         * NEXT STATION: also we want to have an admin menu item for this action. Check out AdminMenu and come back!
         */
        public ShapeResult Create()
        {
            // Instantiating a new content item of type PersonList.
            // We'll use this new item object that only resides in the memory for now (nothing persisted yet) to build an editor shape (it's form).
            var newItem = _contentManager.New("PersonList");

            // Go to this method's definition to see what we have there!
            return PersonListEditorShapeResult(newItem);
        }

        public ActionResult CreatePost()
        {
            // NEXT STATION: 
        }

        private ShapeResult PersonListEditorShapeResult(IContent listItem)
        {
            // The editor shape is the tree of shapes containing every shape needed to build the editor of the item. E.g. it contains all the parts'
            // editor shapes that build up the item.
            var editorShape = _contentManager.BuildEditor(listItem);

            // Now this is interesting! We've created a new ad-hoc shape here, called PersonListEditor and passed it the editor shape of our item.
            // NEXT STATION: Orchard will try to find a corresponding templates by probing for conventionally named files. We happen to have one 
            // under Views/PersonList. Check it out and come back here!
            var formShape = _orchardServices.New.PersonListEditor(EditorShape: editorShape);

            return new ShapeResult(this, formShape);
        }
    }
}