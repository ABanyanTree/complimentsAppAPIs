using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LikeKero.Data.Constants;
using LikeKero.Data.Interfaces;
using LikeKero.Domain;
using LikeKero.Domain.DomainInterfaces;

using LikeKero.Domain.Utility;
using Microsoft.Extensions.Options;

namespace LikeKero.Data.Services
{
    public class UserRepository : Repository<UserMaster>, IUserRepository
    {

        public UserRepository(IOptions<ReadConfig> connStr, IDapperResolver<UserMaster> resolver) : base(connStr, resolver)
        {

        }

        public Task<UserMaster> GetByEmailAsync(UserMaster obj)
        {
            //string[] addParams = new string[] { UserMasterInfra.EMAILADDRESS, UserMasterInfra.ISLOGIN };
            string[] addParams = new string[] { UserMasterInfra.EMAILADDRESS };
            return GetAsync(obj, addParams, UserMasterInfra.SPROC_USERBYEMAIL);
        }

        public async Task<UserMaster> IsEmailInUseAsync(List<KeyValuePair<string, string>> keyValuePairs)
        {

            return await GetAsyncWithDynamicParam(keyValuePairs, UserMasterInfra.SPROC_USER_ISEMAILINUSE);
        }

        public async Task<int> AddEditAsync(UserMaster obj)
        {

            string[] addParams = new string[] { UserMasterInfra.USERID, UserMasterInfra.FIRSTNAME, UserMasterInfra.LASTNAME,UserMasterInfra.LOGINID,
            UserMasterInfra.EMAILADDRESS,UserMasterInfra.PASSWORD,UserMasterInfra.CONTACTNUMBER,
            UserMasterInfra.HIRINGDATE,UserMasterInfra.ZIPCODE,UserMasterInfra.STATUS,UserMasterInfra.DEPARTMENTID,
            UserMasterInfra.JOBCODEID,UserMasterInfra.ROLECHANGEDATE,UserMasterInfra.TIMEZONEID,
            UserMasterInfra.REQUESTERUSERID,UserMasterInfra.COUNTRYID,UserMasterInfra.STATEID,UserMasterInfra.CITYID,
            UserMasterInfra.GROUPID,UserMasterInfra.ISADMIN

            };
            return await ExecuteNonQueryAsync(obj, addParams, UserMasterInfra.SPROC_ADDEDIT_USERMASTER);
        }

        public async Task UpdateProfilePic(List<KeyValuePair<string, string>> keyValuePairs)
        {
            await ExecuteNonQueryAsyncWithDynamicParam(keyValuePairs, UserMasterInfra.SPROC_UPDATE_USERPROFILEPIC);
        }


        public async Task<IEnumerable<UserMaster>> GetAllUsersAsync(UserMaster obj)
        {
            string[] addParams = new string[] { BaseInfra.PAGEINDEX, BaseInfra.PAGESIZE, BaseInfra.SORTEXP, UserMasterInfra.FIRSTNAME, UserMasterInfra.EMAILADDRESS, UserMasterInfra.LASTNAME,
                UserMasterInfra.STATUS, UserMasterInfra.DEPARTMENTID, UserMasterInfra.JOBCODEID, UserMasterInfra.REQUESTERUSERID,
                UserMasterInfra.SEARCHONGROUPID, UserMasterInfra.GROUPID, UserMasterInfra.HIRINGDATEFROM, UserMasterInfra.HIRINGDATETO, UserMasterInfra.ROLECHANGEDATEFROM, UserMasterInfra.ROLECHANGEDATETO, UserMasterInfra.NOPAGING
                //, UserMasterInfra.ISADMIN
            };
            return await GetAllAsync(obj, addParams, UserMasterInfra.SPROC_USERMASTER_LSTALL);
        }

        public UserMaster LoginAndGetFeatures(UserMaster obj)
        {
            IEnumerable<dynamic> mapItems = new List<dynamic>()
                {
                    new MapItem(typeof(UserMaster), "UserMaster"),
                    //new MapItem(typeof(UserHierarchy), "UserHierarchy"),
                    new MapItem(typeof(UserRole), "UserRole"),
                    new MapItem(typeof(FeatureMaster), "FeatureMaster")
                };

            string[] addParams = new string[] { UserMasterInfra.EMAILADDRESS };
            dynamic dynamicObject = GetMultipleAsync(obj, addParams, UserMasterInfra.SPROC_LOGIN_USER_BY_EMAIL_PASSWORD, mapItems);


            var User = ((List<dynamic>)dynamicObject.UserMaster).Cast<UserMaster>().ToList()?.FirstOrDefault();
            //var UserHierarchy = ((List<dynamic>)dynamicObject.UserHierarchy).Cast<UserHierarchy>().ToList()?.FirstOrDefault();
            var UserRoleList = ((List<dynamic>)dynamicObject.UserRole).Cast<UserRole>().ToList();
            var FeatureList = ((List<dynamic>)dynamicObject.FeatureMaster).Cast<FeatureMaster>().ToList();

            if (User != null)
            {
                // User.userHierarchy = UserHierarchy;
                User.userRoles = UserRoleList;
                User.Features = FeatureList;
                return User;
            }

            return null;
        }

        public Task<int> ForgotPassword(UserMaster obj)
        {
            string[] addParams = new string[] { UserMasterInfra.EMAILADDRESS, UserMasterInfra.PASSWORD };
            return ExecuteNonQueryAsync(obj, addParams, UserMasterInfra.SPROC_SETNEWPASSWORD);
        }

        public Task<int> ActiveInActiveUser(UserMaster obj)
        {
            string[] addParams = new string[] { UserMasterInfra.STATUS, UserMasterInfra.USERID };
            return ExecuteNonQueryAsync(obj, addParams, UserMasterInfra.SPROC_ACTIVEINACTIVEUSER);
        }

        public UserSearchModel GetLookupsForUserSearch(List<KeyValuePair<string, string>> keyValuePairs)
        {
            IEnumerable<dynamic> mapItems = new List<dynamic>()
                {
                    new MapItem(typeof(LookupMaster), "LookupMaster"),
                    //new MapItem(typeof(DepartmentMaster), "DepartmentMaster"),
                    //new MapItem(typeof(JobRoleMaster), "JobRoleMaster"),
                  
                    //new MapItem(typeof(LookupMaster), "EmpManagerJobRole"),
                };


            dynamic dynamicObject = GetMultipleAsyncWithDynamicParam(keyValuePairs, UserMasterInfra.SPROC_MANAGEUSERS_LOOKUP, mapItems);

            var ListLookUpMaster = ((List<dynamic>)dynamicObject.LookupMaster).Cast<LookupMaster>().ToList();
            // var ListDepartmentMaster = ((List<dynamic>)dynamicObject.DepartmentMaster).Cast<DepartmentMaster>().ToList();

            //var ListEmpManagerJobRole = ((List<dynamic>)dynamicObject.EmpManagerJobRole).Cast<LookupMaster>().ToList();

            UserSearchModel userSearchModel = new UserSearchModel();

            if (userSearchModel != null)
            {
                userSearchModel.ListUserStatus = ListLookUpMaster;
                // userSearchModel.ListDepartments = ListDepartmentMaster;

                //userSearchModel.ListEmpManagerJobRole = ListEmpManagerJobRole;

                return userSearchModel;
            }

            return null;
        }

        public async Task<UserMaster> IsLoginIdInUseAsync(List<KeyValuePair<string, string>> keyValuePairs)
        {

            return await GetAsyncWithDynamicParam(keyValuePairs, UserMasterInfra.SPROC_USER_ISLOGINIDINUSE);
        }

        public async Task<UserMaster> GetUserData(UserMaster obj)
        {
            string[] addParams = new string[] { UserMasterInfra.REQUESTERUSERID, UserMasterInfra.USERID };
            return await GetAsync(obj, addParams, UserMasterInfra.SPROC_GETUSER_BY_USERID);
        }

        public async Task<UserMaster> InserUserMasterViaBulk(UserMaster obj)
        {
            string[] addParams = new string[] { UserMasterInfra.USERID, UserMasterInfra.FIRSTNAME, UserMasterInfra.LASTNAME,UserMasterInfra.BULKIMPORTID,
            UserMasterInfra.EMAILADDRESS,UserMasterInfra.PASSWORD,
            UserMasterInfra.HIRINGDATE,
            UserMasterInfra.JOBCODEID,UserMasterInfra.ROLECHANGEDATE,
            UserMasterInfra.REQUESTERUSERID,


            };
            return await GetAsync(obj, addParams, UserMasterInfra.SPROC_CHECKBULKIMPORTVALIDATION_USERS);
        }

        public async Task<int> ChangePasswordAsync(UserMaster obj)
        {
            string[] addParams = new string[] { UserMasterInfra.USERID, UserMasterInfra.PASSWORD, UserMasterInfra.ISPASSWORDCHANGED, BaseInfra.REQUESTERUSERID };
            return await ExecuteNonQueryAsync(obj, addParams, UserMasterInfra.SPROC_CHANGEPASSWORD);
        }

        public async Task<UserMaster> GetSelfUser(UserMaster obj)
        {
            string[] addParam = new string[] { UserMasterInfra.USERID, BaseInfra.REQUESTERUSERID };
            return await GetAsync(obj, addParam, UserMasterInfra.SPROC_GETSELFUSERINFO);
        }


        public async Task<IEnumerable<UserMaster>> ResolveBR(UserMaster obj)
        {
            string[] addParams = new string[] { BaseInfra.PAGEINDEX, BaseInfra.PAGESIZE, BaseInfra.SORTEXP,
                UserMasterInfra.BRID,UserMasterInfra.REQUESTERUSERID,UserMasterInfra.FIRSTNAME,UserMasterInfra.LASTNAME
            };
            return await GetAllAsync(obj, addParams, UserMasterInfra.SPROC_RESOLVE_BR);
        }

        public async Task<IEnumerable<UserMaster>> ResolveBRExpired(UserMaster obj)
        {
            string[] addParams = new string[] { BaseInfra.PAGEINDEX, BaseInfra.PAGESIZE, BaseInfra.SORTEXP, UserMasterInfra.REQUESTERUSERID, UserMasterInfra.FIRSTNAME, UserMasterInfra.LASTNAME, null
            };
            return await GetAllAsync(obj, addParams, UserMasterInfra.SPROC_RESOLVE_BR_EXPIRED);
        }

        public async Task<IEnumerable<UserMaster>> ResolveBR_Assigned_User(UserMaster obj)
        {
            string[] addParams = new string[] { BaseInfra.PAGEINDEX, BaseInfra.PAGESIZE, BaseInfra.SORTEXP,
                UserMasterInfra.BRACTIVITYID,UserMasterInfra.REQUESTERUSERID,UserMasterInfra.FIRSTNAME
            };
            return await GetAllAsync(obj, addParams, UserMasterInfra.SPROC_RESOLVEBR_AFTERASSIGNMENT);
        }

        public async Task<IEnumerable<UserMaster>> ResolveBussinessAssignmentUser(UserMaster obj)
        {
            string[] addParams = new string[] { BaseInfra.PAGEINDEX, BaseInfra.PAGESIZE, BaseInfra.SORTEXP,
                null,UserMasterInfra.FIRSTNAME,UserMasterInfra.LASTNAME
            };
            return await GetAllAsync(obj, addParams, UserMasterInfra.SPROC_RESOLVE_BUSINESSASSIGNMENT);
        }


        public async Task<IEnumerable<UserMaster>> GetUserRolesbyUserId(UserMaster obj)
        {
            string[] addParams = new string[] { UserMasterInfra.USERID
            };
            return await GetAllAsync(obj, addParams, UserMasterInfra.SPROC_SHOWROLES_FOR_USER_BYUSERID);
        }

        public async Task<IEnumerable<UserMaster>> UserListForEnrollment(UserMaster obj)
        {
            string[] addParams = new string[] { UserMasterInfra.FIRSTNAME, UserMasterInfra.LASTNAME, UserMasterInfra.SEARCHONGROUPID, UserMasterInfra.GROUPID, UserMasterInfra.JOBCODEID, BaseInfra.REQUESTERUSERID, null };

            return await GetAllAsync(obj, addParams, UserMasterInfra.SPROC_USERLISTFORENROLLMENT);
        }

        public async Task<IEnumerable<UserMaster>> OrderEnrolledUsersList(UserMaster obj)
        {
            string[] addParams = new string[] { null, null };
            return await GetAllAsync(obj, addParams, UserMasterInfra.SPROC_ORDERENROLLEDUSERS);
        }

        public async Task<UserMaster> IsUserEnrolledByOrderCode(List<KeyValuePair<string, string>> keyValuePairs)
        {
            return await GetAsyncWithDynamicParam(keyValuePairs, UserMasterInfra.SPROC_ISUSERENROLLEDBYORDERCODE);
        }

        public async Task<UserMaster> GetCartCount(UserMaster obj)
        {
            string[] addParams = new string[] { BaseInfra.REQUESTERUSERID, UserMasterInfra.ISADMIN };
            return await GetAsync(obj, addParams, UserMasterInfra.SPROC_GETCARTCOUNT);
        }

        public Task<UserMaster> RunBRforAddeduser(UserMaster obj)
        {
            string[] addParams = new string[] { UserMasterInfra.ADDEDUSERID };
            return GetAsync(obj, addParams, UserMasterInfra.SPROC_RUN_BUSINESSRULE_FOR_USER_ADD);
        }

        public Task<UserMaster> RunBRforBulkImport(UserMaster obj)
        {
            string[] addParams = new string[] { UserMasterInfra.BULKIMPORTID };
            return GetAsync(obj, addParams, UserMasterInfra.SPROC_RUN_BUSINESSRULE_FOR_BULKIMPORT);
        }

        public Task<int> RemoveGroupAdmin(UserMaster obj)
        {
            string[] addParams = new string[] { UserMasterInfra.USERROLEID };
            return ExecuteNonQueryAsync(obj, addParams, UserMasterInfra.SPROC_REMOVEGROUPADMIN);
        }

        public async Task<IEnumerable<UserMaster>> GetAllUsersForTransfer(UserMaster obj)
        {
            string[] addParams = new string[] { BaseInfra.PAGEINDEX, BaseInfra.PAGESIZE, BaseInfra.SORTEXP, UserMasterInfra.FIRSTNAME, UserMasterInfra.EMAILADDRESS, UserMasterInfra.LASTNAME,
                UserMasterInfra.STATUS, UserMasterInfra.DEPARTMENTID, UserMasterInfra.JOBCODEID, UserMasterInfra.REQUESTERUSERID,
                UserMasterInfra.SEARCHONGROUPID, UserMasterInfra.GROUPID, UserMasterInfra.HIRINGDATEFROM, UserMasterInfra.HIRINGDATETO, UserMasterInfra.ROLECHANGEDATEFROM, UserMasterInfra.ROLECHANGEDATETO, UserMasterInfra.NOPAGING
                , UserMasterInfra.ISADMIN
            };
            return await GetAllAsync(obj, addParams, UserMasterInfra.SPROC_USERMASTERFORTRANSFER_LSTALL);
        }

        public async Task<int> TransferUser(UserMaster obj)
        {
            string[] addParams = new string[] { UserMasterInfra.USERID, UserMasterInfra.REMARK, UserMasterInfra.GROUPID, BaseInfra.REQUESTERUSERID, UserMasterInfra.MASTERLOGID };

            return await ExecuteNonQueryAsync(obj, addParams, UserMasterInfra.SPROC_TRANSFERUSER_UPS);
        }

        public async Task<int> TransferUserMaster(UserMaster obj)
        {
            string[] addParams = new string[] { UserMasterInfra.MASTERLOGID, UserMasterInfra.GROUPID, UserMasterInfra.REMARK, UserMasterInfra.TOTALRECORDS, BaseInfra.REQUESTERUSERID };

            return await ExecuteNonQueryAsync(obj, addParams, UserMasterInfra.SPROC_TRANSFERUSERMASTER_UPS);
        }

        public async Task<int> TrackSignOutTime(UserMaster obj)
        {
            string[] addParams = new string[] { UserMasterInfra.LOGINUNIQUEID };

            return await ExecuteNonQueryAsync(obj, addParams, UserMasterInfra.SPROC_TRACKSIGNOUTTIME);
        }

        public async Task<IEnumerable<UserMaster>> GetNotLoggedInUsers()
        {
            return await GetAllAsync(new UserMaster(), new string[] { }, UserMasterInfra.SPROC_GETNOTLOGGEDINUSERS);
        }

        public async Task<int> SaveWelcomeEmailSummary(UserMaster obj)
        {
            string[] addParams = new string[] { BaseInfra.REQUESTERUSERID, "TotalUsers", "TotalSuccessfull" };
            return await ExecuteNonQueryAsync(obj, addParams, UserMasterInfra.SPROC_WELCOMEEMAILSUMMARY_UPS);
        }

        public async Task<IEnumerable<UserMaster>> GetWelcomeEmailLog(UserMaster obj)
        {
            string[] addParams = new string[] { BaseInfra.REQUESTERUSERID };
            return await GetAllAsync(obj, addParams, UserMasterInfra.SPROC_WELCOMEEMAILSUMMARY_LSTALL);
        }


     
        public UserMaster GetFeaturesByUserRole(UserMaster obj)
        {
            IEnumerable<dynamic> mapItems = new List<dynamic>()
                {
                     new MapItem(typeof(UserMaster), "UserMaster"),
                    new MapItem(typeof(UserRole), "UserRole"),
                    new MapItem(typeof(FeatureMaster), "FeatureMaster")
                };

            string[] addParams = new string[] { UserMasterInfra.USERID, UserMasterInfra.UROLEID };
            dynamic dynamicObject = GetMultipleAsync(obj, addParams, UserMasterInfra.SPROC_FEATURELIST_BY_USERROLE, mapItems);

            var User = ((List<dynamic>)dynamicObject.UserMaster).Cast<UserMaster>().ToList()?.FirstOrDefault();
            var UserRoleList = ((List<dynamic>)dynamicObject.UserRole).Cast<UserRole>().ToList();
            var FeatureList = ((List<dynamic>)dynamicObject.FeatureMaster).Cast<FeatureMaster>().ToList();


            // UserMaster User = new UserMaster();
            User.Features = FeatureList;
            User.userRoles = UserRoleList;
            return User;

        }

        public async Task<int> AddEditSpecialUser(UserMaster obj)
        {
            string[] addParams = new string[] { UserMasterInfra.USERID, UserMasterInfra.FIRSTNAME, UserMasterInfra.LASTNAME,
            UserMasterInfra.EMAILADDRESS,UserMasterInfra.ROLEID,UserMasterInfra.PREVIOUSROLEID, UserMasterInfra.REGIONLIST,
            UserMasterInfra.COUNTRYLIST,UserMasterInfra.ISACCESSBREACHLOG,UserMasterInfra.REQUESTERUSERID
            };
            return await ExecuteNonQueryAsync(obj, addParams, UserMasterInfra.SPROC_SPECIALUSER_UPS);
        }

        public async Task<IEnumerable<UserMaster>> GetAllSpecialUsersAsync(UserMaster obj)
        {
            string[] addParams = new string[] { BaseInfra.PAGEINDEX, BaseInfra.PAGESIZE, BaseInfra.SORTEXP, UserMasterInfra.USERNAME, UserMasterInfra.EMAILADDRESS,
                UserMasterInfra.REGIONID, UserMasterInfra.COUNTRYID, UserMasterInfra.LOBID,UserMasterInfra.SUBLOBID, UserMasterInfra.USERTYPE, UserMasterInfra.REQUESTERUSERID
            };
            return await GetAllAsync(obj, addParams, UserMasterInfra.SPROC_SPECIALUSERMASTER_LSTALL);
        }

        public Task<UserMaster> GetSpecialUser(UserMaster obj)
        {
            string[] addParams = new string[] { UserMasterInfra.USERID };
            return GetAsync(obj, addParams, UserMasterInfra.SPROC_SPECIALUSER_SEL);
        }

        public Task<UserMaster> IsUserExist(string emailAddress)
        {
            UserMaster obj = new UserMaster() { EmailAddress = emailAddress };
            string[] addParams = new string[] { UserMasterInfra.EMAILADDRESS };
            return GetAsync(obj, addParams, UserMasterInfra.SPROC_ISUSEREXIST_SEL);
        }

        public Task<UserMaster> IsInUseCount(string emailAddress)
        {
            UserMaster obj = new UserMaster() { EmailAddress = emailAddress };
            string[] addParams = new string[] { UserMasterInfra.EMAILADDRESS };
            return GetAsync(obj, addParams, UserMasterInfra.SPROC_USERMASTER_ISINUSECOUNT);
        }

        public async Task<int> DeleteSpecialUser(UserMaster obj)
        {
            string[] addParams = new string[] { UserMasterInfra.USERID };
            return await ExecuteNonQueryAsync(obj, addParams, UserMasterInfra.SPROC_SPECIALUSER_DEL);
        }
    }
}
