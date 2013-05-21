/*
 * We'll now add some settings to the dashboard (see: http://docs.orchardproject.net/Documentation/Modifying-site-settings).
 * As pretty much everthing that counts, site settings are also content items in Orchard. More precisely, the site settings is a single
 * content item of type Site. By adding a content part to this Site content type we can extend site settings.
 * 
 * Site settings start off with a standard content part. These parts are under the Contents feature.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;
using Orchard.Environment.Extensions;

namespace OrchardHUN.TrainingDemo.Models
{
    [OrchardFeature("OrchardHUN.TrainingDemo.Contents")]
    public class DemoSettingsPart : ContentPart<DemoSettingsPartRecord>
    {
        // For demonstration purposes we'll only store this single value. But we alread know how to store data, right?
        public string Message
        {
            get { return Record.Message; }
            set { Record.Message = value; }
        }
    }


    [OrchardFeature("OrchardHUN.TrainingDemo.Contents")]
    public class DemoSettingsPartRecord : ContentPartRecord
    {
        public virtual string Message { get; set; }
    }

    // NEXT STATION: for the sake of clarity we have the migrations in a separate class for this record. Go to SettingsMigrations!
}