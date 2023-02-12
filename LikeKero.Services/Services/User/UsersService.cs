using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LikeKero.Data.Constants;
using LikeKero.Domain;
using LikeKero.Domain.DomainInterfaces;
using LikeKero.Domain.Utility;
using LikeKero.Infra;
using LikeKero.Infra.BaseUri;
using LikeKero.Infra.EmailSender;
using LikeKero.Infra.Encryption;
using LikeKero.Infra.FileSystem;
using LikeKero.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace LikeKero.Services.Services
{
    public class UsersService : IUser
    {
        private IUserRepository _userRepository;
        private IFileUpload _fileUpload;
        private IUriService _uriService;
        private IEmailSender _emailSender;
        private IEmailSenderEntity _emailsenderentity;
        private IEmailSentLog _emailSentLog;
        private IOptions<FileSystemPath> _options = null;


        public UsersService(IUserRepository userRepository, IFileUpload fileUpload, IUriService uriService,
            IEmailSender emailSender, IEmailSenderEntity emailsenderentity, IEmailSentLog emailSentLog, IOptions<FileSystemPath> options
            )
            : base()
        {
            _userRepository = userRepository;
            _fileUpload = fileUpload;
            _uriService = uriService;
            _emailSender = emailSender;
            _emailsenderentity = emailsenderentity;
            _emailSentLog = emailSentLog;
            _options = options;
        }

        public Task<int> AddEditAsync(UserMaster obj)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(UserMaster obj)
        {
            throw new NotImplementedException();
        }


        public Task<IEnumerable<UserMaster>> GetAllAsync(UserMaster obj)
        {
            throw new NotImplementedException();
        }

        public async Task<UserMaster> GetAsync(UserMaster obj)
        {
            UserMaster objReturn = await _userRepository.GetUserData(obj);
            if (objReturn == null)
            {
                return new UserMaster();
            }
            if (string.IsNullOrEmpty(objReturn?.ProfilePic))
            {
                objReturn.ProfilePic = _uriService.GetBaseUri().ToString() + Convert.ToString(_options.Value.AfterDomain) + "/" + _uriService.GetDefaultProfilePic();
            }
            else
            {
                objReturn.ProfilePic = _uriService.GetBaseUri().ToString() + Convert.ToString(_options.Value.AfterDomain) + "/" + objReturn.ProfilePic;
            }
            return objReturn;
        }


        public UserMaster LoginAndGetFeatures(UserMaster obj)
        {
            obj.IsLogin = true;
            UserMaster returnObj = _userRepository.GetByEmailAsync(obj)?.Result;

            //Original Code
            //string hashedPassword = Cryptography.MD5Hash(obj?.Salt + returnObj?.Password);
            //if (hashedPassword != obj?.Password)
            //    return null;


            //extra line temp use
            string encryptPass = Cryptography.MD5Hash(obj.Password);
            encryptPass = Cryptography.MD5Hash(obj?.Salt + encryptPass);
            string hashedPassword = Cryptography.MD5Hash(obj?.Salt + returnObj?.Password);
            if (hashedPassword != encryptPass)
                return null;


            var response = _userRepository.LoginAndGetFeatures(obj);

            if (response.userRoles == null || response.userRoles.Count < 1)
            {
                response.IsAdmin = false;
            }
            else
            {
                response.IsAdmin = true;
            }
            response.LoginUniqueId = returnObj.LoginUniqueId;//Vikas - to track login details
            return response;
        }


        public UserMaster LoginAndGetFeaturesWithEmail(UserMaster obj)
        {


            var response = _userRepository.LoginAndGetFeatures(obj);

            if (response.userRoles == null || response.userRoles.Count < 1)
            {
                response.IsAdmin = false;
            }
            else
            {
                response.IsAdmin = true;
            }
            return response;
        }

        public Task<UserMaster> RunBRforBulkImport(UserMaster obj)
        {


            return _userRepository.RunBRforBulkImport(obj);


        }




        public async Task<string> AddEditUserAsync(UserMaster obj)
        {

            bool IsSendEmail = false;
            string randomPasswordForEmail = string.Empty;
            if (string.IsNullOrEmpty(obj.UserId))
            {
                IsSendEmail = true;
                obj.UserId = Utility.GetUniqueKeyWithPrefix("USR_", 8);
                string randomPassword = Utility.GetUniqueKeyWithPrefix("Pass", 8);
                randomPasswordForEmail = randomPassword;

                //ENCRYPT NEW PASSWORD AND UPDATE IN DATABASE
                randomPassword = Cryptography.MD5Hash(randomPassword);
                obj.Password = randomPassword;

            }

            UserMaster emailUserMaster = obj;

            string strUserID = obj.UserId;
            await _userRepository.AddEditAsync(obj);

            if (!string.IsNullOrEmpty(emailUserMaster.EmailAddress) && emailUserMaster.EmailAddress.Contains("@") && IsSendEmail)
            {
                //SEND EMAIL TO USER
                EmailSenderEntity emailconfig = new EmailSenderEntity();
                emailconfig.EmailType = EmailTypeInfra.WELCOME_EMAIL;
                emailconfig = await _emailsenderentity.GetAsync(emailconfig);
                emailconfig.EmailTo = obj.EmailAddress;
                emailUserMaster.Password = randomPasswordForEmail;
                await _emailSentLog.AddEmailEntry(emailconfig, emailUserMaster);

            }

            obj.AddedUserId = strUserID;
            _userRepository.RunBRforAddeduser(obj);

            return strUserID;
        }


        //public async Task<bool> UploadProfilePic(string strUserID, IFormFile formFile)
        //{
        //    if (!string.IsNullOrEmpty(strUserID) && formFile != null && formFile.Length > 0)
        //    {
        //        string ext = Path.GetExtension(formFile.FileName);
        //        string[] arrExtenstions = _options.Value.ProfilePicExtensions.Split(',');
        //        if (arrExtenstions.Contains(ext) == false)
        //            return false;

        //        string strProfilePicPath = await _fileUpload.UploadUserProfilePic(formFile, strUserID);

        //        var keyValuePairs = new List<KeyValuePair<string, string>>();
        //        keyValuePairs.Add(new KeyValuePair<string, string>(UserMasterInfra.USERID, strUserID));
        //        keyValuePairs.Add(new KeyValuePair<string, string>(UserMasterInfra.PROFILEPIC, strProfilePicPath));
        //        await _userRepository.UpdateProfilePic(keyValuePairs);
        //        return true;
        //    }

        //    return false;

        //}

        //public async Task<Stream> GetUserTemplateFile()
        //{
        //    return await _fileUpload.GetUserTemplateFile();
        //}


        public UserSearchModel GetLookupsForUserSearch()
        {
            var keyValuePairs = new List<KeyValuePair<string, string>>();
            keyValuePairs.Add(new KeyValuePair<string, string>(UserMasterInfra.LOOKUPTYPE, UserMasterInfra.USER_STATUS));

            return _userRepository.GetLookupsForUserSearch(keyValuePairs);
        }

        public UserMaster TestLogin(UserMaster obj)
        {
            return _userRepository.LoginAndGetFeatures(obj);
        }

        public async Task<IEnumerable<UserMaster>> GetAllUsersAsync(UserMaster obj)
        {
            return await _userRepository.GetAllUsersAsync(obj);
        }

        public async Task<IEnumerable<UserMaster>> ResolveBR(UserMaster obj)
        {
            return await _userRepository.ResolveBR(obj);
        }

        public async Task<IEnumerable<UserMaster>> ResolveBRExpired(UserMaster obj)
        {
            return await _userRepository.ResolveBRExpired(obj);
        }

        public async Task<IEnumerable<UserMaster>> ResolveBR_Assigned_User(UserMaster obj)
        {
            return await _userRepository.ResolveBR_Assigned_User(obj);
        }

        public async Task<int> SetNewPassword(UserMaster obj)
        {
            string randomPassword = Utility.CreateRandomPassword(8);
            obj = await GetByEmailAsync(obj);
            //SEND EMAIL TO USER
            if (!string.IsNullOrEmpty(obj.EmailAddress))
            {
                EmailSenderEntity emailconfig = new EmailSenderEntity();
                emailconfig.EmailType = EmailTypeInfra.FORGOT_PASSWORD_EMAIL;
                emailconfig = await _emailsenderentity.GetAsync(emailconfig);
                emailconfig.EmailTo = obj.EmailAddress;
                obj.Password = randomPassword;
                var emailSentLog = await _emailSender.SendInstantEmail(emailconfig, obj);

                await _emailSentLog.AddEditAsync(emailSentLog);

            }
            //ENCRYPT NEW PASSWORD AND UPDATE IN DATABASE
            randomPassword = Cryptography.MD5Hash(randomPassword);
            obj.Password = randomPassword;

            return await _userRepository.ForgotPassword(obj);
        }

        public Task<UserMaster> GetByEmailAsync(UserMaster obj)
        {
            return _userRepository.GetByEmailAsync(obj);
        }
        public Task<UserMaster> IsEmailInUseAsync(string EmailAddress, string UserId)
        {
            var keyValuePairs = new List<KeyValuePair<string, string>>();
            keyValuePairs.Add(new KeyValuePair<string, string>(UserMasterInfra.EMAILADDRESS, EmailAddress));
            keyValuePairs.Add(new KeyValuePair<string, string>(UserMasterInfra.USERID, UserId));
            return _userRepository.IsEmailInUseAsync(keyValuePairs);
        }

        public Task<UserMaster> IsLoginIDInUseAsync(string LoginID, string UserId)
        {
            var keyValuePairs = new List<KeyValuePair<string, string>>();
            keyValuePairs.Add(new KeyValuePair<string, string>(UserMasterInfra.LOGINID, LoginID));
            keyValuePairs.Add(new KeyValuePair<string, string>(UserMasterInfra.USERID, UserId));
            return _userRepository.IsLoginIdInUseAsync(keyValuePairs);
        }

        public Task<int> ActiveInActiveUser(UserMaster obj)
        {
            return _userRepository.ActiveInActiveUser(obj);
        }

        //public async Task<Stream> GetBulkImportUploadedFile(string BulkImportId, string BulkImportFileName)
        //{
        //    return await _fileUpload.GetBulkImportUploadedFile(BulkImportId, BulkImportFileName);
        //}

        public async Task<UserMaster> InserUserMasterViaBulk(UserMaster obj)
        {
            return await _userRepository.InserUserMasterViaBulk(obj);
        }

        public async Task<int> ChangePassword(UserMaster obj)
        {
            return await _userRepository.ChangePasswordAsync(obj);
        }

        public async Task<UserMaster> GetSelfUserInfo(UserMaster obj)
        {
            return await _userRepository.GetSelfUser(obj);
        }

        public async Task<Stream> GetUserExportData(UserMaster obj)
        {
            obj.NoPaging = true;
            var userList = await _userRepository.GetAllUsersAsync(obj);

            return await _fileUpload.ExportUserData(userList);
        }

        public async Task<IEnumerable<UserMaster>> ResolveBussinessAssignmentUser(UserMaster obj)
        {
            return await _userRepository.ResolveBussinessAssignmentUser(obj);
        }

        public async Task<IEnumerable<UserMaster>> GetUserRolesbyUserId(UserMaster obj)
        {
            var ListUserRoles = await _userRepository.GetUserRolesbyUserId(obj);

            //if(ListUserRoles != null)
            //{
            //    List<UserMaster> returnRoles = new List<UserMaster>();
            //    string[] ArrayRoles = ListUserRoles.ToList().Select(x => x.DisplayRole).Distinct().ToArray();
            //    foreach (string strDisplayRoles in ArrayRoles)
            //    {
            //        var GroupNames = ListUserRoles.Where(x => x.DisplayRole == strDisplayRoles).ToList();
            //        UserMaster ent = new UserMaster();
            //        ent.DisplayRole = strDisplayRoles;
            //        ent.GroupName = "";
            //        foreach (var Group in GroupNames)
            //        {
            //            ent.GroupName = ent.GroupName + Group.GroupName + ",";
            //        }
            //        ent.GroupName = ent.GroupName.TrimEnd(',');
            //        returnRoles.Add(ent);
            //    }

            //    return returnRoles;

            //}

            return ListUserRoles;
        }

        public async Task<IEnumerable<UserMaster>> UserListForEnrollment(UserMaster obj)
        {
            return await _userRepository.UserListForEnrollment(obj);
        }

        public async Task<IEnumerable<UserMaster>> OrderEnrolledUsersList(UserMaster obj)
        {
            return await _userRepository.OrderEnrolledUsersList(obj);
        }

        public async Task<UserMaster> IsUserEnrolledByOrderCode(string OrderCode, string RequesterUserId)
        {
            var keyValuePairs = new List<KeyValuePair<string, string>>();
            keyValuePairs.Add(new KeyValuePair<string, string>(BaseInfra.REQUESTERUSERID, RequesterUserId));
            keyValuePairs.Add(new KeyValuePair<string, string>(null, OrderCode));
            return await _userRepository.IsUserEnrolledByOrderCode(keyValuePairs);
        }

        public async Task<UserMaster> GetCartCount(UserMaster obj)
        {
            return await _userRepository.GetCartCount(obj);
        }

        public async Task<int> RemoveGroupAdmin(UserMaster obj)
        {
            return await _userRepository.RemoveGroupAdmin(obj);
        }

        public async Task<IEnumerable<UserMaster>> GetAllUsersForTransfer(UserMaster obj)
        {
            return await _userRepository.GetAllUsersForTransfer(obj);
        }

        public async Task<int> TransferUserMaster(UserMaster obj)
        {
            foreach (string singleUser in obj.SelectedUsers.Split(','))
            {
                if (string.IsNullOrEmpty(singleUser))
                    continue;
                obj.TotalRecords++;
            }
            return await _userRepository.TransferUserMaster(obj);
        }

        public async Task<int> TransferUser(UserMaster obj)
        {
            return await _userRepository.TransferUser(obj);
        }

        public async Task<int> TrackSignOutTime(UserMaster obj)
        {
            return await _userRepository.TrackSignOutTime(obj);
        }

        private async Task<bool> SendWelcomeEmail(UserMaster obj)
        {

            bool IsSendEmail = false;
            string randomPasswordForEmail = string.Empty;
            string randomPassword = Utility.GetUniqueKeyWithPrefix("Pass", 8);
            randomPasswordForEmail = randomPassword;

            //ENCRYPT NEW PASSWORD AND UPDATE IN DATABASE
            randomPassword = Cryptography.MD5Hash(randomPassword);
            obj.Password = randomPassword;
            obj.IsPasswordChanged = false;
            await _userRepository.ChangePasswordAsync(obj);
            UserMaster emailUserMaster = obj;

            if (!string.IsNullOrEmpty(emailUserMaster.EmailAddress))
            {
                //SEND EMAIL TO USER
                EmailSenderEntity emailconfig = new EmailSenderEntity();
                emailconfig.EmailType = EmailTypeInfra.WELCOME_EMAIL;
                emailconfig = await _emailsenderentity.GetAsync(emailconfig);
                emailconfig.EmailTo = obj.EmailAddress;
                emailUserMaster.Password = randomPasswordForEmail;
                await _emailSentLog.AddEmailEntry(emailconfig, emailUserMaster);

                IsSendEmail = true;
            }

            return IsSendEmail;
        }

        public async Task<UserMaster> SendWelcomeEmailToUsersNotLogIn(string RequesterUserId)
        {
            // Create procedure which returns all users not logged in
            // Only return UserId, EmailAddress, FirstName, LastName
            // int TotalNotLoggedInUsers = lst.Count
            // int TotalEmailSent = 0
            //Loop call SendWelcomeEmail()
            // If return true then TotalEmailSent++
            // int USersWithoutEmail = TotalNotLoggedInUsers - TotalEmailSent
            // Create above properties in UserMaster and dump in new table with todays date with 3 properties

            UserMaster obj = new UserMaster();
            IEnumerable<UserMaster> lstData = await _userRepository.GetNotLoggedInUsers();
            obj.TotalUsers = lstData.ToList().Count;

            foreach (UserMaster singleUser in lstData)
            {
                singleUser.RequesterUserId = RequesterUserId;
                bool res = await SendWelcomeEmail(singleUser);
                if (res == true)
                {
                    obj.TotalSuccessfull++;
                }
            }
            obj.RequesterUserId = RequesterUserId;
            await _userRepository.SaveWelcomeEmailSummary(obj);

            return obj;
        }

        public async Task<IEnumerable<UserMaster>> GetWelcomeEmailLog(UserMaster obj)
        {
            return await _userRepository.GetWelcomeEmailLog(obj);

        }


        //

        public UserMaster GetFeaturesByUserRole(UserMaster obj)
        {
            return _userRepository.GetFeaturesByUserRole(obj);
        }

        // Shahen => 15/12/21: UserRole and respective data
        public async Task<string> AddEditSpecialUser(UserMaster obj)
        {
            if (string.IsNullOrEmpty(obj.UserId))
            {
                //IsSendEmail = true;
                obj.UserId = Utility.GeneratorUniqueId("USR");               
            }            
            int i = await _userRepository.AddEditSpecialUser(obj);
            return i > 0 ? obj.UserId : string.Empty;
        }

        public async Task<IEnumerable<UserMaster>> GetAllSpecialUsersAsync(UserMaster userobj)
        {
            return await _userRepository.GetAllSpecialUsersAsync(userobj);
        }

        public async Task<UserMaster> GetSpecialUser(UserMaster obj)
        {
            return await _userRepository.GetSpecialUser(obj);
        }

        public async Task<UserMaster> IsUserExist(string emailAddress)
        {
            return await _userRepository.IsUserExist(emailAddress);
        }

        public async Task<UserMaster> IsInUseCount(string emailAddress)
        {
            return await _userRepository.IsInUseCount(emailAddress);
        }

        public async Task<int> DeleteSpecialUser(UserMaster userMaster)
        {
            return await _userRepository.DeleteSpecialUser(userMaster);
        }
    }
}
