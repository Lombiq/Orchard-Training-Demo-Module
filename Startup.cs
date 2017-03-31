using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Modules;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Orchard.BackgroundTasks;
using Orchard.ContentManagement.Display.ContentDisplay;
using Orchard.Data.Migration;
using Orchard.DisplayManagement.Descriptors;
using Orchard.Environment.Commands;
using Orchard.Environment.Navigation;
using OrchardHUN.DisplayManagement.Descriptors;
using OrchardHUN.TrainingDemo.Commands;
using OrchardHUN.TrainingDemo.ContentElementDisplays;
using OrchardHUN.TrainingDemo.Migrations;
using OrchardHUN.TrainingDemo.Models;
using OrchardHUN.TrainingDemo.Services;
using System;
using YesSql.Core.Indexes;

namespace OrchardHUN.TrainingDemo
{
    public class Startup : StartupBase
    {
        public override void Configure(IApplicationBuilder builder, IRouteBuilder routes, IServiceProvider serviceProvider)
        {
            //routes.MapAreaRoute(
            //    name: "YourFirstRoute",
            //    areaName: "OrchardHUN.TrainingDemo",
            //    template: "YourFirstOrchard/Index",
            //    defaults: new { controller = "YourFirstOrchard", action = "Index" }
            //);

            routes.MapAreaRoute(
                name: "NotifyMeRoute",
                areaName: nameof(TrainingDemo),
                template: $"{nameof(Controllers.DependencyInjectionController)}/{nameof(Controllers.DependencyInjectionController.NotifyMe)}",
                defaults: new { controller = nameof(Controllers.DependencyInjectionController), action = nameof(Controllers.DependencyInjectionController.NotifyMe) }
            );

            routes.MapAreaRoute(
                name: "ListPersons",
                areaName: nameof(TrainingDemo),
                template: $"{nameof(Controllers.PersonController)}/{nameof(Controllers.PersonController.Index)}",
                defaults: new { controller = nameof(Controllers.PersonController), action = nameof(Controllers.PersonController.Index) }
            );


            //builder.UseMiddleware<NonBlockingMiddleware>();
            //builder.UseMiddleware<BlockingMiddleware>();
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            //services.AddScoped<ITestDependency, ClassFoo>();
            //services.AddScoped<ICommandHandler, DemoCommands>();
            //services.AddSingleton<IBackgroundTask, TestBackgroundTask>();
            //services.AddScoped<IShapeTableProvider, DemoShapeProvider>();
            //services.AddShapeAttributes<DemoShapeProvider>();
            //services.AddScoped<INavigationProvider, AdminMenu>();
            //services.AddScoped<IContentDisplayDriver, TestContentElementDisplay>();
            services.AddScoped<IDataMigration, PersonMigration>();
            services.AddScoped<IDataMigration, AddressMigration>();
            services.AddScoped<IIndexProvider, PersonPartIndexProvider>();
            services.AddScoped<IPersonManager, PersonManager>();
        }
    }
}