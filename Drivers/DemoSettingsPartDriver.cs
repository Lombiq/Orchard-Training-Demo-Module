/*
 * Really nothing special in this driver.
 * 
 * NEXT STATION: after you've enjoyed looking head over to EditorTemplates/Parts.DemoSettings
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using Orchard.Environment.Extensions;
using OrchardHUN.TrainingDemo.Models;

namespace OrchardHUN.TrainingDemo.Drivers
{
    [OrchardFeature("OrchardHUN.TrainingDemo.Contents")]
    public class DemoSettingsPartDriver : ContentPartDriver<DemoSettingsPart>
    {
        protected override string Prefix
        {
            get { return "OrchardHUN.TrainingDemo.DemoSettingsPart"; }
        }


        protected override DriverResult Editor(DemoSettingsPart part, dynamic shapeHelper)
        {
            // Site settings are sometimes conventionally suffixed with SiteSettings.
            // Not surprisingly there's a corresponding entry in Placement.info too.
            return ContentShape("Parts_DemoSettings_SiteSettings",
                () => shapeHelper.EditorTemplate(
                    TemplateName: "Parts.DemoSettings.SiteSettings",
                    Model: part,
                    Prefix: Prefix));
        }

        protected override DriverResult Editor(DemoSettingsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }
    }
}