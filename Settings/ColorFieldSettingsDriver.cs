using System.Threading.Tasks;
using Lombiq.TrainingDemo.Fields;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.DisplayManagement.Views;

namespace Lombiq.TrainingDemo.Settings
{
    public class ColorFieldSettingsDriver : ContentPartFieldDefinitionDisplayDriver<ColorField>
    {
        public override IDisplayResult Edit(ContentPartFieldDefinition partFieldDefinition)
        {
            return Initialize<ColorFieldSettings>("ColorFieldSettings_Edit", model => partFieldDefinition.Settings.Populate(model))
                .Location("Content");
        }

        public override async Task<IDisplayResult> UpdateAsync(ContentPartFieldDefinition partFieldDefinition, UpdatePartFieldEditorContext context)
        {
            var model = new ColorFieldSettings();

            await context.Updater.TryUpdateModelAsync(model, Prefix);

            context.Builder.MergeSettings(model);

            return Edit(partFieldDefinition);
        }
    }
}