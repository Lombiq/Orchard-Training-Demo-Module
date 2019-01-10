using System.Threading.Tasks;
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

namespace Lombiq.TrainingDemo.Drivers
{
    // You shouldn't be surprised - content fields also have display drivers. ContentFieldDisplayDriver is specifically
    // for content fields. Don't forget to register this class to the service provider (see: Startup.cs).
    public class ColorFieldDisplayDriver : ContentFieldDisplayDriver<ColorField>
    {
        public IStringLocalizer T { get; set; }


        public ColorFieldDisplayDriver(IStringLocalizer<ColorFieldDisplayDriver> stringLocalizer)
        {
            T = stringLocalizer;
        }


        public override IDisplayResult Display(ColorField field, BuildFieldDisplayContext context)
        {
            // Same Display method for generating display shape but this time the Initialize shape helper is being
            // used. We've seen it in the PersonPartDisplayDriver.Edit method. For this we need a view model object
            // that will be populated with the field data. The GetDisplayShapeType helper will generate a
            // conventionally shape type for our content field which will be the name of our content field. Obviously,
            // alternates can also be used - so if the content item is being displayed with a Custom display type then
            // the ColorField.Custom.cshtml file name can be used, otherwise, the ColorField.cshtml will be active.
            return Initialize<DisplayColorFieldViewModel>(GetDisplayShapeType(context), model =>
            {
                model.Field = field;
                model.Part = context.ContentPart;
                model.PartFieldDefinition = context.PartFieldDefinition;
            })
            .Location("Content")
            .Location("SummaryAdmin", "");
        }

        // NEXT STATION: Take a look at the Views/ColorField.cshtml shape to see how our field should display the given
        // color and then come back here.

        public override IDisplayResult Edit(ColorField field, BuildFieldEditorContext context)
        {
            // Nothing new here, the Initialize shape helper is being used to generate an editor shape.
            return Initialize<EditColorFieldViewModel>(GetEditorShapeType(context), model =>
            {
                model.Value = field.Value;
                model.ColorName = field.ColorName;
                model.Field = field;
                model.Part = context.ContentPart;
                model.PartFieldDefinition = context.PartFieldDefinition;
            });
        }

        // NEXT STATION: Settings/ColorFieldSettings

        public override async Task<IDisplayResult> UpdateAsync(ColorField field, IUpdateModel updater, UpdateFieldEditorContext context)
        {
            var viewModel = new EditColorFieldViewModel();

            // Using this overload of the model updater you can specifically say what properties need to be updated
            // only. This way you make sure no other properties will be bound to the view model.
            if (await updater.TryUpdateModelAsync(viewModel, Prefix, f => f.Value, f => f.ColorName))
            {
                // Use the settings are now let's use them for validation.
                var settings = context.PartFieldDefinition.Settings.ToObject<ColorFieldSettings>();
                if (settings.Required && string.IsNullOrWhiteSpace(field.Value))
                {
                    updater.ModelState.AddModelError(Prefix, T["A value is required for {0}.", context.PartFieldDefinition.DisplayName()]);
                }

                // Also some custom validation for our ColorField hex value. Could be done in the view model instead.
                if (!string.IsNullOrWhiteSpace(field.Value) &&
                    !Regex.IsMatch(viewModel.Value, "^#([A-Fa-f0-9]{8}|[A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$"))
                {
                    updater.ModelState.AddModelError(Prefix, T["The given color is invalid."]);
                }

                field.ColorName = viewModel.ColorName;
                field.Value = viewModel.Value;
            }

            return Edit(field, context);
        }
    }
}

// NEXT STATION: Indexing/ColorFieldIndexHandler.cs