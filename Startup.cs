using System;
using Fluid;
using Lombiq.TrainingDemo.Drivers;
using Lombiq.TrainingDemo.Fields;
using Lombiq.TrainingDemo.Indexes;
using Lombiq.TrainingDemo.Indexing;
using Lombiq.TrainingDemo.Migrations;
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
using OrchardCore.Navigation;
using OrchardCore.ResourceManagement;
using YesSql.Indexes;

namespace Lombiq.TrainingDemo
{
    public class Startup : StartupBase
    {
        static Startup()
        {
            // To be able to access these view models in display shapes rendered by the Liquid markup engine you need
            // to register them. To learn more about Liquid in Orchard Core see this documentation:
            // https://orchardcore.readthedocs.io/en/latest/OrchardCore.Modules/OrchardCore.Liquid/README/
            TemplateContext.GlobalMemberAccessStrategy.Register<PersonPartViewModel>();
            TemplateContext.GlobalMemberAccessStrategy.Register<ColorField>();
            TemplateContext.GlobalMemberAccessStrategy.Register<DisplayColorFieldViewModel>();

            // NEXT STATION: Views/PersonPart.Edit.cshtml
        }


        public override void ConfigureServices(IServiceCollection services)
        {
            // Book
            services.AddScoped<IDisplayDriver<Book>, BookDisplayDriver>();
            services.AddScoped<IDisplayManager<Book>, DisplayManager<Book>>();
            services.AddScoped<IDataMigration, BookMigrations>();
            services.AddSingleton<IIndexProvider, BookIndexProvider>();

            // Person Part
            services.AddSingleton<ContentPart, PersonPart>();
            services.AddScoped<IDataMigration, PersonMigrations>();
            services.AddScoped<IContentPartDisplayDriver, PersonPartDisplayDriver>();
            services.AddSingleton<IIndexProvider, PersonPartIndexProvider>();

            // Color Field
            services.AddSingleton<ContentField, ColorField>();
            services.AddScoped<IContentFieldDisplayDriver, ColorFieldDisplayDriver>();
            services.AddScoped<IContentPartFieldDefinitionDisplayDriver, ColorFieldSettingsDriver>();
            services.AddScoped<IContentFieldIndexHandler, ColorFieldIndexHandler>();

            // Resources
            services.AddScoped<IResourceManifestProvider, ResourceManifest>();

            // Admin menu
            services.AddScoped<INavigationProvider, AdminMenu>();
        }

        public override void Configure(IApplicationBuilder builder, IRouteBuilder routes, IServiceProvider serviceProvider)
        {
            // You can put service configuration here as you would do it in other ASP.NET Core applications. If you
            // don't need it you can skip overriding it.
        }
    }
}