using OrchardHUN.TrainingDemo.Controllers;
using OrchardHUN.TrainingDemo.Drivers;
using OrchardHUN.TrainingDemo.Filters;
using OrchardHUN.TrainingDemo.Handlers;
using OrchardHUN.TrainingDemo.Models;
using OrchardHUN.TrainingDemo.Services;

/* In this file, you'll find the index of the whole (or at least most of the)
 * module's classes for easier navigation between topics.
 * You can navigate directly to classes and interfaces by clicking on their
 * names (enclosed in a typeof() ) and pressing F12.
 */
namespace OrchardHUN.TrainingDemo
{
    static class Map
    {
        private static dynamic _temp;

        private static void Treasure()
        {
            // Module.txt: features and dependencies


            // Static resources: styles and scripts
                // Declaration
                _temp = typeof(ResourceManifest);

                // Usage: require/include
                // Views/PersonListDashboard


            // [Themed]: Integrating with the current theme
            _temp = typeof(YourFirstOrchardController);


            // Dependency injection and basic services
                // Dependency injection and types of dependencies, Logger, Localizer, Notifier
                _temp = typeof(DependencyInjectionController);

                // WorkContext and OrchardServices
                // Views/PersonListDashboard.cshtml
                _temp = typeof(ContentsAdminController);

                // Ways of injections
                    // Single
                    _temp = typeof(DependencyInjectionController);

                    // IEnumberable<T>
                    _temp = typeof(PersonManager);

                    // Lazy<T>
                    _temp = typeof(PersonListPart);

                    // Work<T>
                    _temp = typeof(PersonListPartHandler);


            // Data storage
                // Record
                _temp = typeof(PersonRecord);

                // Repository
                _temp = typeof(PersonManager);

                // Record migrations
                _temp = typeof(Migrations);

                // ContentManager
                _temp = typeof(ContentsAdminController);

                // Abstracted file storage with IStorageProvider
                _temp = typeof(FileManagementController);


            // Exception handling
                // OrchardException
                _temp = typeof(FileManagementController);

                // IsFatal()
                _temp = typeof(PersonController);


            // Ad-hoc shape creation: Views/PersonListDashboard.cshtml


            // OrchardFeature attribute
            _temp = typeof(PersonListPart);


            // ContentPart development
                // ContentPart, ContentPartRecord
                _temp = typeof(PersonListPart);

                // ContentType migrations
                _temp = typeof(ContentsMigrations);

                // Drivers: shapes, display and edit methods, export/import
                _temp = typeof(ContentsAdminController);
                _temp = typeof(PersonListPartDriver);

                // Handlers and filters
                _temp = typeof(PersonListPartHandler);

                // Placement.info


            // Custom routes
            _temp = typeof(Routes);


            // Navigation providers: implementing an admin menu (with corresponding admin controller)
            _temp = typeof(AdminMenu);
            _temp = typeof(ContentsAdminController);


            // Filters: result and action filters, FilterProvider
            _temp = typeof(ResourceFilter);


            // Event bus and event handlers
            _temp = typeof(BackgroundTask);
            _temp = typeof(ScheduledTask);
            _temp = typeof(DynamicEventHandler);


            // Permissions and authorization
            _temp = typeof(Permissions);
            _temp = typeof(ContentsAdminController);


            // Background and scheduled tasks
            _temp = typeof(BackgroundTask);
            _temp = typeof(ScheduledTask);


            // Caching: ICacheManager and ICacheService, ISignals
            _temp = typeof(IDateTimeCachingService);
            _temp = typeof(CacheController);


            // Unit tests
                // IClock
                _temp = typeof(IDateTimeCachingService);

                /* Check out the OrchardHUN.TrainingDemo.Tests folder in the project folder of this module for a complete
                 * unit test, starting with StartHere.txt
                 */
        }
    }
}