using LikeKero.UI.ViewModels;

using LikeKero.UI.ViewModels.Request.User;
using LikeKero.UI.ViewModels.Response;
using LikeKero.UI.ViewModels.Response.User;
using Refit;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace LikeKero.UI.RefitClientFactory
{
    [Headers("Authorization: Bearer")]

    public interface IUserApi
    {
        [Post(path: "/api/user/login")]
        Task<ApiResponse<AuthSuccessResponseVM>> LoginAsync(UserLoginRequestVM model);
        [Post(path: "/api/user/getuser")]
        Task<ApiResponse<AuthSuccessResponseVM>> GetUserAsync(UserLoginRequestVM model);
        [Get(path: "/api/user/getsalt")]
        Task<HttpResponseMessage> GetMD5Salt();

        [Post(path: "/api/user/userlist")]
        Task<ApiResponse<List<UserListResponseVM>>> GetUserList(UserListRequestVM model);

        [Post(path: "/api/user/forgotpassword")]
        Task<ApiResponse<AuthSuccessResponseVM>> ForgotPasswordAsync(UserForgotPasswordRequestVM model);

        //[Get(path: "/api/entity/entitylist")]
        //Task<ApiResponse<List<EntityNameReponseVM>>> EntityList(string RequesterUserId, string SearchPhrase, string GroupId);

        [Get(path: "/api/user/getlookups")]
        Task<ApiResponse<UserListResponseVM>> GetLookUps();

        [Get(path: "/api/user/isemailinuse")]
        Task<HttpResponseMessage> IsEmailInUse(string EmailAddress, string UserID);

        [Get(path: "/api/user/isloginidinuse")]
        Task<HttpResponseMessage> IsLoginIDInUse(string LoginId, string UserID);

        [Post(path: "/api/user/addedituser")]
        Task<HttpResponseMessage> AddEditUser(UserMasterRequestVM model);


        [Post(path: "/api/state/countrystatelist")]
        Task<ApiResponse<List<UserMasterRequestVM>>> CountryStateList(UserMasterRequestVM model);

        [Post(path: "/api/state/statecitieslist")]
        Task<ApiResponse<List<UserMasterRequestVM>>> StateCitiesList(UserMasterRequestVM model);


        //[Multipart]
        //[Post(path: "/api/user/uploaduserprofilepic")]
        //Task<HttpResponseMessage> UploadUserProfilePic(string strUserID, StreamPart uploadedFile);

        [Post(path: "/api/user/activeinactiveuser")]
        Task<ApiResponse<List<HttpResponseMessage>>> ActiveInActiveUserAsync(UserActiveInActiveRequestVM model);

        [Post(path: "/api/user/getuserdata")]
        Task<ApiResponse<UserMasterRequestVM>> GetUserDataAsync(GetUserRequestVM model);

        //[Get(path: "/api/user/getusertemplateexcel")]
        //Task<HttpResponseMessage> GetUserTemplateExcel();

        //[Multipart]
        //[Post(path: "/api/user/uploaduserimport")]
        //// Task<HttpResponseMessage> UploadUserImport(string requestorUserId, StreamPart uploadedFile);
        //Task<ApiResponse<BulkImportListResponseVM>> UploadUserImport(string requestorUserId, StreamPart uploadedFile);

        //[Post(path: "/api/user/bulkimportlistall")]
        //Task<ApiResponse<List<BulkImportListResponseVM>>> BulkImportListAll(BulkImportListRequestVM request);

        //[Post(path: "/api/user/checkoig")]
        //Task<ApiResponse<OIGCheckResponseVM>> CheckOIG(OIGCheckRequestVM model);

        //[Get(path: "/api/user/getbulkimportuploadedfile")]
        //Task<HttpResponseMessage> GetBulkImportUploadedFile(string BulkImportId);

        //[Get(path: "/api/user/bulkimportmasterrecord")]
        //Task<ApiResponse<BulkImportListResponseVM>> BulkImportMasterRecord(string BulkImportId);

        //[Get(path: "/api/user/bulkimportlogs")]
        //Task<ApiResponse<List<BulkImportLogListResponseVM>>> BulkImportLogs(string BulkImportId);

        //[Post(path: "/api/lookup/getlookupbytype")]
        //Task<ApiResponse<List<LookupByTypeResponseVM>>> GetLookupByType(string LookupType);

        //[Get(path: "/api/user/getbulkimportlog")]
        //Task<ApiResponse<BulkImportLogResponseVM>> GetBulkImportLog(string BulkImportLogId);

        [Post(path: "/api/user/changepassword")]
        Task<ApiResponse<HttpResponseMessage>> ChangePassword(ChangePasswordRequestVM model);

        [Post(path: "/api/user/exportuserdata")]
        Task<HttpResponseMessage> ExportUserData(UserListRequestVM model);

        //[Post(path: "/api/user/saveoigusers")]
        //Task<HttpResponseMessage> SaveOIGUsers(string BulkIMportLogId, string RequesterUserId);

        [Get(path: "/api/user/GetSelfUserInfo")]
        Task<ApiResponse<SelfUserResponseVM>> GetSelfUserInfo(SelfUserRequestVM model);

        [Post(path: "/api/user/refresh")]
        Task<ApiResponse<AuthSuccessResponseVM>> Refresh(RefreshTokenRequestVM model);


        [Get(path: "/api/user/getrolelistbyuserid")]
        Task<ApiResponse<List<UserRoleResponseVM>>> GetRoleListByUserId(GetUserRequestVM request);

        //[Get(path: "/api/user/isuserenrolledbyordercode")]
        //Task<HttpResponseMessage> IsUserEnrolledByOrderCode(string OrderCode, string RequesterUserId);

        //[Post(path: "/api/course/enrolluserbyordercode")]
        //Task<HttpResponseMessage> EnrollUserByOrderCode(string OrderCode, string RequesterUserId);

        //[Get(path: "/api/user/isordercodeexists")]
        //Task<HttpResponseMessage> IsOrderCodeExists(string OrderCode);

        [Post(path: "/api/user/removegroupadmin")]
        Task<HttpResponseMessage> RemoveGroupAdmin(string UserRoleId);

        //[Get(path: "/api/entity/entitylisttomakeadmin")]
        //Task<ApiResponse<List<EntityNameReponseVM>>> EntityListToMakeAdmin(SearchEntityRequestVM request);

        //[Get(path: "/api/user/UserListForTransfer")]
        //Task<ApiResponse<List<UserListResponseVM>>> UserListForTransfer(UserListRequestVM model);

        //[Post(path: "/api/user/transferusers")]
        //Task<HttpResponseMessage> TransferUsers(TransferUserRequestVM model);

        //[Post(path: "/api/user/transferusermaster")]
        //Task<HttpResponseMessage> TransferUserMaster(TransferUserRequestVM model);

        //[Get(path: "/api/user/transferlog")]
        //Task<ApiResponse<List<UserTransferLogResponseVM>>> TransferLog(UserTransferLogRequestVM model);

        //[Get(path: "/api/user/transferlogdetails")]
        //Task<ApiResponse<List<UserTransferLogResponseVM>>> TransferLogDetails(UserTransferLogRequestVM model);

        //[Get(path: "/api/user/transferlogmasterrecord")]
        //Task<ApiResponse<UserTransferLogResponseVM>> TransferLogMasterRecord(string MasterLogId);

        //[Post(path: "/api/user/exportusertransferdata")]
        //Task<HttpResponseMessage> ExportUserTransferData(UserTransferLogRequestVM model);

        //[Post(path: "/api/user/userlistformanualcompletion")]
        //Task<ApiResponse<List<UserListResponseVM>>> UserListForManualCompletion(UserListRequestVM model);


        //[Get(path: "/api/entity/getgrouprecord")]
        //Task<ApiResponse<EntityNameReponseVM>> GetGroupRecord(string GroupId);

        [Get(path: "/api/user/tracksignouttime")]
        Task<HttpResponseMessage> TrackSignOutTime(string loginUniqueId);

        //[Get(path: "/api/user/welcomeemailfornotloggedinusers")]
        //Task<HttpResponseMessage> WelcomeEmailForNotLoggedInUsers(string RequesterUserId);

        //[Get(path: "/api/entity/grouplistwithdescr")]
        //Task<ApiResponse<List<EntityNameReponseVM>>> GroupListWithDescr(string RequesterUserId, string SearchPhrase, string GroupId);


        //

        [Post(path: "/api/user/getfeaturesbyuserrole")]
        Task<ApiResponse<AuthSuccessResponseVM>> GetFeaturesByUserRole(UserFeaturesRequestVM model);
    }
}
