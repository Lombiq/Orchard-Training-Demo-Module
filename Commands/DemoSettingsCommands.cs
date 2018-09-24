/*
 * This class will add some commands to the Orchard command line (http://docs.orchardproject.net/Documentation/Using-the-command-line-interface).
 * 
 * With the commands exposed here we'll be able to modify the site settings we developed with DemoSettingsPart.
 */

using Orchard.Commands;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.Settings;
using OrchardHUN.TrainingDemo.Models;

namespace OrchardHUN.TrainingDemo.Commands
{
    [OrchardFeature("OrchardHUN.TrainingDemo.Contents")]
    // Unless you want to deal with lower-level stuff, always derive your command handler from DefaultOrchardCommandHandler.
    public class DemoSettingsCommands : DefaultOrchardCommandHandler
    {
        private readonly ISiteService _siteService;


        public DemoSettingsCommands(ISiteService siteService) => _siteService = siteService;


        // Simple command, without arguments
        [CommandName("trainingdemo getsettings")]
        [CommandHelp("trainingdemo getsettings\r\n\t" + "Gets the demo site settings.")]
        public void GetSettings()
        {
            var message = _siteService.GetSiteSettings().As<DemoSettingsPart>().Message;
            // This is how you can display strings from the command line. Use T-strings here for easy localization.
            Context.Output.WriteLine(T("The following message is saved: {0}", message));
        }

        // Command with an argument. Just add arguments to your method.
        [CommandName("trainingdemo setsettings")]
        [CommandHelp("trainingdemo setsettings \"<Message>\"\r\n\t" + "Sets the demo site settings message.")]
        public void SetSettings(string newMessage)
        {
            _siteService.GetSiteSettings().As<DemoSettingsPart>().Message = newMessage;

            Context.Output.WriteLine(T("The following message was saved: {0}", newMessage));
        }

        // Go ahead, try out the commands from Orchard.exe! Watch as the saved message changes.

        // NEXT STATION: let's go to Controllers/ContentsAdminController! We'll deal with content items from code, now 
        // big time.
    }
}