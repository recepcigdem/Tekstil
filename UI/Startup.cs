using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Business.Concrete;
using Core.DependencyResolvers;
using Core.Extensions;
using Core.Utilities.IoC;
using Core.Utilities.Security.Encryption;
using Core.Utilities.Security.JWT;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using UI.Utils;

namespace UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDependencyResolvers(new ICoreModule[]
            {
                new CoreModule(),
            });
            //services.AddSingleton<IAgeGroupService, AgeGroupManager>();
            //services.AddSingleton<IAgeGroupDal, EfAgeGroupDal>();
            services.AddControllersWithViews();
            services.AddSingleton<LocalizationService>();

            services.AddLocalization(o =>
            {
                // We will put our translations in a folder called Resources
                o.ResourcesPath = "Resources";
            });

            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            
            services.AddCors();
            services.AddControllersWithViews().AddViewLocalization();

            services.AddDistributedMemoryCache(); // Adds a default in-memory implementation of IDistributedCache
            services.AddSession();

           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //app.ConfigureCustomExceptionMiddleware();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            IList<CultureInfo> supportedCultures = new List<CultureInfo>
            {
                new CultureInfo("tr-TR"),
                new CultureInfo("en-US")
            };

            var localizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            };

            localizationOptions.RequestCultureProviders.Clear();
            localizationOptions.RequestCultureProviders.Add(new CustomCultureProvider());
            app.UseRequestLocalization(localizationOptions);

            app.UseRouting();
            app.UseAuthentication();

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{culture=en}/{controller=Login}/{action=Index}");

                endpoints.MapControllerRoute(
                    name: "woLanguage",
                    pattern: "{controller=Login}/{action=Index}");

                endpoints.MapFallback(context => {
                    context.Response.Redirect("/Home/Error");
                    return Task.CompletedTask;
                });
            });
        }
    }
}
