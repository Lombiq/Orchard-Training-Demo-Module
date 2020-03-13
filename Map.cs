using Lombiq.TrainingDemo.Controllers;
using Lombiq.TrainingDemo.Drivers;
using Lombiq.TrainingDemo.Fields;
using Lombiq.TrainingDemo.Filters;
using Lombiq.TrainingDemo.Indexes;
using Lombiq.TrainingDemo.Indexing;
using Lombiq.TrainingDemo.Migrations;
using Lombiq.TrainingDemo.Models;
using Lombiq.TrainingDemo.Navigation;
using Lombiq.TrainingDemo.Permissions;
using Lombiq.TrainingDemo.Services;
using Lombiq.TrainingDemo.Settings;
using Lombiq.TrainingDemo.ViewModels;

/* 
 * In this file you'll find the index of the whole (or at least most of the) module's classes for easier navigation 
 * between topics. You can navigate directly to classes and their methods by clicking on their names (enclosed in a 
 * Factory<T>() ) and pressing F12.
 * 
 * This class is not doing anything and only serves as an easy to use table of contents.
 */
namespace Lombiq.TrainingDemo
{
    static class Map
    {
        private static T Factory<T>() => default;

        private static void Treasure()
        {
            // Manifest.cs: module manifest and dependencies

            // Static resources: styles and scripts
            // Declaration
            Factory<ResourceManifest>();

            // Usage: require/include
            // Views/PersonListDashboard


            // Basic controller demonstrating localization, Notifier, Logger, routing
            Factory<YourFirstOrchardCoreController>();


            // Display management, IDisplayManager
            Factory<DisplayManagementController>();

            // Display types, zones, placement
            // Views/Book.cshtml
            // Views/Book.Description.cshtml
            Factory<BookDisplayDriver>();


            // ContentPart, ContentField on ContentPart
            Factory<PersonPart>();

            // Displaying, editing and updating ContentPart
            // Views/PersonPart.cshtml
            // Views/PersonPart.Edit.cshtml
            // Views/PersonPart.Summary.cshtml built-in Summary display type
            // Views/PersonPart.SummaryAdmin.cshtml built-in SummaryAdmin display type
            Factory<PersonPartDisplayDriver>();

            // Validating ContentPart fields
            Factory<PersonPartViewModel>();

            // IndexProvider, indexing simple obj

            // IndexProvider, indexing simple object or ContentPart in records
            Factory<BookIndex>();
            Factory<PersonPartIndex>();

            // Content Type, ContentPart, ContentField, index record creation.
            Factory<PersonMigrations>();

            // ISession, IContentItemDisplayManager, IClock
            Factory<DatabaseStorageController>();
            Factory<PersonListController>();


            // ContentField
            Factory<ColorField>();

            // ContentFieldSettings
            Factory<ColorFieldSettings>();

            // Editing and updating ContentFieldSettings
            // Views/ColorFieldSettings.Edit.cshtml
            Factory<ColorFieldSettingsDriver>();

            // Displaying, editing and updating ContentField
            // Views/ColorField.cshtml
            // Views/ColorField.Edit.cshtml
            // Views/ColorField.Option.cshtml default editor option name
            // Views/ColorField-ColorPicker.Option.cshtml custom editor option name
            // Views/ColorField-ColorPicker.Edit.cshtml custom editor option editor
            Factory<ColorFieldDisplayDriver>();

            // ContentFieldIndexHandler, indexing ContentField using custom index provider (e.g. Lucene).
            Factory<ColorFieldIndexHandler>();


            // ResourceManifest, scripts, styles
            // Views/ColorField-ColorPicker.Edit.cshtml resource injection
            Factory<ResourceManifest>();


            // IAuthorizationService
            Factory<AuthorizationController>();

            // Permissions, PermissionProvider
            Factory<DemoSettingsPermissions>();
            Factory<PersonPermissions>();


            // Admin attribute
            Factory<AdminController>();

            // Menu, Admin menu, NavigationProvider
            Factory<DemoSettingsAdminMenu>();
            Factory<PersonsAdminMenu>();


            // SectionDisplayDriver, ISite, DisplayDriver for SiteSettings
            Factory<DemoSettingsDisplayDriver>();

            // SiteSettings, ISite, ISiteService
            Factory<SiteSettingsController>();


            // ResultFilter, IAsyncResultFilter
            Factory<ShapeInjectionFilter>();
            Factory<ResourceInjectionFilter>();

            // ILayoutAccessor, IShapeFactory, zones, ad-hoc shapes, shape injection
            Factory<ShapeInjectionFilter>();

            // IResourceManager, resource injection
            Factory<ResourceInjectionFilter>();


            // Memory Cache, Dynamic Cache
            // IMemoryCache, IDynamicCacheService, ITagCache
            Factory<DateTimeCachingService>();

            // ILocalClock
            // Views/Cache/Index.cshtml
            // Views/Cache/Shape.cshtml
            // Views/CachedShape.cshtml
            Factory<DateTimeCachingService>();


            // IMediaFileStore, custom file store
            Factory<FileManagementController>();

            // FileSystemStore, IFileStore
            Factory<CustomFileStore>();


            // BackgroundTask, BackgroundTaskSettings
            Factory<DemoBackgroundTask>();


            // Vue.js app, Vue.js component, Vue.js initialization
            // Views/VueJs/DemoApp.cshtml
            // Views/VueComponents/Demo.Component.cshtml

            // Resource compilation, asset magement, Gulp, Babel
            // Gulpfile.js
        }
    }
}