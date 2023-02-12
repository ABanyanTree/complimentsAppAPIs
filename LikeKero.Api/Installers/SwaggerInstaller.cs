
using LikeKero.Api.options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace LikeKero.Api.Installers
{
    public class SwaggerInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "LikeKero : ComplimentsApp API", Version = "v1" });

                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer",new string[0] }

                };

                x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
        "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                x.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });



                //x.AddSecurityDefinition(name: "Bearer", new OpenApiKeyScheme
                //{
                //    Description = "JWT Authorization header using the bearer scheme ",
                //    Name = "Authorization",
                //    In = "header",
                //    Type = "apiKey"
                //});

                //x.AddSecurityRequirement(security);




                var xmfile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmfile);
                x.IncludeXmlComments(xmlPath);
            }
            );


            services.ConfigureSwaggerGen(options =>
            {
                options.OperationFilter<FileUploadOperation>(); //Register File Upload Operation Filter
            });
        }
    }
}
