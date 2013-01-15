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
            // 1. StartHere.txt - introduction and some basic information

            // 2. Module.txt

            /* 3. See the controller itself, its sole Index action and
             * Views/YourFirstOrchard/Index.cshtml
             * Themed, Controller, Action, Razor
             */
            _temp = typeof(YourFirstOrchardController);

            /* 4.
             * Dependency Injection, Localizer, Notifier, Logger
             */
            _temp = typeof(DependencyInjectionController);

            /* 5. 
             * ORM, NHibernate, Record
             */
            _temp = typeof(PersonRecord);

            /* 6. 
             * Migrations, UpdateFromX
             */
            _temp = typeof(Migrations);

            /* 7. 
             * Dependency Injection, IRepository
             */
            _temp = typeof(PersonManager);

            /* 8. 
             * Fatal exception
             */
            _temp = typeof(PersonController);

            /* 9. 
             * OrchardFeature attribute, ContentPart, ContentPartRecord, Lazy<T>
             */
            _temp = typeof(PersonListPart);

            /* 10.
             * Widget
             */
            _temp = typeof(ContentsMigrations);

            /* 11.
             * Content Handler, Work<T>, On* events, Storage Filter
             */
            _temp = typeof(PersonListPartHandler);

            /* 12.
             * Prefix, Shape, ContentShape, Editor, Display, Import, Export
             */
            _temp = typeof(PersonListPartDriver);

            // 13. Placement.info

            // 14. Views/EditorTemplates/Parts.PersonList.cshtml

            // 15. Views/Parts.PersonList.cshtml

            /* 16.
             * OrchardServices, Authorizer, ContentManager, WorkContext, Shape
             */
            _temp = typeof(ContentsAdminController);

                /* 16.1 */
                _temp = typeof(Routes);

                /* 16.2 */
                _temp = typeof(Permissions);

                /* 16.3 */
                _temp = typeof(AdminMenu);

                // 16.4 Views/PersonListDashboard: Style/Script Require/Include

                    /* 16.4.1 */
                    _temp = typeof(ResourceManifest);

                    // 16.4.2 Views/ComplimentaryEncouragement.cshtml

                // 16.5 Views/LatestPersonLists.cshtml

            /* 17. 
             * FilterProvider, AdminFilter, ResourceManager
             */
            _temp = typeof(ResourceFilter);

            /* 18.
             * ICacheManager, ICacheService, IClock, ISignals
             */
            _temp = typeof(IDateTimeCachingService);

            /* 19. */
            _temp = typeof(CacheController);

        }
    }
}