using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BotDetect.Web;
using LikeKero.Contract.Validators;
using LikeKero.UI.ActionFilters;
using LikeKero.UI.Models;
using LikeKero.UI.Utility;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using LikeKero.UI.Extensions;
using LikeKero.UI.NLogger;


namespace LikeKero.UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(setupAction: options =>
            {
                options.EnableEndpointRouting = true;
                //options.Filters.Add<ValidationFilters>();
                //options.Filters.Add<ExceptionActionFilter>();
            }).AddFluentValidation(mvconfiguration =>
            {
                mvconfiguration.RegisterValidatorsFromAssembly(typeof(UserLoginRequestValidator).GetTypeInfo().Assembly);
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddSingleton<ILog, ErrorLoggerNLog>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.Configure<FormOptions>(options =>
            {
                options.ValueLengthLimit = int.MaxValue;
                options.MultipartBodyLengthLimit = int.MaxValue;
                options.MultipartHeadersLengthLimit = int.MaxValue;
            });

            services.Configure<MySettingsModel>(Configuration.GetSection("MySettings"));
            services.Configure<PaymentSettingsModel>(Configuration.GetSection("PaymentSettings"));
            services.Configure<VersionSettings>(Configuration.GetSection("VersionSettings"));



            int sessionTimeoutInMin = Convert.ToInt32(Configuration["MySettings:SessionTimeSpan"]);

            //services.AddDistributedMemoryCache();

            services.AddSession(
            options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(sessionTimeoutInMin);//You can set Time   
            });

            //add bahubali 7-may-2021 for JsonSerializer
            services.AddControllers()
               .AddJsonOptions(options =>
               {
                   options.JsonSerializerOptions.PropertyNamingPolicy = null;
               });

            //add bahubali 7-may-2021 for runtime Compilation
            services.AddControllersWithViews().AddRazorRuntimeCompilation();



            //services.AddMvc(

            //    setupAction: opt => opt.Filters.Add<TokenRereshFilter>()).AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver()).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);


            services.AddCors(options =>
            {
                options.AddPolicy("UIPolicy", builder =>
                {
                    builder.WithOrigins(Configuration["MySettings:AllowedDomain"]).AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                });
                //.AllowAnyMethod()
                //.AllowAnyHeader()
                //.AllowCredentials());
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger
              , ILoggerFactory logFactory, ILog nLogger
              )
        {
            if (env.EnvironmentName == "Development")
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.ConfigureExceptionHandler(nLogger);

            //app.UseExceptionHandler("/Error");
            ////  app.UseStatusCodePagesWithRedirects("/Error/{0}");
            //app.UseStatusCodePagesWithReExecute("/Error/{0}");
            //ApplicationLogging.LoggerFactory = logFactory;

            //app.UseStatusCodePagesWithRedirects("/Error/{0}");

            app.UseCors("UIPolicy");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();

            //add bahubali 4 June 2021
            CustomHelpers.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());


            // configure your application pipeline to use Captcha middleware
            // Important! UseCaptcha(...) must be called after the UseSession() call
            app.UseCaptcha(Configuration);

            app.Use(async (context, next) =>
            {
    // context.Response.Headers.Add("X-Frame-Options", "DENY"); // This
    context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN"); // Or this
                await next();
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Login}/{id?}"
                    );
                endpoints.MapRazorPages();
            });



            //app.UseMvc(routes =>
            //{
            //    // routes.MapRoute("SpecificRoute", "{association?}/{brand?}", new { controller = "Login", action = "Login" });

            //    //Code Change Amit 28-jan-2020
            //    //Based on the instance redirect to the login
            //    string instance= Configuration["RetailConfig:IsRetail"];

            //   routes
            //   .MapRoute(
            //       name: "default",
            //       template: Utility1.GetStartupRoute(instance));
            //});
        }
    }
}
