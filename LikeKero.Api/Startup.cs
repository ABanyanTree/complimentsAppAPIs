using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LikeKero.Api.Installers;
using LikeKero.Api.Middleware;
using LikeKero.Api.options;
using LikeKero.Mapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LikeKero.Api
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
            services.InstallServicesInAssembly(Configuration);
            services.AddSingleton<FileHandlerMiddleWare>();
            services.Configure<FormOptions>(options =>
            {
                options.ValueLengthLimit = int.MaxValue;
                options.MultipartBodyLengthLimit = int.MaxValue;
                options.MultipartHeadersLengthLimit = int.MaxValue;
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{

            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    app.UseHsts();
            //}

            app.UseDeveloperExceptionPage();

            string PdfgeneratorKey = Configuration.GetValue<string>("VersionSettings:PdfgeneratorKey");
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(PdfgeneratorKey);

            var swaggeroptions = new SwaggerOptions();
            Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggeroptions);


            app.UseSwagger(options =>
            {
                options.RouteTemplate = swaggeroptions.JsonRoute;
            });

            if (swaggeroptions.ShowDefinition == "1")
            {
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint(swaggeroptions.UIEndpoint, swaggeroptions.Description);
                });
            }


            var corsSetting = new CorsSetting();
            Configuration.GetSection(nameof(CorsSetting)).Bind(corsSetting);

                   
            //app.ConfigreFileHandleMiddleWare();
            app.UseHttpsRedirection();
            app.UseStaticFiles();



            //Allowed All Origin
            //app.UseCors(corsSetting.policyName);
            app.UseCors(builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });



            app.UseAuthentication();

            string AllowAllDomain = Configuration.GetValue<string>("VersionSettings:AllowAllDomain");
            if (AllowAllDomain == "1")
            {
                app.Use(async (context, next) =>
                {
                    await next();
                });
            }
            else
            {
                app.Use(async (context, next) =>
                {
                    context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN"); // Or this
                    await next();
                });
            }

            app.UseMvc();

        }
    }
}
