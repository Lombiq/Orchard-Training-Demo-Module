using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Environment.Extensions;
using OrchardHUN.TrainingDemo.Models;

namespace OrchardHUN.TrainingDemo.Handlers
{
    [OrchardFeature("OrchardHUN.TrainingDemo.Contents")]
    public class DemoSettingsPartHandler : ContentHandler
    {
        public DemoSettingsPartHandler()
        {
            // Everything is familiar here except this line. Here we attach our part to the Site content type through an
            // activating filter. The result is the same as if we've attached it from migrations, but this is a
            // lightweight way of doing it. Also since the Site content type shouldn't be edited by a user from the admin
            // UI there's no point in attaching our part in a persisted way to it.
            Filters.Add(new ActivatingFilter<DemoSettingsPart>("Site"));

            // NEXT STATION: we need a driver, so go to Drivers/DemoSettingsPartDriver!
        }
    }
}