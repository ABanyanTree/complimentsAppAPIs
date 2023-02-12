namespace LikeKero.Api.ApiPath
{
    public static class ApiRoutes
    {


        public const string Base = "api";

        public static class LOBChapterMaster
        {
            public const string AddEditLOBChapter = Base + "/lobchapter/addeditlobchapter";
            public const string GetLOBChapter = Base + "/lobchapter/getlobchapter";
            public const string DeleteLOBChapter = Base + "/lobchapter/deletelobchapter";
            public const string GetAllLOBChapterList = Base + "/lobchapter/getalllobchapterlist";
            public const string IsLOBChapterNameInUse = Base + "/lobchapter/islobchapternameinuse";
             public const string GetAllLOBChapter = Base + "/lobchapter/getalllobchapter";           
            public const string IsInUseCount = Base + "/lobchapter/isinusecount";
        }

        public static class SubLOBMaster
        {
            public const string AddEditSubLOB = Base + "/sublob/addeditsublob";
            public const string GetSubLOB = Base + "/sublob/getsublob";
            public const string DeleteSubLOB = Base + "/sublob/deletesublob";
            public const string GetAllSubLOBList = Base + "/sublob/getallsubloblist";
            public const string IsSubLOBNameInUse = Base + "/sublob/issublobnameinuse";
            public const string GetAllSubLOB = Base + "/sublob/getallsublob";
            public const string IsInUseCount = Base + "/sublob/isinusecount";
        }
        public static class LOBMaster
        {
            public const string AddEditLOB = Base + "/lob/addeditlob";
            public const string GetLOB = Base + "/lob/getlob";
            public const string DeleteLOB = Base + "/lob/deletelob";
            public const string GetAllLOBList = Base + "/lob/getallloblist";
            public const string IsLOBNameInUse = Base + "/lob/islobnameinuse";
            public const string GetAllLOB = Base + "/lob/getalllob";           
            public const string IsInUseCount = Base + "/lob/isinusecount";
        }

        public static class SegmentMaster
        {
            public const string AddEditSegment = Base + "/segment/addeditsegment";
            public const string GetSegment = Base + "/segment/getsegment";
            public const string DeleteSegment = Base + "/segment/deletesegment";
            public const string GetAllSegmentList = Base + "/segment/getallsegmentlist";
            public const string IsSegmentNameInUse = Base + "/segment/issegmentnameinuse";
            // public const string GetAllSegment = Base + "/segment/getallsegment";
            //public const string GetAllSegmentCountry = Base + "/segment/getallsegmentcountry";
            public const string IsInUseCount = Base + "/segment/isinusecount";
        }

        public static class CountryMaster
        {
            public const string AddEditCountry = Base + "/country/addeditcountry";
            public const string GetCountry = Base + "/country/getcountry";
            public const string DeleteCountry = Base + "/country/deletecountry";
            public const string GetAllCountryList = Base + "/country/getallcountrylist";
            public const string IsCountryNameInUse = Base + "/country/iscountrynameinuse";
            public const string GetAllCountry = Base + "/country/getallcountry";
            public const string IsInUseCount = Base + "/country/isinusecount";
        }


        public static class RegionMaster
        {
            public const string AddEditRegion = Base + "/region/addeditregion";
            public const string GetRegion = Base + "/region/getregion";
            public const string DeleteRegion = Base + "/region/deleteregion";
            public const string GetAllRegionList = Base + "/region/getallregionlist";           
            public const string IsRegionNameInUse = Base + "/region/isregionnameinuse";
            public const string GetAllRegion = Base + "/region/getallregion";
            public const string IsInUseCount = Base + "/region/isinusecount";
        }


        public static class TestCustomer
        {          
            public const string AddEditTestCustomer = Base + "/testcutomer/addedittestcustomer";
            public const string GetTestCustomer = Base + "/testcustomer/gettestcustomer";
            public const string DeleteTestCustomer = Base + "/testcustomer/deletetestcustomer";
            public const string GetAllTestCustomer = Base + "/testcustomer/getalltestcustomer";
            public const string UploadTestCustomerFile = Base + "/testcustomer/uploadtestcustomerfile";
        }


        public static class User
        {
            public const string GetAllADUser = Base + "/user/getalladuser";
            public const string GetAllADUserList = Base + "/user/getalladuserlist";
            public const string GetAllSpecialUserList = Base + "/user/getallspecialuserlist";
            public const string AddEditSpecialUser = Base + "/user/addeditspecialuser";
            public const string GetSpecialUser = Base + "/user/getspecialuser";
            public const string IsUserExist = Base + "/user/isuserexist";
            public const string IsInUseCount = Base + "/user/isinusecount";
            public const string DeleteSpecialUser = Base + "/user/deletespecialuser";




            public const string GetSalt = Base + "/user/getsalt";
            public const string Login = Base + "/user/login";
            public const string GetUser = Base + "/user/getuser";
            public const string TestLogin = Base + "/user/testlogin";
            public const string UserList = Base + "/user/userlist";
            public const string ForgotPassword = Base + "/user/forgotpassword";
            public const string GetByEmail = Base + "/user/getbyemail";
            public const string GetLookUps = Base + "/user/getlookups";
            public const string IsEmailInUse = Base + "/user/isemailinuse";
            public const string AddEditUser = Base + "/user/addedituser";

            //public const string UploadUserProfilePic = Base + "/user/uploaduserprofilepic";

            public const string IsLoginIDInUse = Base + "/user/isloginidinuse";
            public const string ActiveInActiveUser = Base + "/user/activeinactiveuser";
            public const string GetUserData = Base + "/user/getuserdata";
           // public const string GetUserTemplateExcel = Base + "/user/getusertemplateexcel";
            //public const string UploadUserImport = Base + "/user/uploaduserimport";
            //public const string BulkImportListAll = Base + "/user/bulkimportlistall";
            //public const string CheckOIG = Base + "/user/checkoig";
            //public const string GetBulkImportUploadedFile = Base + "/user/getbulkimportuploadedfile";
            //public const string BulkImportLogs = Base + "/user/bulkimportlogs";
            public const string ChangePassword = Base + "/user/changepassword";
            //public const string GetBulkImportLog = Base + "/user/getbulkimportlog";
            public const string ExportUserData = Base + "/user/exportuserdata";
            //public const string SaveOIGUsers = Base + "/user/saveoigusers";

            //public const string BulkImportMasterRecord = Base + "/user/bulkimportmasterrecord";

            public const string GetSelfUserInfo = Base + "/user/getselfuserinfo";
            public const string Refresh = Base + "/user/refresh";
            public const string GetRoleListByUserId = Base + "/user/getrolelistbyuserid";
            //public const string GetJobRoles = Base + "/user/getjobroles";
            //public const string IsUserEnrolledByOrderCode = Base + "/user/isuserenrolledbyordercode";
            //public const string IsOrderCodeExists = Base + "/user/isordercodeexists";

            //public const string RemoveGroupAdmin = Base + "/user/removegroupadmin";

           // public const string UserListForTransfer = Base + "/user/userlistfortransfer";
            //public const string TransferUsers = Base + "/user/transferusers";
            //public const string TransferUserMaster = Base + "/user/transferusermaster";
            //public const string TransferLog = Base + "/user/transferlog";
            //public const string TransferLogDetails = Base + "/user/transferlogdetails";
            //public const string TransferLogMasterRecord = Base + "/user/transferlogmasterrecord";
            //public const string ExportUserTransferData = Base + "/user/exportusertransferdata";
           // public const string UserListForManualCompletion = Base + "/user/userlistformanualcompletion";
            public const string TrackSignOutTime = Base + "/user/tracksignouttime";
           // public const string WelcomeEmailForNotLoggedInUsers = Base + "/user/welcomeemailfornotloggedinusers";
           // public const string WelcomeEmailLog = Base + "/user/welcomeemaillog";

            //

            public const string GetFeaturesByUserRole = Base + "/user/getfeaturesbyuserrole";





        }

        //public static class EntityName
        //{
        //    public const string EntityList = Base + "/entity/entitylist";
        //    public const string EntityListForHierarchyAndRole = Base + "/entity/entitylistforhierarchyandrole";
        //    public const string GetAllAssociationForNCPDP_DropDown = Base + "/entity/getallassociationforncpdp_dropdown";
        //    public const string GetAllNCPDP = Base + "/entity/getallncpdp";
        //    public const string GETALLNCPDPREPORT = Base + "/entity/getallncpdpreport";
        //    public const string GetAllAdminNCPDP = Base + "/entity/getalladminncpdp";
        //    public const string DeleteNCPDPAdmin = Base + "/entity/deletencpdpadmin";
        //    public const string GetNCPDPByID = Base + "/entity/getncpdpbyid";
        //    public const string AddEditNCPDP = Base + "/entity/addeditncpdp";

        //    public const string GetAllNCPDPAdmin = Base + "/entity/getallncpdpadmin";

        //    public const string EntityListToMakeAdmin = Base + "/entity/entitylisttomakeadmin";
        //    public const string NCPDPQuickLookReport = Base + "/entity/ncpdpquicklookreport";
        //    public const string GroupAdmins = Base + "/entity/groupadmins";
        //    public const string GetGroupRecord = Base + "/entity/getgrouprecord";
        //    public const string GroupListWithDescr = Base + "/entity/grouplistwithdescr";



        //}

        public static class StateMasterRoutes
        {
            public const string CountryStateList = Base + "/state/countrystatelist";
            public const string StateCitiesList = Base + "/state/statecitieslist";
            public const string RSATest = Base + "/city/rsatest";
            public const string DecryptDBEncrypted = Base + "/city/decryptdbencrypted";


        }

        //public static class JobRoleMasterRoutes
        //{
        //    public const string JobRoleList = Base + "/jobrole/jobrolelist";
        //}

    

        public static class ErrorLogRoutes
        {
            public const string ErrorLogList = Base + "/errorlog/errorloglist";
            public const string GetErrorLog = Base + "/errorlog/geterrorlog";
        }

        public static class EmailLogRoutes
        {
            public const string EmailLogList = Base + "/emaillog/emailloglist";
            public const string GetEmailLog = Base + "/emaillog/getemaillog";
        }

        //public static class HostedService
        //{

        //    public const string SendPendingEmails = Base + "/hostedservice/sendpendingemails";
        //    public const string BulkImportPending = Base + "/hostedservice/bulkimportpending";
        //    public const string RunBRRUles = Base + "/hostedservice/runbrrules";
        //    public const string BulkImportOIGFiles = Base + "/hostedservice/bulkimportoigfiles";
        //    public const string BulkImportVendorFiles = Base + "/hostedservice/bulkimportvendorfiles";
        //    public const string ProcessPendingBusinessRuleFiles = Base + "/hostedservice/processpendingbusinessrulefiles";
        //    public const string MilindTestOIG = Base + "/hostedservice/milindtestoig";


        //}

        public static class Lookup
        {
            public const string GetLookupByType = Base + "/lookup/getlookupbytype";

        }

        

        public static class SystemRole
        {
            public const string GetAllRoles = Base + "/systemrole/getallroles";
            public const string SearchForAssignment = Base + "/systemrole/searchforassignment";
            public const string GroupAdminUsers = Base + "/systemrole/groupadminusers";
            public const string ActiveInActiveUserRole = Base + "/systemrole/activeinactiveuserrole";
            public const string AssignRole = Base + "/systemrole/assignrole";
            public const string DeleteUserSystemRole = Base + "/systemrole/deleteusersystemrole";
        }


        public static class FeatureMasterRoutes
        {
            public const string GetAdminFeatures = Base + "/feature/getadminfeatures";
            public const string SaveAdminRights = Base + "/feature/saveadminrights";

        }

   

        public static class DashboardRoute
        {
            //public const string UserSummary = Base + "/dashboard/usersummary";
            //public const string CourseAssignmentAndCompletion = Base + "/dashboard/courseassignmentandcompletion";
            //public const string MonthlyEnrollmentAndCompletion = Base + "/dashboard/monthlyenrollmentandcompletion";
            //public const string ACPEClaimStatus = Base + "/dashboard/acpeclaimstatus";
            //public const string GroupUserCount = Base + "/dashboard/groupusercount";
            //public const string GetActiveNCPDPByMonth = Base + "/dashboard/getactivencpdpbymonth";
        }

    
    }
}


