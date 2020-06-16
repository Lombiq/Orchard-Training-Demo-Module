/*
 * A Startup class (there can be multiple ones in a module under different namespaces) will be called by the framework.
 * It's the same as the ASP.NET Startup class (https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup). In
 * there you can e.g. register injected services and change the request pipeline.
 */

using Fluid;
using Lombiq.TrainingDemo.Drivers;
using Lombiq.TrainingDemo.Fields;
using Lombiq.TrainingDemo.Filters;
using Lombiq.TrainingDemo.Handlers;
using Lombiq.TrainingDemo.Indexes;
using Lombiq.TrainingDemo.Indexing;
using Lombiq.TrainingDemo.Middlewares;
using Lombiq.TrainingDemo.Migrations;
using Lombiq.TrainingDemo.Models;
using Lombiq.TrainingDemo.Navigation;
using Lombiq.TrainingDemo.Permissions;
using Lombiq.TrainingDemo.Services;
using Lombiq.TrainingDemo.Settings;
using Lombiq.TrainingDemo.ViewModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrchardCore.BackgroundTasks;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.Data.Migration;
using OrchardCore.DisplayManagement;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.Environment.Shell;
using OrchardCore.Environment.Shell.Configuration;
using OrchardCore.Indexing;
using OrchardCore.Navigation;
using OrchardCore.ResourceManagement;
using OrchardCore.Security.Permissions;
using OrchardCore.Settings;
using System;
using System.IO;
using YesSql.Indexes;

namespace Lombiq.TrainingDemo
{
    // Note that the Startup class is a conventionally named class with conventionally named methods. This way it'll be
    // hooked up to the rest of the app automatically. Magic!
    public class Startup
    {
        private readonly IShellConfiguration _shellConfiguration;


        public Startup(IShellConfiguration shellConfiguration)
        {
            _shellConfiguration = shellConfiguration;
        }


        static Startup()
        {
            // To be able to access these view models in display shapes rendered by the Liquid markup engine you need
            // to register them. To learn more about Liquid in Orchard Core see this documentation:
            // https://docs.orchardcore.net/en/dev/docs/reference/modules/Liquid/
            TemplateContext.GlobalMemberAccessStrategy.Register<PersonPartViewModel>();
            TemplateContext.GlobalMemberAccessStrategy.Register<ColorField>();
            TemplateContext.GlobalMemberAccessStrategy.Register<DisplayColorFieldViewModel>();

            // NEXT STATION: Views/PersonPart.Edit.cshtml
        }


        public void ConfigureServices(IServiceCollection services)
        {
            // Book
            services.AddScoped<IDisplayDriver<Book>, BookDisplayDriver>();
            services.AddScoped<IDisplayManager<Book>, DisplayManager<Book>>();
            services.AddScoped<IDataMigration, BookMigrations>();
            services.AddSingleton<IIndexProvider, BookIndexProvider>();

            // Person Part
            services.AddContentPart<PersonPart>()
                .UseDisplayDriver<PersonPartDisplayDriver>()
                .AddHandler<PersonPartHandler>();
            services.AddScoped<IDataMigration, PersonMigrations>();
            services.AddSingleton<IIndexProvider, PersonPartIndexProvider>();

            // Color Field
            services.AddContentField<ColorField>();
            services.AddScoped<IContentFieldDisplayDriver, ColorFieldDisplayDriver>();
            services.AddScoped<IContentPartFieldDefinitionDisplayDriver, ColorFieldSettingsDriver>();
            services.AddScoped<IContentFieldIndexHandler, ColorFieldIndexHandler>();

            // Resources
            services.AddScoped<IResourceManifestProvider, ResourceManifest>();

            // Permissions
            services.AddScoped<IPermissionProvider, PersonPermissions>();

            // Admin Menu
            services.AddScoped<INavigationProvider, PersonsAdminMenu>();

            // Demo Settings
            services.Configure<DemoSettings>(_shellConfiguration.GetSection("Lombiq_TrainingDemo"));
            services.AddTransient<IConfigureOptions<DemoSettings>, DemoSettingsConfiguration>();
            services.AddScoped<IDisplayDriver<ISite>, DemoSettingsDisplayDriver>();
            services.AddScoped<IPermissionProvider, DemoSettingsPermissions>();
            services.AddScoped<INavigationProvider, DemoSettingsAdminMenu>();


            // Filters
            services.Configure<MvcOptions>((options) =>
            {
                options.Filters.Add(typeof(ShapeInjectionFilter));
                options.Filters.Add(typeof(ResourceInjectionFilter));
            });

            // File System
            services.AddSingleton<ICustomFileStore>(serviceProvider =>
            {
                // So our goal here is to have a custom folder in the tenant's own folder. The Media folder is also
                // there but we won't use that. To get tenant-specific data we need to use the ShellOptions and
                // ShellShettings objects.
                var shellOptions = serviceProvider.GetRequiredService<IOptions<ShellOptions>>().Value;
                var shellSettings = serviceProvider.GetRequiredService<ShellSettings>();

                var tenantFolderPath = PathExtensions.Combine(
                    // This is the absolute path of the "App_Data" folder.
                    shellOptions.ShellsApplicationDataPath,
                    // This is the folder which contains the tenants which is Sites by default.
                    shellOptions.ShellsContainerName,
                    // This is the tenant name. We want our custom folder inside this folder.
                    shellSettings.Name);

                // And finally our full base path.
                var customFolderPath = PathExtensions.Combine(tenantFolderPath, "CustomFiles");

                // Now register our CustomFileStore instance with the path given.
                return new CustomFileStore(customFolderPath);

                // NEXT STATION: Controllers/FileManagementController and find the CreateFileInCustomFolder method.
            });

            // Caching
            services.AddScoped<IDateTimeCachingService, DateTimeCachingService>();

            // Background tasks. Note that these have to be singletons.
            services.AddSingleton<IBackgroundTask, DemoBackgroundTask>();
        }

        public void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            // You can put service configuration here as you would do it in other ASP.NET Core applications. If you
            // don't need it you can skip overriding it. However, here we need it for our middleware.

            builder.UseMiddleware<RequestLoggingMiddleware>();
        }
    }
}