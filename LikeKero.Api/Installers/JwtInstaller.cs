using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LikeKero.Api.options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace LikeKero.Api.Installers
{
    public class JwtInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {

            var jwtsettings = new JwtSettings();
            configuration.Bind(key: nameof(jwtsettings), jwtsettings);
            services.AddSingleton(jwtsettings);

            var tokenvalidationparameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key: Encoding.ASCII.GetBytes(jwtsettings.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = false,
                ValidateLifetime = true
            };

            services.AddSingleton(tokenvalidationparameters);

            services.AddAuthentication(configureOptions: x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }

                ).AddJwtBearer(x =>
                {
                    x.SaveToken = true;
                    x.TokenValidationParameters = tokenvalidationparameters;
                }

                );
        }
    }
}
