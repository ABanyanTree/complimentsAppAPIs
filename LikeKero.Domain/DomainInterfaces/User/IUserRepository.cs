using LikeKero.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LikeKero.Domain.DomainInterfaces
{
    public interface IUserRepository: IRepository<UserMaster>
    {
        UserMaster LoginAndGetFeatures(UserMaster obj);

        Task<UserMaster> GetByEmailAsync(UserMaster obj);
        Task<IEnumerable<UserMaster>> GetAllUsersAsync(UserMaster obj);

        Task<int> ForgotPassword(UserMaster obj);

        UserSearchModel GetLookupsForUserSearch(List<KeyValuePair<string, string>> keyValuePairs);

        Task<UserMaster> IsEmailInUseAsync(List<KeyValuePair<string, string>> keyValuePairs);

        Task<int> AddEditAsync(UserMaster obj);
        Task<UserMaster> IsLoginIdInUseAsync(List<KeyValuePair<string, string>> keyValuePairs);

        Task<int> ActiveInActiveUser(UserMaster obj);

        Task UpdateProfilePic(List<KeyValuePair<string, string>> keyValuePairs);
        Task<UserMaster> GetUserData(UserMaster obj);
        Task<UserMaster> InserUserMasterViaBulk(UserMaster obj);
        Task<int> ChangePasswordAsync(UserMaster obj);
        Task<UserMaster> GetSelfUser(UserMaster obj);

        Task<IEnumerable<UserMaster>> ResolveBR(UserMaster obj);
        Task<IEnumerable<UserMaster>> ResolveBR_Assigned_User(UserMaster obj);
        Task<IEnumerable<UserMaster>> ResolveBussinessAssignmentUser(UserMaster obj);
        Task<IEnumerable<UserMaster>> GetUserRolesbyUserId(UserMaster obj);
        Task<IEnumerable<UserMaster>> UserListForEnrollment(UserMaster obj);
        Task<IEnumerable<UserMaster>> OrderEnrolledUsersList(UserMaster obj);
        Task<UserMaster> IsUserEnrolledByOrderCode(List<KeyValuePair<string, string>> keyValuePairs);
        Task<UserMaster> GetCartCount(UserMaster obj);

        Task<UserMaster> RunBRforAddeduser(UserMaster obj);
        Task<UserMaster> RunBRforBulkImport(UserMaster obj);
        Task<int> RemoveGroupAdmin(UserMaster obj);
        Task<IEnumerable<UserMaster>> GetAllUsersForTransfer(UserMaster obj);

        Task<int> TransferUser(UserMaster obj);
        Task<int> TransferUserMaster(UserMaster obj);
        Task<int> TrackSignOutTime(UserMaster obj);

        Task<IEnumerable<UserMaster>> GetNotLoggedInUsers();
        Task<int> SaveWelcomeEmailSummary(UserMaster obj);
        Task<IEnumerable<UserMaster>> GetWelcomeEmailLog(UserMaster obj);

        Task<IEnumerable<UserMaster>> ResolveBRExpired(UserMaster obj);


        //

        UserMaster GetFeaturesByUserRole(UserMaster obj);
        Task<int> AddEditSpecialUser(UserMaster obj);
        Task<IEnumerable<UserMaster>> GetAllSpecialUsersAsync(UserMaster obj);
        Task<UserMaster> GetSpecialUser(UserMaster obj);
        Task<UserMaster> IsUserExist(string emailAddress);
        Task<UserMaster> IsInUseCount(string emailAddress);
        Task<int> DeleteSpecialUser(UserMaster userMaster);
    }
}
