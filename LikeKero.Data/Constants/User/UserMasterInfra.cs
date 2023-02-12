using System;
using System.Collections.Generic;
using System.Text;

namespace LikeKero.Data.Constants
{
    public class UserMasterInfra : BaseInfra
    {
        public static string SPROC_LOGIN_USER_BY_EMAIL_PASSWORD = "Sproc_Login_User_By_Email_Password";
        public static string SPROC_USERMASTER_LSTALL = "sproc_UserMaster_lstAll";
        public static string SPROC_USERBYEMAIL = "sproc_UserByEmail";
        public static string SPROC_SETNEWPASSWORD = "sproc_SetNewPassword";
        public static string SPROC_MANAGEUSERS_LOOKUP = "sproc_ManageUsers_LookUp";
        public static string SPROC_USER_ISEMAILINUSE = "sproc_User_IsEmailInUse";
        public static string SPROC_ADDEDIT_USERMASTER = "sproc_AddEdit_UserMaster";
        public static string SPROC_USER_ISLOGINIDINUSE = "sproc_User_IsloginIdInUse";
        public static string SPROC_ACTIVEINACTIVEUSER = "sproc_ActiveInActiveUser";
        public static string SPROC_UPDATE_USERPROFILEPIC = "sproc_Update_UserProfilePic";
        public const string SPROC_GETUSER_BY_USERID = "sproc_GetUser_by_UserId";
        public const string SPROC_CHECKBULKIMPORTVALIDATION_USERS = "sproc_CheckBulkImportValidation_Users";
        public const string SPROC_GETSELFUSERINFO = "sproc_GetSelfUserInfo";

        public const string SPROC_RESOLVE_BR = "sproc_Resolve_BR";
        public const string SPROC_RESOLVEBR_AFTERASSIGNMENT = "sproc_ResolveBR_AfterAssignment";
        public const string SPROC_RESOLVE_BUSINESSASSIGNMENT = "sproc_Resolve_BusinessAssignment";
        public const string SPROC_SHOWROLES_FOR_USER_BYUSERID = "Sproc_ShowRoles_For_User_byUserId";

        public const string SPROC_RUN_BUSINESSRULE_FOR_USER_ADD = "sproc_Run_BusinessRule_For_User_ADD";
        public const string SPROC_RUN_BUSINESSRULE_FOR_BULKIMPORT = "sproc_Run_BusinessRule_For_BulkImport";

        public const string SPROC_RESOLVE_BR_EXPIRED = "sproc_Resolve_BR_Expired";




        public static string ADDEDUSERID = "AddedUserId";
        public static string EMAILADDRESS = "EmailAddress";
        public const string PASSWORD = "Password";
        public const string FIRSTNAME = "FirstName";
        public const string LASTNAME = "LastName";
        public const string USERNAME = "UserName";
        public const string PROFILEIMAGEPATH = "ProfileImagePath";


        public static string REGIONID = "RegionId";
        public static string COUNTRYID = "CountryId";
        public static string LOBID = "LOBId";
        public static string SUBLOBID = "SubLOBId";
        public static string USERTYPE = "UserType";
        
        public static string REGIONLIST = "RegionList";
        public static string COUNTRYLIST = "CountryList";
        public static string ISACCESSBREACHLOG = "IsAccessBreachLog";
        public static string PREVIOUSUSERTYPE = "PreviousUserType";
        public const string ISACTIVE = "IsActive";
        public const string ROLEID = "RoleID";
        public const string PREVIOUSROLEID = "PreviousRoleID";



        public const string DEPARTMENTID = "DepartmentId";
        public const string JOBCODEID = "JobCodeId";
        public const string SEARCHONGROUPID = "SearchOnGroupId";
        public const string GROUPID = "GroupId";
        public const string STATUS = "Status";
        public const string LOOKUPTYPE = "LookUpType";
        public const string USER_STATUS = "USER_STATUS";
        public const string USERID = "UserId";
        public const string LOGINID = "LoginId";
        public const string BULKIMPORTID = "BulkImportId";
        



        public static string CONTACTNUMBER = "ContactNumber";
        public static string HIRINGDATE = "HiringDate";
        public static string ZIPCODE = "ZipCode";
        public static string ROLECHANGEDATE = "RoleChangeDate";
        public static string TIMEZONEID = "TimeZoneId";
        public static string PROFILEPIC = "ProfilePic";
        public static string STATEID = "StateId";
        public static string CITYID = "CityId";

        public const string HIRINGDATEFROM = "HiringDateFrom";
        public const string HIRINGDATETO = "HiringDateTo";
        public const string ROLECHANGEDATEFROM = "RoleChangeDateFrom";
        public const string ROLECHANGEDATETO = "RoleChangeDateTo";

        public const string ISPASSWORDCHANGED = "IsPasswordChanged";

        public const string SPROC_CHANGEPASSWORD = "sproc_ChangePassword";
        public const string NOPAGING = "NoPaging";

        public static string BRID = "BRId";
        public static string BRACTIVITYID = "BRActivityId";
        public static string ISADMIN = "IsAdmin";

        public static string ISLOGIN = "IsLogin";



        //
        public static string UROLEID = "URoleId";       

       


        public const string SPROC_SPECIALUSER_UPS = "sproc_SpecialUser_ups";
        public const string SPROC_SPECIALUSER_SEL = "sproc_SpecialUser_sel";
        public const string SPROC_SPECIALUSERMASTER_LSTALL = "sproc_SpecialUserMaster_lstAll";
        public const string SPROC_ISUSEREXIST_SEL = "sproc_IsUserExist_sel";
        public const string SPROC_USERMASTER_ISINUSECOUNT = "sproc_UserMaster_IsInUseCount";
        public const string SPROC_SPECIALUSER_DEL = "sproc_SpecialUser_del";






        public const string SPROC_USERLISTFORENROLLMENT = "sproc_UserListForEnrollment";
        public const string SPROC_ORDERENROLLEDUSERS = "sproc_OrderEnrolledUsers";
        public const string SPROC_JOBROLE_GETALL = "sproc_JobRole_getAll";
        public const string SPROC_ISUSERENROLLEDBYORDERCODE = "sproc_IsUserEnrolledByOrderCode";
        public const string SPROC_GETCARTCOUNT = "sproc_GetCartCount";

        public const string SPROC_REMOVEGROUPADMIN = "sproc_RemoveGroupAdmin";
        public const string USERROLEID = "UserRoleId";

        public const string SPROC_GROUPNAMES_TOMAKEADMIN = "sproc_GroupNames_ToMakeAdmin";
        public const string SPROC_USERMASTERFORTRANSFER_LSTALL = "sproc_UserMasterForTransfer_lstAll";

        public const string SPROC_TRANSFERUSERMASTER_UPS = "sproc_TransferUserMaster_ups";
        public const string SPROC_TRANSFERUSER_UPS = "sproc_TransferUser_ups";
        public const string REMARK = "Remark";
        public const string MASTERLOGID = "MasterLogId";
        public const string TOTALRECORDS = "TotalRecords";
        public const string LOGINUNIQUEID = "LoginUniqueId";
        public const string SPROC_TRACKSIGNOUTTIME = "sproc_TrackSignOutTime";
        public const string SPROC_GETNOTLOGGEDINUSERS = "sproc_GetNotLoggedInUsers";
        public const string SPROC_WELCOMEEMAILSUMMARY_UPS = "sproc_WelcomeEmailSummary_ups";
        public const string SPROC_WELCOMEEMAILSUMMARY_LSTALL = "sproc_WelcomeEmailSummary_lstAll";


        //
        public const string SPROC_FEATURELIST_BY_USERROLE = "Sproc_FeatureList_By_UserRole";


    }
}
