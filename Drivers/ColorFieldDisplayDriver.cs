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
    public class ColorFieldDisplayDriver : ContentFieldDisplayDriver<ColorField>
    {
        public IStringLocalizer T { get; set; }


        public ColorFieldDisplayDriver(IStringLocalizer<ColorFieldDisplayDriver> localizer)
        {
            T = localizer;
        }


        public override IDisplayResult Display(ColorField field, BuildFieldDisplayContext context)
        {
            return Initialize<DisplayColorFieldViewModel>(GetDisplayShapeType(context), model =>
            {
                model.Field = field;
                model.Part = context.ContentPart;
                model.PartFieldDefinition = context.PartFieldDefinition;
            })
            .Location("Content")
            .Location("SummaryAdmin", "");
        }

        public override IDisplayResult Edit(ColorField field, BuildFieldEditorContext context)
        {
            return Initialize<EditColorFieldViewModel>(GetEditorShapeType(context), model =>
            {
                model.Value = field.Value;
                model.ColorName = field.ColorName;
                model.Field = field;
                model.Part = context.ContentPart;
                model.PartFieldDefinition = context.PartFieldDefinition;
            });
        }

        public override async Task<IDisplayResult> UpdateAsync(ColorField field, IUpdateModel updater, UpdateFieldEditorContext context)
        {
            var viewModel = new EditColorFieldViewModel();

            if (await updater.TryUpdateModelAsync(viewModel, Prefix, f => f.Value, f => f.ColorName))
            {
                var settings = context.PartFieldDefinition.Settings.ToObject<ColorFieldSettings>();
                if (settings.Required && string.IsNullOrWhiteSpace(field.Value))
                {
                    updater.ModelState.AddModelError(Prefix, T["A value is required for {0}.", context.PartFieldDefinition.DisplayName()]);
                }

                if (!string.IsNullOrWhiteSpace(field.Value) &&
                    !Regex.IsMatch(viewModel.Value, "^#(?:[0-9a-fA-F]{3}){1,2}$"))
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