using Lombiq.HelpfulLibraries.OrchardCore.Navigation;
using Lombiq.TrainingDemo.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using OrchardCore.Mvc.Core.Utilities;
using OrchardCore.Navigation;
using System;

namespace Lombiq.TrainingDemo.Navigation;

// This provider creates "main" type navigation entries. These are not used by Orchard Core itself, but you can call
// it from custom code. You can retrieve them with INavigationManager.BuildMenuAsync("main", ActionContext). If you
// use the Lombiq.HelpfulExtensions.Widgets feature, it has a MenuWidget which can accept the MenuItem collection
// returned by this method call and display a Bootstrap "navbar" wherever you need a menu. Or if you use a theme
// which extends Lombiq.BaseTheme, then it already displays your "main" navigation entries as a MenuWidget on its
// own. Yes, this is basically an ad for our other projects. :)
//
// For details on how to use them, see the Lombiq.BaseTheme.Samples project:
// https://github.com/Lombiq/Orchard-Base-Theme/tree/issue/OSOE-62/Lombiq.BaseTheme.Samples
public class TrainingDemoNavigationProvider : MainMenuNavigationProviderBase
{
    public TrainingDemoNavigationProvider(
        IHttpContextAccessor hca,
        IStringLocalizer<TrainingDemoNavigationProvider> stringLocalizer)
        : base(hca, stringLocalizer)
    {
    }

    protected override void Build(NavigationBuilder builder)
    {
        var context = _hca.HttpContext;
        builder
            .Add(T["Training Demo"], builder => builder
                .Add(T["Your First OrchardCore Controller"], Header)
                .Add(T["Index"], subMenu => subMenu
                    .Action<YourFirstOrchardCoreController>(context, controller => controller.Index()))
                .Add(T["Notify Me"], subMenu => subMenu
                    .ActionTask<YourFirstOrchardCoreController>(context, controller => controller.NotifyMe()))
                .AddSeparator(T)
                .Add(T["Display Management"], Header)
                .Add(T["Display Book"], subMenu => subMenu
                    .ActionTask<DisplayManagementController>(context, controller => controller.DisplayBook()))
                .Add(T["Display Book Description"], subMenu => subMenu
                    .ActionTask<DisplayManagementController>(context, controller => controller.DisplayBookDescription()))
                .AddSeparator(T)
                .Add(T["Database Storage"], Header)
                .Add(T["Create Books"], subMenu => subMenu
                    .Action<DatabaseStorageController>(context, controller => controller.CreateBooks()))
                .Add(T["J. K. Rowling Books"], subMenu => subMenu
                    .ActionTask<DatabaseStorageController>(context, controller => controller.JKRowlingBooks()))
                .AddSeparator(T)
                .Add(T["Person List"], Header)
                .Add(T["Older Than 30"], subMenu => subMenu
                    .ActionTask<PersonListController>(context, controller => controller.OlderThan30()))
                .Add(T["Fountain of Eternal Youth"], subMenu => subMenu
                    .Action(
                        nameof(PersonListController.FountainOfEternalYouth),
                        typeof(PersonListController).ControllerName(),
                        "Lombiq.TrainingDemo"))
                .Add(T["Create an Android"], subMenu => subMenu
                    .Action(
                        nameof(PersonListController.CreateAnAndroid),
                        typeof(PersonListController).ControllerName(),
                        "Lombiq.TrainingDemo"))
                .AddSeparator(T)
                .Add(T["Authorization"], Header)
                .Add(T["CanEditPerson"], subMenu => subMenu
                    .ActionTask<AuthorizationController>(context, controller => controller.CanEditPerson()))
                .Add(T["CanManagePersons"], subMenu => subMenu
                    .ActionTask<AuthorizationController>(context, controller => controller.CanManagePersons()))
                .AddSeparator(T)
                .Add(T["Admin"], Header)
                .Add(T["Index"], subMenu => subMenu
                    .Action<AdminController>(context, controller => controller.Index()))
                .Add(T["Person List (Newest)"], subMenu => subMenu
                    .ActionTask<AdminController>(context, controller => controller.PersonListNewest()))
                .Add(T["Person List (Oldest)"], subMenu => subMenu
                    .ActionTask<AdminController>(context, controller => controller.PersonListOldest()))
                .AddSeparator(T)
                .Add(T["Site Settings"], Header)
                .Add(T["Site Name"], Action<SiteSettingsController>(nameof(SiteSettingsController.SiteName)))
                .Add(T["Demo Settings"], Action<SiteSettingsController>(nameof(SiteSettingsController.DemoSettings)))
                .AddSeparator(T)
                .Add(T["File Management"], Header)
                .Add(T["Create File in Media Folder"], Action<FileManagementController>(nameof(FileManagementController.CreateFileInMediaFolder)))
                .Add(T["Read File from Media Folder"], Action<FileManagementController>(nameof(FileManagementController.ReadFileFromMediaFolder)))
                .Add(T["Upload File to Media"], Action<FileManagementController>(nameof(FileManagementController.UploadFileToMedia)))
                .Add(T["Create File in Custom Folder"], Action<FileManagementController>(nameof(FileManagementController.CreateFileInCustomFolder)))
                .AddSeparator(T)
                .Add(T["API (Not for front end.)"], Header)
                .AddSeparator(T)
                .Add(T["Cross Tenant Services"], Action<CrossTenantServicesController>(nameof(CrossTenantServicesController.Index))));
    }

    private void Header(NavigationItemBuilder subMenu) => subMenu.Url("#").AddClass("disabled menuWidget__link_title");

    private static Action<NavigationItemBuilder> Action<T>(string actionName) =>
        subMenu => subMenu.Action(actionName, typeof(T).ControllerName(), "Lombiq.TrainingDemo");
}

// END OF TRAINING SECTION: Navigation menus

// NEXT STATION: Controllers/SiteSettingsController.cs
