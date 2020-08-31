using Lombiq.TrainingDemo.Fields;
using Lombiq.TrainingDemo.Settings;
using Lombiq.TrainingDemo.ViewModels;
using Microsoft.Extensions.Localization;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lombiq.TrainingDemo.Drivers
{
    // You shouldn't be surprised - content fields also have display drivers. ContentFieldDisplayDriver is specifically
    // for content fields. Don't forget to register this class with the service provider (see: Startup.cs).
    public class ColorFieldDisplayDriver : ContentFieldDisplayDriver<ColorField>
    {
        private readonly IStringLocalizer T;


        public ColorFieldDisplayDriver(IStringLocalizer<ColorFieldDisplayDriver> stringLocalizer) => T = stringLocalizer;


        public override IDisplayResult Display(ColorField field, BuildFieldDisplayContext fieldDisplayContext) =>
            // Same Display method for generating display shapes but this time the Initialize shape helper is being
            // used. We've seen it in the PersonPartDisplayDriver.Edit method. For this we need a view model object
            // which will be populated with the field data. The GetDisplayShapeType helper will generate a conventional
            // shape type for our content field which will be the name the field. Obviously, alternates can also be
            // used - so if the content item is being displayed with a display type named "Custom" then the
            // ColorField.Custom.cshtml file will be used, otherwise, the ColorField.cshtml will be active.
            Initialize<DisplayColorFieldViewModel>(
                GetDisplayShapeType(fieldDisplayContext),
                viewModel =>
                {
                    viewModel.Field = field;
                    viewModel.Part = fieldDisplayContext.ContentPart;
                    viewModel.PartFieldDefinition = fieldDisplayContext.PartFieldDefinition;
                })
            .Location("Content")
            .Location("SummaryAdmin", string.Empty);

        // NEXT STATION: Take a look at the Views/ColorField.cshtml shape to see how our field should display the given
        // color and then come back here.

        public override IDisplayResult Edit(ColorField field, BuildFieldEditorContext context) =>
            // Nothing new here, the Initialize shape helper is being used to generate an editor shape.
            Initialize<EditColorFieldViewModel>(
                GetEditorShapeType(context),
                viewModel =>
                {
                    viewModel.Value = field.Value;
                    viewModel.ColorName = field.ColorName;
                    viewModel.Field = field;
                    viewModel.Part = context.ContentPart;
                    viewModel.PartFieldDefinition = context.PartFieldDefinition;
                });

        // NEXT STATION: Settings/ColorFieldSettings

        public override async Task<IDisplayResult> UpdateAsync(ColorField field, IUpdateModel updater, UpdateFieldEditorContext context)
        {
            var viewModel = new EditColorFieldViewModel();

            // Using this overload of the model updater you can specifically say what properties need to be updated.
            // This way you make sure no other properties will be bound to the view model. Instead of this you could
            // put [BindNever] attributes on the properties to make the model binder to skip those, it's up to you.
            if (await updater.TryUpdateModelAsync(viewModel, Prefix, f => f.Value, f => f.ColorName))
            {
                // Get the ColorFieldSettings to use it when validating the view model.
                var settings = context.PartFieldDefinition.GetSettings<ColorFieldSettings>();
                if (settings.Required && string.IsNullOrWhiteSpace(viewModel.Value))
                {
                    updater.ModelState.AddModelError(Prefix, T["A value is required for {0}.", context.PartFieldDefinition.DisplayName()]);
                }

                // Also some custom validation for our ColorField hex value.
                if (!string.IsNullOrWhiteSpace(viewModel.Value) &&
                    !Regex.IsMatch(viewModel.Value, "^#([A-Fa-f0-9]{8}|[A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$"))
                {
                    updater.ModelState.AddModelError(Prefix, T["The given color is invalid."]);
                }

                field.ColorName = viewModel.ColorName;
                field.Value = viewModel.Value;
            }

            return await EditAsync(field, context);
        }
    }
}

// END OF TRAINING SECTION: Content Field development

// NEXT STATION: Indexing/ColorFieldIndexHandler.cs
