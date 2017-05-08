using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Toz.Dotnet.Core.Services;
using Toz.Dotnet.Core.Interfaces;

namespace Toz.Dotnet
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddSingleton<IRestService, RestService>();
            services.AddSingleton<IFilesManagementService, FilesManagementService>();
            services.AddSingleton<IPetsManagementService, PetsManagementService>();
            services.AddSingleton<INewsManagementService, NewsManagementService>();
            services.AddSingleton<IUsersManagementService, UsersManagementService>();
            services.AddSingleton<IScheduleManagementService, ScheduleManagementService>();
            services.AddSingleton<IOrganizationManagementService, OrganizationManagementService>();
            services.AddSingleton<IBackendErrorsService, BackendErrorsService>();

            services.AddSession();
            services.AddMemoryCache();
            services.AddSingleton<IAuthService, AuthService>(); // TEMPORARY
            //services.AddSingleton<IOrganizationManagementService, OrganizationManagementService>();

            services.AddMvc()
                .AddViewLocalization(
                LanguageViewLocationExpanderFormat.Suffix,
                opts => { opts.ResourcesPath = "Resources"; })
                .AddDataAnnotationsLocalization();

            services.Configure<RequestLocalizationOptions>(
                opts =>
                {
                var supportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("pl"),
                    new CultureInfo("de"),
                };

                opts.DefaultRequestCulture = new RequestCulture(culture: "pl", uiCulture: "pl");
                opts.SupportedCultures = supportedCultures;
                opts.SupportedUICultures = supportedCultures;
            });

            var appSettings = Configuration.GetSection("AppSettings");

            services.Configure<Toz.Dotnet.Resources.Configuration.AppSettings>(appSettings);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseSession();
            app.UseStaticFiles();

            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
