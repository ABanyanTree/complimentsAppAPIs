using LikeKero.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LikeKero.Services.Interfaces
{
   public interface IUser: IServiceBase<UserMaster>
    {
       
        UserMaster LoginAndGetFeatures(UserMaster obj);
        UserMaster TestLogin(UserMaster obj);
        Task<IEnumerable<UserMaster>> GetAllUsersAsync(UserMaster obj);
        Task<int> SetNewPassword(UserMaster obj);
        Task<UserMaster> GetByEmailAsync(UserMaster obj);
        UserSearchModel GetLookupsForUserSearch();
        Task<UserMaster> IsEmailInUseAsync(string EmailAddress, string UserId);
        Task<string> AddEditUserAsync(UserMaster obj);
        Task<UserMaster> IsLoginIDInUseAsync(string LoginID, string UserId);
        Task<int> ActiveInActiveUser(UserMaster obj);
       // Task<bool> UploadProfilePic(string strUserID, IFormFile formFile);
       // Task<Stream> GetUserTemplateFile();
        //Task<Stream> GetBulkImportUploadedFile(string BulkImportId, string BulkImportFileName);
        Task<UserMaster> InserUserMasterViaBulk(UserMaster obj);
        Task<int> ChangePassword(UserMaster obj);
        Task<UserMaster> GetSelfUserInfo(UserMaster obj);
        Task<Stream> GetUserExportData(UserMaster obj);
        Task<IEnumerable<UserMaster>> ResolveBR(UserMaster obj);

        Task<IEnumerable<UserMaster>> ResolveBR_Assigned_User(UserMaster obj);
        Task<IEnumerable<UserMaster>> ResolveBussinessAssignmentUser(UserMaster obj);
        UserMaster LoginAndGetFeaturesWithEmail(UserMaster obj);
        Task<IEnumerable<UserMaster>> GetUserRolesbyUserId(UserMaster obj);

        Task<IEnumerable<UserMaster>> UserListForEnrollment(UserMaster obj);
        Task<IEnumerable<UserMaster>> OrderEnrolledUsersList(UserMaster obj);
        Task<UserMaster> IsUserEnrolledByOrderCode(string OrderCode, string RequesterUserId);
        Task<UserMaster> GetCartCount(UserMaster obj);
        Task<UserMaster> RunBRforBulkImport(UserMaster obj);
        Task<int> RemoveGroupAdmin(UserMaster obj);

        Task<IEnumerable<UserMaster>> GetAllUsersForTransfer(UserMaster obj);
        Task<int> TransferUser(UserMaster obj);
        Task<int> TransferUserMaster(UserMaster obj);
        
        Task<int> TrackSignOutTime(UserMaster obj);
        Task<UserMaster> SendWelcomeEmailToUsersNotLogIn(string RequesterUserId);
        Task<IEnumerable<UserMaster>> GetWelcomeEmailLog(UserMaster obj);

        Task<IEnumerable<UserMaster>> ResolveBRExpired(UserMaster obj);



        //

        UserMaster GetFeaturesByUserRole(UserMaster obj);
        Task<string> AddEditSpecialUser(UserMaster userobj);
        Task<IEnumerable<UserMaster>> GetAllSpecialUsersAsync(UserMaster userobj);
        Task<UserMaster> GetSpecialUser(UserMaster userMaster);
        Task<UserMaster> IsUserExist(string emailAddress);
        Task<UserMaster> IsInUseCount(string emailAddress);
        Task<int> DeleteSpecialUser(UserMaster userMaster);
    }
}
