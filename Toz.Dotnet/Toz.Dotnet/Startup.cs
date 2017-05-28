using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Toz.Dotnet.Core.Services;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Models.Validation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Toz.Dotnet.Authorization;
using Toz.Dotnet.Resources.Configuration;
using Microsoft.AspNetCore.DataProtection;
using System.IO;

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
            services.TryAddSingleton<IRestService, RestService>();
            services.AddSingleton<IFilesManagementService, FilesManagementService>();
            services.AddSingleton<IPetsManagementService, PetsManagementService>();
            services.AddSingleton<INewsManagementService, NewsManagementService>();
            services.AddSingleton<IUsersManagementService, UsersManagementService>();
            services.AddSingleton<IScheduleManagementService, ScheduleManagementService>();
            services.AddSingleton<IOrganizationManagementService, OrganizationManagementService>();
            services.AddSingleton<IValidationAttributeAdapterProvider, CustomValidationAttributeAdapterProvider>();
            services.AddSingleton<IBackendErrorsService, BackendErrorsService>();
            services.AddSingleton<IProposalsManagementService, ProposalsManagementService>();
            services.AddSingleton<IHowToHelpInformationService, HowToHelpInformationService>();
            services.TryAddSingleton<IAuthService, AuthService>();
            services.AddSingleton<IAccountManagementService, AccountManagementService>();
            services.AddSingleton<IHelpersManagementService, HelpersManagementService>();
            services.AddSingleton<ICommentsManagementService, CommentsManagementService>();

            services.AddSession();
            services.AddMemoryCache();

            services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo("Keys"));
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("TOZ", "SA"));
            });

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

            services.Configure<AppSettings>(appSettings);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IDataProtectionProvider provider)
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

            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationScheme = CookieAuthenticationDefaults.AuthenticationScheme,
                CookieName = CookieAuthenticationDefaults.CookiePrefix + CookieAuthenticationDefaults.AuthenticationScheme,
                LoginPath = new PathString("/Account/SignIn"),
                AccessDeniedPath = new PathString("/Account/SignIn"),
                LogoutPath = new PathString("/Account/SignOut"),
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                DataProtectionProvider = provider.CreateProtector("CustomDataProtector")
            });

            app.UseCustomAuthorization();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
