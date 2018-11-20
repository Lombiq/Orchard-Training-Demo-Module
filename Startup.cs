using System;
using Fluid;
using Lombiq.TrainingDemo.Drivers;
using Lombiq.TrainingDemo.Fields;
using Lombiq.TrainingDemo.Indexes;
using Lombiq.TrainingDemo.Indexing;
using Lombiq.TrainingDemo.Models;
using Lombiq.TrainingDemo.Settings;
using Lombiq.TrainingDemo.ViewModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.Data.Migration;
using OrchardCore.DisplayManagement;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.Indexing;
using OrchardCore.Modules;
using YesSql.Indexes;

namespace Lombiq.TrainingDemo
{
    public class Startup : StartupBase
    {
        static Startup()
        {
            // Registering both field types and shape types are necessary as they can 
            // be accessed from inner properties.

            TemplateContext.GlobalMemberAccessStrategy.Register<ColorField>();
            TemplateContext.GlobalMemberAccessStrategy.Register<DisplayColorFieldViewModel>();
        }


        public override void ConfigureServices(IServiceCollection services)
        {
            // Book
            services.AddScoped<IDisplayDriver<Book>, BookDisplayDriver>();
            services.AddScoped<IDisplayManager<Book>, DisplayManager<Book>>();

            // Person Part
            services.AddSingleton<ContentPart, PersonPart>();
            services.AddScoped<IDataMigration, Migrations>();
            services.AddScoped<IContentPartDisplayDriver, PersonPartDisplayDriver>();
            services.AddSingleton<IIndexProvider, PersonPartIndexProvider>();

            // Color Field
            services.AddSingleton<ContentField, ColorField>();
            services.AddScoped<IContentFieldDisplayDriver, ColorFieldDisplayDriver>();
            services.AddScoped<IContentPartFieldDefinitionDisplayDriver, ColorFieldSettingsDriver>();
            services.AddScoped<IContentFieldIndexHandler, ColorFieldIndexHandler>();
        }

        public override void Configure(IApplicationBuilder builder, IRouteBuilder routes, IServiceProvider serviceProvider)
        {
            routes.MapAreaRoute(
                name: "Home",
                areaName: "Lombiq.TrainingDemo",
                template: "Home/Index",
                defaults: new { controller = "Home", action = "Index" }
            );

            //routes.MapAreaRoute(
            //    name: "PersonList",
            //    areaName: "Lombiq.TrainingDemo",
            //    template: "Admin/PersonList",
            //    defaults: new { controller = "PersonListAdmin", action = "Index" }
            //);
        }
    }
}