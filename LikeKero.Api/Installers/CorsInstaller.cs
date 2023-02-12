using LikeKero.Api.options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LikeKero.Api.Installers
{
    public class CorsInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var corsSetting = new CorsSetting();
            configuration.GetSection(nameof(CorsSetting)).Bind(corsSetting);

            services.AddCors(options =>
            {
                options.AddPolicy(corsSetting.policyName, builder =>
                {
                    builder.WithOrigins(corsSetting.AllowedDomain).AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                });
                       //.AllowAnyMethod()
                       //.AllowAnyHeader()
                       //.AllowCredentials());
            });
        }
    }
}
