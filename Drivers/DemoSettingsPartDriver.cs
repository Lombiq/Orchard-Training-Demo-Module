/*
 * Really nothing special in this driver. BUT! If we'd like to have this editor under a different editor group than the 
 * default one (i.e. we'd like to display the editor not under Settings/General but Settings/Something else) then it 
 * changes which edito builds the shape!
 * So if you use editor groups (see editor groups samples here: http://orcharddojo.net/orchard-resources/Library/Examples/) 
 * make sure to employ the technique of building the shape from the second Editor() method, and checking the updater for 
 * null inside (as shown in the samples).
 *
 * NEXT STATION: after you've enjoyed looking head over to EditorTemplates/Parts.DemoSettings
 */

using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;
using OrchardHUN.TrainingDemo.Models;

namespace OrchardHUN.TrainingDemo.Drivers
{
    [OrchardFeature("OrchardHUN.TrainingDemo.Contents")]
    public class DemoSettingsPartDriver : ContentPartDriver<DemoSettingsPart>
    {
        protected override string Prefix => "OrchardHUN.TrainingDemo.DemoSettingsPart";


        protected override DriverResult Editor(DemoSettingsPart part, dynamic shapeHelper) =>
            // Site settings are sometimes conventionally suffixed with SiteSettings. Not surprisingly there's a
            // corresponding entry in Placement.info too.
            ContentShape("Parts_DemoSettings_SiteSettings",
                () => shapeHelper.EditorTemplate(
                    TemplateName: "Parts.DemoSettings.SiteSettings",
                    Model: part,
                    Prefix: Prefix));

        protected override DriverResult Editor(DemoSettingsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater?.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }

        // Note that we don't care about import/export. This is because site settings (more precisely: public
        // readable/writable properties of site settings parts that are of type string, bool or int; but see:
        // https://github.com/OrchardCMS/Orchard/issues/4974) are automatically exported/imported.
    }
}