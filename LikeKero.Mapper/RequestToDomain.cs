using AutoMapper;
using LikeKero.Contract.Requests;
using LikeKero.Contract.Requests.Feature;
using LikeKero.Contract.Requests.Others;
using LikeKero.Contract.Requests.User;
using LikeKero.Domain;
using LikeKero.Domain.Feature;

namespace LikeKero.Mapper
{
    public class RequestToDomain : Profile
    {

        public RequestToDomain()
        {

            
            //CreateMap<StateMasterRequest, StateMaster>();
            //CreateMap<CityMasterRequest, CityMaster>();
                    CreateMap<EmailEntityRequest, EmailSenderEntity>();

          
            //CreateMap<BulkImportListRequest, BulkImportMaster>();
            CreateMap<ErrorLogRequest, ErrorLogs>();
            CreateMap<EmailSentLogRequest, EmailSentLog>();

            CreateMap<EmailGetRequest, EmailSentLog>();

            CreateMap<ErrorGetRequest, ErrorLogs>();
            //CreateMap<OIGCheckRequest, OIGData>();

            CreateMap<LookupByTypeRequest, LookupMaster>();
            CreateMap<ChangePasswordRequest, UserMaster>();

           

            //CreateMap<BulkImportMasterRequest, BulkImportMaster>();

         

            CreateMap<SystemRoleCreateUpdateRequest, UserRole>();


            CreateMap<AdminRightsRequest, RoleFeatureMaster>();

          

            //CreateMap<GetBusinessRuleRequest, UserMaster>();

          


           


            //CreateMap<SearchOIGRequest, OIGData>();

            //CreateMap<SaveOIGHistoryRequest, OIGData>();
           
            //CreateMap<SaveOIGHistoryRequest, OIGUploadHistory>();
            //CreateMap<SearchOIGHistoryLogRequest, OIGUploadHistoryLog>();
            //CreateMap<SearchOIGHistoryRequest, OIGUploadHistory>();

          
            //CreateMap<SearchOIGUserRequest, OIGData>();
            //CreateMap<SearchOIGEntityrRequest, OIGData>();

            //CreateMap<SaveOIGHistoryRequest, OIGData>();


      

            CreateMap<RefreshTokenRequest, RefreshToken>();

           

            CreateMap<UserCourseEnrollmentRequest, UserMaster>();
          
           
        
            //KIRAN(4feb2020): Policy
           

            //CreateMap<TransferUserRequest, UserMaster>();

           // CreateMap<UserTransferLogRequest, UserTransferLog>();


            CreateMap<RefreshTokenRequest, RefreshToken>();


            
            //
            CreateMap<TestCustomerRequest, TestCustomer>();
            CreateMap<SearchTestCustomerRequest, TestCustomer>();

            CreateMap<UserFeaturesRequest, UserMaster>();

          
            CreateMap<ADUserListRequest, ADUser>();
            CreateMap<SpecialUserRequest, UserMaster>();
            CreateMap<CreateSpecialUserRequest, UserMaster>();

        }



    }
}
