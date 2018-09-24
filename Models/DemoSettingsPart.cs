/*
 * We'll now add some settings to the dashboard (see: http://docs.orchardproject.net/Documentation/Modifying-site-settings).
 * As pretty much everthing that counts, site settings are also content items in Orchard. More precisely, the site 
 * settings is a single content item of type Site. By adding a content part to this Site content type we can extend site 
 * settings.
 * 
 * Site settings start off with a standard content part. These parts are under the Contents feature.
 */

using Orchard.ContentManagement;
using Orchard.Environment.Extensions;

namespace OrchardHUN.TrainingDemo.Models
{
    [OrchardFeature("OrchardHUN.TrainingDemo.Contents")]
    public class DemoSettingsPart : ContentPart
    {
        // For demonstration purposes we'll only store this single value. Notice that here we only use the infoset (what
        // we also used in PersonListPart). Since this is a site settings part nobody would want to use Message for
        // filtering or ordering in a query: thus we don't need a record for storing it, it will be only saved in the
        // infoset.
        public string Message
        {
            get => this.Retrieve(x => x.Message);
            set => this.Store(x => x.Message, value);
        }
    }

    // NEXT STATION: Handlers/DemoSettingsPartHandler!
}