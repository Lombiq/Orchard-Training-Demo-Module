using Lombiq.TrainingDemo.Models;
using Lombiq.TrainingDemo.ViewModels;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using System.Threading.Tasks;

namespace Lombiq.TrainingDemo.Drivers
{
    // Drivers inherited from ContentPartDisplayDrivers have a functionality similar to the one described in
    // BookDisplayDriver but these are for ContentParts. Don't forget to register this class with the service provider
    // (see: Startup.cs).
    public class PersonPartDisplayDriver : ContentPartDisplayDriver<PersonPart>
    {
        // Some notes on the various methods you can override:
        // - Keep in mind that all of them have a sync and async version, use the one more appropriate for what you do
        //   inside them (use the async ones if you'll write any async code in the body, the sync ones otherwise).
        // - Also, some overrides will take a context object: When in doubt use the one with the context object,
        //   otherwise there is no difference apart from what you can see: The context can be passed to helper methods
        //   and is otherwise used in the background to carry some other contextual object like the current
        //   IUpdateModel. If you end up not using the context anywhere in the method's body than that means that you
        //   don't actually need it, no magic will happen behind the scenes because of it.

        // A Display method that we already know. This time it's much simpler because we don't want to create multiple
        // shapes for the PersonPart - however we could.
        public override IDisplayResult Display(PersonPart part, BuildPartDisplayContext context) =>
            // Here you have a shape helper with a shape name possibly and a factory. The Initialize method will
            // instantiate a view model from a type given as a generic parameter. It's recommended to use view models
            // for the views like we're doing it here (sometimes you'd want a separate view model for the Display() and
            // Edit().
            // There are helper methods to generate the shape type. GetDisplayShapeType() in this case will generate
            // "PersonPart" by default but this can be overridden form the part's settings under the content type's
            // settings on the admin. In the factory we map the content part properties to the view model; if there is
            // any heavy lifting needed to generate the view model (like fetching data from the database or an external
            // API) then do it in that factory. That way the work will only be done if the shape is actually displayed.
            Initialize<PersonPartViewModel>(GetDisplayShapeType(context), viewModel => PopulateViewModel(part, viewModel))
                // Note that again we're referring to a display type here just as with Books, the Detail display type.
                // While display types can be customized, by default Orchard uses "Detail" when the content item is
                // opened in its entirety on the frontend, "Summary" when it's listed on the frontend, and
                // "SummaryAdmin" when it's listed on the admin site. You can specify how a content part is displayed
                // in these scenarios with these location settings.
                // In this simple case, the same shape will be used both in Detail and Summary.

                // NEXT STATION: Check out PersonPart.cshtml quickly and come back here.

                // Note that we need the weight 1 to put our part's data above the Biography coming from a Text Field.
                .Location("Detail", "Content:1")
                .Location("Summary", "Content:1");

        // This is something that wasn't implemented in the BookDisplayDriver (but could've been). It will generate the
        // editor shape for the PersonPart.
        public override IDisplayResult Edit(PersonPart part, BuildPartEditorContext context) =>
            // Something similar to the Display method happens. GetEditorShapeType() will by default generate
            // "PersonPart_Edit".

            // Notice that there is no location given for this shape (no Location() method call). There's another
            // option of giving these locations using Placement.json files. Since it is not possible to put comments in
            // a .json file the explanation is here but make sure you check the file while reading this. It's important
            // to give a location somewhere otherwise to shape won't be displayed. The shape file should be in the
            // Views folder by default, however, it could be outside the Views folder too (e.g. inside the Drivers
            // folder).
            // In Placement files you can give specific locations to any shapes generated by Orchard. You can also
            // specify rules to match when the location will be applied: like only for certain fields, content types,
            // just under a given path. In our Placement file you can see that the PersonPart shape gets the first
            // position in the Content zone and the TextField shape gets the second. To make sure that not all the
            // TextFields will get the same position a "differentiator" property is given which refers to the part name
            // where the field is attached to and the field name. Make sure you also read the documentation to know
            // this feature better:
            // https://docs.orchardcore.net/en/dev/docs/reference/core/Placement/#placement-files

            // NEXT STATION: placement.json (needs to be lowercase) then come back here.
            Initialize<PersonPartViewModel>(GetEditorShapeType(context), viewModel => PopulateViewModel(part, viewModel));

        // NEXT STATION: Startup.cs and find the static constructor.

        // So we had an Edit (or EditAsync) method that generates the editor shape. Now it's time to do the content
        // part-specific model binding and validation.
        public override async Task<IDisplayResult> UpdateAsync(PersonPart part, IUpdateModel updater, UpdatePartEditorContext context)
        {
            var viewModel = new PersonPartViewModel();

            // Via the IUpdateModel you will be able to use the current controller's model binding helpers here in the
            // driver. The prefix property will be used to distinguish between similarly named input fields when
            // building the editor form (so e.g. two content parts composing a content item can have an input field
            // called "Name"). By default Orchard Core will use the content part name but if you have multiple drivers
            // with editors for a content part you need to override it in the driver.
            await updater.TryUpdateModelAsync(viewModel, Prefix);

            // Now you can do some validation if needed. One way to do it you can simply write your own validation here
            // or you can do it in the view model class.

            // Go and check the ViewModels/PersonPartViewModel to see how to do it and then come back here.

            // Finally map the view model to the content part. By default, these changes won't be persisted if there was
            // a validation error. Otherwise these will be automatically stored in the database.
            part.BirthDateUtc = viewModel.BirthDateUtc;
            part.Name = viewModel.Name;
            part.Handedness = viewModel.Handedness;

            return await EditAsync(part, context);
        }


        private static void PopulateViewModel(PersonPart part, PersonPartViewModel viewModel)
        {
            viewModel.PersonPart = part;

            viewModel.BirthDateUtc = part.BirthDateUtc;
            viewModel.Name = part.Name;
            viewModel.Handedness = part.Handedness;
        }
    }
}

// NEXT STATION: Controllers/PersonListController and go back to the OlderThan30 method where we left.
