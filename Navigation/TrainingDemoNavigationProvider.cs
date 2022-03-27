using Lombiq.HelpfulLibraries.OrchardCore.Navigation;
using Lombiq.TrainingDemo.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using OrchardCore.Mvc.Core.Utilities;
using OrchardCore.Navigation;

namespace Lombiq.TrainingDemo.Navigation;

// This provider creates "main" type navigation entries. These are not used by Orchard Core itself, but you can call
// it from custom code. You can retrieve them with INavigationManager.BuildMenuAsync("main", ActionContext). If you
// use the Lombiq.HelpfulExtensions.Widgets feature, it has a MenuWidget which can accept the MenuItem collection
// returned by this method call and display a Bootstrap "navbar" wherever you need a menu. Or if you use a theme
// which extends Lombiq.BaseTheme, then it already displays your "main" navigation entries as a MenuWidget on its
// own. Yes, this is basically an ad for our other projects. :)
public class TrainingDemoNavigationProvider : MainMenuNavigationProviderBase
{
    private readonly IHttpContextAccessor _hca;

    public TrainingDemoNavigationProvider(
        IHttpContextAccessor hca,
        IStringLocalizer<TrainingDemoNavigationProvider> stringLocalizer)
        : base(hca, stringLocalizer) =>
        _hca = hca;

    protected override void Build(NavigationBuilder builder)
    {
        var context = _hca.HttpContext;
        builder
            .Add(T["Helpful Libraries"], builder => builder
                .Add(T["Controllers"], sectionBuilder => sectionBuilder
                    .Add(T["Your First OrchardCore Controller"], itemBuilder => itemBuilder
                        .Add(T["Index"], subMenu => subMenu
                            .Action<YourFirstOrchardCoreController>(context, controller => controller.Index()))
                        .Add(T["Notify Me"], subMenu => subMenu
                            .ActionTask<YourFirstOrchardCoreController>(context, controller => controller.NotifyMe()))
                    )
                    .Add(T["Display Management"], itemBuilder => itemBuilder
                        .Add(T["Display Book"], subMenu => subMenu
                            .ActionTask<DisplayManagementController>(context, controller => controller.DisplayBook()))
                        .Add(T["Display Book Description"], subMenu => subMenu
                            .ActionTask<DisplayManagementController>(context, controller => controller.DisplayBookDescription()))
                    )
                    .Add(T["Database Storage"], itemBuilder => itemBuilder
                        .Add(T["Create Books"], subMenu => subMenu
                            .Action<DatabaseStorageController>(context, controller => controller.CreateBooks()))
                        .Add(T["J. K. Rowling Books"], subMenu => subMenu
                            .ActionTask<DatabaseStorageController>(context, controller => controller.JKRowlingBooks()))
                    )
                    .Add(T["Person List"], itemBuilder => itemBuilder
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
                    )
                    .Add(T["Authorization"], itemBuilder => itemBuilder
                        .Add(T["CanEditPerson"], subMenu => subMenu
                            .ActionTask<AuthorizationController>(context, controller => controller.CanEditPerson()))
                        .Add(T["CanManagePersons"], subMenu => subMenu
                            .ActionTask<AuthorizationController>(context, controller => controller.CanManagePersons()))
                    )
                    .Add(T["Admin"], itemBuilder => itemBuilder
                        .Add(T["Index"], subMenu => subMenu
                            .Action<AdminController>(context, controller => controller.Index()))
                        .Add(T["Person List (Newest)"], subMenu => subMenu
                            .ActionTask<AdminController>(context, controller => controller.PersonListNewest()))
                        .Add(T["Person List (Oldest)"], subMenu => subMenu
                            .ActionTask<AdminController>(context, controller => controller.PersonListOldest()))
                    )
                    .Add(T["Site Settings"], itemBuilder =>
                        Add<SiteSettingsController>(
                            itemBuilder,
                            (T["Site Name"], nameof(SiteSettingsController.SiteName)),
                            (T["Demo Settings"], nameof(SiteSettingsController.DemoSettings)))
                    )
                    .Add(T["File Management"], itemBuilder =>
                        Add<FileManagementController>(
                            itemBuilder,
                            (T["Create File in Media Folder"], nameof(FileManagementController.CreateFileInMediaFolder)),
                            (T["Read File from Media Folder"], nameof(FileManagementController.ReadFileFromMediaFolder)),
                            (T["Upload File to Media"], nameof(FileManagementController.UploadFileToMedia)),
                            (T["Create File in Custom Folder"], nameof(FileManagementController.CreateFileInCustomFolder)))
                    )
                    .Add(T["API (Not for front end.)"], _ => { })
                    .Add(T["Cross Tenant Services"], itemBuilder =>
                        Add<CrossTenantServicesController>(
                            itemBuilder,
                            (T["Index"], nameof(CrossTenantServicesController.Index)))
                    )
                )
            );
    }

    private static void Add<T>(NavigationItemBuilder builder, params (LocalizedString Label, string Action)[] actionNames)
    {
        var controllerName = typeof(T).ControllerName();

        foreach (var (label, action) in actionNames)
        {
            builder.Add(label, subMenu => subMenu.Action(action, controllerName, "Lombiq.TrainingDemo"));
        }
    }
}
