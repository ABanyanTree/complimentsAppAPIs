using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LikeKero.Api.Filters;
using LikeKero.Api.options;
using LikeKero.Contract.Validators;
using LikeKero.Domain;
using LikeKero.Infra;
using LikeKero.Infra.BaseUri;
using LikeKero.Infra.EmailSender;
using LikeKero.Infra.FileSystem;
using LikeKero.Infra.Others;
using LikeKero.Ioc;
using LikeKero.Mapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace LikeKero.Api.Installers
{
    public class MVCInstallers : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            

            services.AddMvc(setupAction: options => {
                options.EnableEndpointRouting = false;
                options.Filters.Add<ValidationFilters>();
                options.Filters.Add<ExceptionActionFilter>();
            }).AddFluentValidation(mvconfiguration => { mvconfiguration.RegisterValidatorsFromAssembly(typeof(UserLoginRequestValidator).GetTypeInfo().Assembly); })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.Configure<ReadConfig>(configuration.GetSection("data:DBcon"));
            services.Configure<FileSystemPath>(configuration.GetSection("FileSystemPath"));
            services.Configure<EmailConfig>(configuration.GetSection("EmailConfig"));
            services.Configure<VersionSettings>(configuration.GetSection("VersionSettings"));

            services.Configure<RetailConfig>(configuration.GetSection("RetailConfig"));

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new DomainToResponseProfile());
                mc.AddProfile(new RequestToDomain());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);//Dep

            services.AddSingleton<IUriService>(provider =>
            {
                var httpContextAccesser = provider.GetRequiredService<IHttpContextAccessor>();
                var request = httpContextAccesser.HttpContext.Request;
                var absoluteUrl = string.Concat(request.Scheme + "://" + request.Host.ToUriComponent() + "/");
                var profilePicName = configuration.GetSection("FileSystemPath:DefaultProfilePicName");
                return new UriService(absoluteUrl, profilePicName.Value);
            });


            DependencyContainer.RegisterServices(services);

            //RetailDependencyContainer.RegisterServices(services);
        }
    }
}
