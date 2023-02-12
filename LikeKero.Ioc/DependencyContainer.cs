using Microsoft.Extensions.DependencyInjection;
using LikeKero.Data.Interfaces;
using LikeKero.Data.Services;
using LikeKero.Data.Services.Feature;
using LikeKero.Data.Services.User;
using LikeKero.Domain.DomainInterfaces;
using LikeKero.Domain.DomainInterfaces.Feature;
using LikeKero.Domain.DomainInterfaces.User;
using LikeKero.Infra.BulkImport;
using LikeKero.Infra.EmailSender;
using LikeKero.Infra.FileSystem;
using LikeKero.Services.Interfaces;
using LikeKero.Services.Interfaces.Others;
using LikeKero.Services.Interfaces.User;
using LikeKero.Services.Services;
using LikeKero.Services.Services.Feature;
using LikeKero.Services.Services.Others;
using LikeKero.Services.Services.User;

namespace LikeKero.Ioc
{
    public class DependencyContainer
    {
        public string newstr { get; set; }

        public static void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton(typeof(IDapperResolver<>), typeof(DapperResolver<>));

            services.AddSingleton<IUser, UsersService>();
            services.AddSingleton<IUserRepository, UserRepository>();

            services.AddSingleton<IErrorLogs, ErrorLogsService>();
            services.AddSingleton<IErrorLogsRepository, ErrorLogsRepository>();
            

            //services.AddSingleton<IEntityName, EntityNameService>();
            //services.AddSingleton<IEntityNameRepository, EntityNameRepository>();

            services.AddSingleton<IFileUpload, FileUpload>();
          
            services.AddSingleton<IEmailSender, EmailSender>();

            services.AddSingleton<IEmailSenderEntity, EmailSenderEntityService>();
            services.AddSingleton<IEmailSenderEntityRepository, EmailSenderEntityRepository>();

            services.AddSingleton<IEmailSentLog, EmailSentLogService>();
            services.AddSingleton<IEmailSentLogRepository, EmailSentLogRepository>();

            services.AddSingleton<ILookupMaster, LookupMasterService>();
            services.AddSingleton<ILookupMasterRepository, LookupMasterRepository>();

           
            services.AddSingleton<IBulkImport, BulkImport>();

          
            services.AddSingleton<ISystemRole, SystemRoleService>();
            services.AddSingleton<ISystemRoleRepository, SystemRoleRepository>();         
            

            services.AddSingleton<IFeatureMaster, FeatureMasterService>();
            services.AddSingleton<IFeatureMasterRepository, FeatureMasterRepository>();


            services.AddSingleton<IRoleFeatureMaster, RoleFeatureMasterService>();
            services.AddSingleton<IRoleFeatureMasterRepository, RoleFeatureMasterRepository>();

           
            services.AddSingleton<IUtilityService, UtilityService>();


            services.AddSingleton<IRefreshTokenService, RefreshTokenService>();
            services.AddSingleton<IRefreshTokenRepository, RefreshTokenRepository>();

            //
            services.AddSingleton<ITestCustomer, TestCustomerService>();
            services.AddSingleton<ITestCustomerRepository, TestCustomerRepository>();

            

            services.AddSingleton<IADUserService, ADUserService>();
            services.AddSingleton<IADUserRepository, ADUserRepository>();

        }
    }
}
