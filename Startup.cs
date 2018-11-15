using System;
using Lombiq.TrainingDemo.Drivers;
using Lombiq.TrainingDemo.Indexes;
using Lombiq.TrainingDemo.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.Data.Migration;
using OrchardCore.DisplayManagement;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.Modules;
using YesSql.Indexes;

namespace Lombiq.TrainingDemo
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDisplayDriver<Book>, BookDisplayDriver>();
            services.AddScoped<IDisplayManager<Book>, DisplayManager<Book>>();

            services.AddSingleton<ContentPart, PersonPart>();
            services.AddScoped<IDataMigration, Migrations>();
            services.AddScoped<IContentPartDisplayDriver, PersonPartDisplayDriver>();
            services.AddSingleton<IIndexProvider, PersonPartIndexProvider>();
        }

        public override void Configure(IApplicationBuilder builder, IRouteBuilder routes, IServiceProvider serviceProvider)
        {
            routes.MapAreaRoute(
                name: "Home",
                areaName: "Lombiq.TrainingDemo",
                template: "Home/Index",
                defaults: new { controller = "Home", action = "Index" }
            );

            routes.MapAreaRoute(
                name: "PersonList",
                areaName: "Lombiq.TrainingDemo",
                template: "Admin/PersonList",
                defaults: new { controller = "PersonListAdmin", action = "Index" }
            );
        }
    }
}