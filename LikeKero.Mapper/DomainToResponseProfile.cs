using AutoMapper;
using LikeKero.Contract.Responses;
using LikeKero.Contract.Responses.Feature;
using LikeKero.Contract.Responses.Others;
using LikeKero.Contract.Responses.SystemRole;
using LikeKero.Contract.Responses.User;
using LikeKero.Domain;
using LikeKero.Domain.Feature;

namespace LikeKero.Mapper
{
    public class DomainToResponseProfile : Profile
    {

        public DomainToResponseProfile()
        {
            CreateMap<UserMaster, AuthSuccessResponse>();
            CreateMap<FeatureMaster, FeatureMasterResponse>();
           // CreateMap<UserHierarchy, UserHierarchyResponse>();
            CreateMap<UserRole, UserRoleResponse>();
            CreateMap<UserMaster, UserForgotPasswordResponse>();
           // CreateMap<EntityName, EntityNameResponse>();
           // CreateMap<EntityName, EntityAssociationResponse>();
            //CreateMap<EntityName, ManageNCPDPResponse>();

            CreateMap<UserMaster, UserListResponse>();

            //CreateMap<DepartmentMaster, DepartmentMasterResponse>();
            //CreateMap<JobRoleMaster, JobRoleMasterResponse>();
            CreateMap<LookupMaster, LookUpMasterResponse>();
          
            CreateMap<UserSearchModel, UserSearchModelResponse>();

          
            CreateMap<UserMaster, GetUserResponse>();
            CreateMap<EmailSenderEntity, EmailEntityResponse>();

           

          
            CreateMap<ErrorLogs, ErrorLogResponse>();
            CreateMap<EmailSentLog, EmailSentLogResponse>();

            CreateMap<EmailSentLog, EmailGetResponse>();
            CreateMap<ErrorLogs, ErrorGetResponse>();

            //CreateMap<OIGData, OIGCheckResponse>();

         

            CreateMap<LookupMaster, LookupByTypeResponse>();

          

      

          //  CreateMap<UserRole, GroupAdminUsersResponse>();
            CreateMap<UserRole, SystemRoleResponse>();
            CreateMap<UserRole, AssignmentSearchResponse>();

         

          
            CreateMap<FeatureMaster, FeatureMasterResponse>();
            CreateMap<UserRole, AdminUserRoleResponse>();
            CreateMap<RoleFeatureMaster, RoleFeatureResponse>();
            CreateMap<FeatureMaster, AdminRightsResponse>();

          


         
            //CreateMap<OIGUploadHistoryLog, SearchOIGUploadHistoryLogResponse>();

         
            //CreateMap<OIGUploadHistory, SearchOIGHistoryResponse>();



       
            //CreateMap<OIGUploadHistoryLog, SearchOIGUploadHistoryLogResponse>();



        


          

            CreateMap<UserMaster, UserRoleResponse>();

      
            CreateMap<UserMaster, UserCourseEnrollmentResponse>();
            //CreateMap<UserMaster, CourseEnrolledUsersResponse>();
           

            //KIRAN(4feb2020): Policy
           

            //CreateMap<UserTransferLog, UserTransferLogResponse>();

            
            //CreateMap<EntityName, NCPDPQuickLookResponse>();
          

            CreateMap<UserMaster, WelcomeEmailLogResponse>();

            CreateMap<TestCustomer, TestCustomerGetResponse>();

            
            
        }

    }
}
