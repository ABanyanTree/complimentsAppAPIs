using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LikeKero.Api.ApiPath;
using LikeKero.Api.Filters;
using LikeKero.Api.options;
using LikeKero.Contract.Requests;
using LikeKero.Contract.Requests.User;
using LikeKero.Contract.Responses;
using LikeKero.Contract.Responses.Feature;
using LikeKero.Contract.Responses.User;
using LikeKero.Domain;
using LikeKero.Infra;
using LikeKero.Infra.Encryption;
using LikeKero.Infra.FileSystem;
using LikeKero.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LikeKero.Api.Controllers
{

    public class UserController : Controller
    {
        private readonly IUser _user;
        private readonly IMapper _mapper;
        private readonly JwtSettings _jwtSettings;
        //private readonly IBulkImportMaster _bulkImportMaster;
        //private readonly IBulkImportMasterLogs _bulkImportMasterLogs;
        private readonly IOptions<FileSystemPath> _options;
        //private readonly IOIGData oIGData;
        private readonly TokenValidationParameters _TokenValidationParameters = null;
        private readonly IRefreshTokenService _iRefreshTokenService;
        //private readonly IJobRoleMaster iJobRoleMaster;
        //private readonly ICourseMaster iCourseMaster = null;
        //private readonly IUserTransferLog iUserTransferLog = null;
        private readonly IADUserService _ADUserService;
       // private readonly IRegionMaster _RegionMasterService;
        private readonly ICountryMaster _CountryMasterService;


        public UserController(IUser user, IMapper mapper, JwtSettings jwtSettings
            , IOptions<FileSystemPath> options, TokenValidationParameters TokenValidationParameters,
            IRefreshTokenService iRefreshTokenService, IADUserService ADUserService,  ICountryMaster CountryMaster)
        {
            _user = user;
            _mapper = mapper;
            _jwtSettings = jwtSettings;
            // _bulkImportMaster = IBulkImportMaster;
            _options = options;
            //  oIGData = _IOIGData;
            //  _bulkImportMasterLogs = IBulkImportMasterLogs;
            _TokenValidationParameters = TokenValidationParameters;
            _iRefreshTokenService = iRefreshTokenService;
            //  iJobRoleMaster = IJobRoleMaster;
            //  iCourseMaster = ICourseMaster;
            // iUserTransferLog = IUserTransferLog;
            _ADUserService = ADUserService;
            //_RegionMasterService = RegionMaster;
            _CountryMasterService = CountryMaster;
        }

        #region -- Search AD User to Add into System --

        [HttpGet(ApiRoutes.User.GetAllADUserList)]
        [ProducesResponseType(typeof(ADUserListResponse), statusCode: 200)]
        [CustomAuthorizeAttribute(FeatureId = FeatureMasterInfra.FEATURE_GETALLUSERS)]
        public async Task<IActionResult> GetAllADUserList(ADUserListRequest request)
        {
            var requestObj = _mapper.Map<ADUser>(request);
            var responseObj = await _ADUserService.GetAllAsync(requestObj);

            var resObj = _mapper.Map<List<ADUserListResponse>>(responseObj);

            if (resObj == null)
                return Ok(new List<ADUserListResponse>());

            return Ok(responseObj);
        }

        [HttpGet(ApiRoutes.User.GetAllADUser)]
        [ProducesResponseType(typeof(ADUserListResponse), statusCode: 200)]
        [CustomAuthorizeAttribute(FeatureId = FeatureMasterInfra.FEATURE_GETALLUSERS)]
        public async Task<IActionResult> GetAllADUser()
        {
            var responseObj = await _ADUserService.GetAllADUser();
            var resObj = _mapper.Map<List<ADUserListResponse>>(responseObj);
            if (resObj == null)
                return Ok(new List<ADUserListResponse>());

            return Ok(responseObj);
        }

        #endregion



        #region -- User Master --

        /// <summary>
        /// Add Edit Special user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost(ApiRoutes.User.AddEditSpecialUser), DisableRequestSizeLimit]
        [ProducesResponseType(typeof(string), statusCode: 200)]
        [ProducesResponseType(typeof(ErrorResponse), statusCode: 401)]
        [ProducesResponseType(typeof(ErrorResponse), statusCode: 400)]
        [CustomAuthorizeAttribute(FeatureId = FeatureMasterInfra.FEATURE_ADDEDITUSER)]
        public async Task<IActionResult> AddEditSpecialUser([FromBody] CreateSpecialUserRequest request)
        {
            var userobj = _mapper.Map<UserMaster>(request);
            var userid = await _user.AddEditSpecialUser(userobj);
            return Ok(userid);
        }

        /// <summary>
        /// Manage Special User
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet(ApiRoutes.User.GetAllSpecialUserList)]
        [ProducesResponseType(typeof(List<SpecialUserResponse>), statusCode: 200)]
        [CustomAuthorizeAttribute(FeatureId = FeatureMasterInfra.FEATURE_GETALLUSERS)]
        public async Task<IActionResult> GetAllSpecialUserList(SpecialUserRequest request)
        {
            var userobj = _mapper.Map<UserMaster>(request);
            if (string.IsNullOrEmpty(userobj.RequesterUserId))
            {
                return ReturnErrorIfUserIDIsEmpty("RequesterUserId");
            }

            var UserList = await _user.GetAllSpecialUsersAsync(userobj);

            var responseObj = _mapper.Map<List<SpecialUserResponse>>(UserList);
            return Ok(responseObj);
        }

        /// <summary>
        /// Get Single Users
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpGet(ApiRoutes.User.GetSpecialUser)]
        [ProducesResponseType(typeof(SpecialUserResponse), statusCode: 200)]
        [CustomAuthorizeAttribute(FeatureId = FeatureMasterInfra.FEATURE_GETALLUSERS)]
        public async Task<IActionResult> GetSpecialUser([FromQuery] string UserId)
        {
            var objResponse = await _user.GetSpecialUser(new UserMaster() { UserId = UserId });
            var responseObj = _mapper.Map<SpecialUserResponse>(objResponse);

            //if (!string.IsNullOrEmpty(responseObj.UserId))
            //{
            //    responseObj.RegionDataList = new List<GetRegionResponse>();
            //    responseObj.CountryDataList = new List<GetCountryResponse>();
            //    //All regions
            //    if (!string.IsNullOrEmpty(responseObj.RegionList))
            //    {
            //        var regionResponseObj = await _RegionMasterService.GetAllRegionByIDs(new RegionMaster() { RegionID = responseObj.RegionList });
            //        var regionResponse = _mapper.Map<List<GetRegionResponse>>(regionResponseObj);
            //        if (regionResponse.ToList().Count > 0)
            //        {
            //            responseObj.RegionDataList.AddRange(regionResponse);
            //        }
            //    }

            //    if (!string.IsNullOrEmpty(responseObj.CountryList))
            //    {
            //        var countryResponseObj = await _CountryMasterService.GetAllCountryByIDs(new CountryMaster() { CountryID = responseObj.CountryList });
            //        var countryResponse = _mapper.Map<List<GetCountryResponse>>(countryResponseObj);
            //        if (countryResponse.ToList().Count > 0)
            //        {
            //            responseObj.CountryDataList.AddRange(countryResponse);
            //        }
            //    }
            //}

            return Ok(responseObj);
        }

        /// <summary>
        /// Check if User is already exist or not
        /// </summary>
        /// <param name="EmailAddress"></param>
        /// <returns></returns>
        [HttpGet(ApiRoutes.User.IsUserExist)]
        [ProducesResponseType(typeof(SpecialUserResponse), statusCode: 200)]
        [CustomAuthorizeAttribute(FeatureId = FeatureMasterInfra.FEATURE_ADDEDITUSER)]
        public async Task<IActionResult> IsUserExist([FromQuery] string EmailAddress)
        {
            if (string.IsNullOrEmpty(EmailAddress))
            {
                return ReturnErrorIfUserIDIsEmpty("EmailAddress");
            }

            var user = await _user.IsUserExist(EmailAddress);
            if (user == null)
            {
                return Json(false);
            }
            else
            {
                return Json(true);
            }

        }

        /// <summary>
        /// Check if User is assigned to somewhere else like any logs
        /// </summary>
        /// <param name="EmailAddress"></param>
        /// <returns></returns>
        [HttpGet(ApiRoutes.User.IsInUseCount)]
        [ProducesResponseType(typeof(SpecialUserResponse), statusCode: 200)]
        [CustomAuthorizeAttribute(FeatureId = FeatureMasterInfra.FEATURE_ADDEDITUSER)]
        public async Task<IActionResult> IsInUseCount([FromQuery] string EmailAddress)
        {
            var objResponse = await _user.IsInUseCount(EmailAddress);
            return Ok(objResponse.TotalCount);
        }

        /// <summary>
        /// Delete special user
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpDelete(ApiRoutes.User.DeleteSpecialUser)]
        [ProducesResponseType(typeof(SpecialUserResponse), statusCode: 200)]
        [CustomAuthorizeAttribute(FeatureId = FeatureMasterInfra.FEATURE_ADDEDITUSER)]
        public async Task<IActionResult> DeleteSpecialUser([FromQuery] string UserId)
        {
            var objResponse = await _user.DeleteSpecialUser(new UserMaster { UserId = UserId });
            return Ok(objResponse);
        }


        #endregion









        /// <summary>
        /// Get Salt for Password ENcryption
        /// </summary>       
        /// <returns>Salt as String</returns>
        /// <response code="200">Salt as String</response>
        /// <response code="400">Unable to generate Salt</response>      
        [HttpGet(ApiRoutes.User.GetSalt)]
        [ProducesResponseType(typeof(string), statusCode: 200)]
        [ProducesResponseType(typeof(ErrorResponse), statusCode: 400)]
        public IActionResult GetSalt()
        {
            DateTime startTime = DateTime.Now;
            var salt = Cryptography.CreateSalt();
            DateTime endTime = DateTime.Now;

            _iRefreshTokenService.SaveSaltTime(startTime, endTime, "GetSalt");

            return Ok(salt);
        }

        /// <summary>
        /// Login By Email and Password
        /// </summary>
        /// <param name="request"></param>
        /// <returns>User Object with Token</returns>
        /// <response code="200">User Object with Token</response>
        /// <response code="400">Unable to Login due to InValid Credentials</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost(ApiRoutes.User.Login)]
        [ProducesResponseType(typeof(AuthSuccessResponse), statusCode: 200)]
        [ProducesResponseType(typeof(ErrorResponse), statusCode: 400)]
        public IActionResult Login([FromBody] UserLoginRequest request)
        {

            DateTime startTime = DateTime.Now;
            var userobj = _mapper.Map<UserMaster>(request);
            var user = _user.LoginAndGetFeatures(userobj);

            if (user != null && !string.IsNullOrEmpty(user.UserId))
            {
                user = GenerateToken(user);
                var responseObj = _mapper.Map<AuthSuccessResponse>(user);
                var responseFeaturesObj = _mapper.Map<List<FeatureMasterResponse>>(user.Features);
                var responseRolesObj = _mapper.Map<List<UserRoleResponse>>(user.userRoles);


                responseObj.UserFeatures = responseFeaturesObj;
                responseObj.userRoles = responseRolesObj;
                responseObj.IsSuccess = true;

                UserMaster objCart = new UserMaster()
                {
                    RequesterUserId = responseObj.UserId,
                    IsAdmin = true
                };
                //objCart = _user.GetCartCount(objCart).Result;
                //responseObj.AdminCartCount = objCart.TotalCartItems;

                objCart = new UserMaster()
                {
                    RequesterUserId = responseObj.UserId,
                    IsAdmin = false
                };
                //objCart = _user.GetCartCount(objCart).Result;
                //responseObj.UserCartCount = objCart.TotalCartItems;

                DateTime endTime = DateTime.Now;
                _iRefreshTokenService.SaveSaltTime(startTime, endTime, "Login");

                return Ok(responseObj);
            }

            ErrorModel errorModel = new ErrorModel();
            errorModel.FieldName = "Login Failed";
            errorModel.Message = "Invalid Username or Password";
            ErrorResponse errorResponse = new ErrorResponse();
            errorResponse.Errors = new List<ErrorModel>();
            errorResponse.Errors.Add(errorModel);

            return BadRequest(errorResponse);
        }

        /// <summary>
        /// Get All Users
        /// </summary>
        /// <param name="request"></param>
        /// <returns>User List based on Search Criteria</returns>
        /// <response code="200">User List</response>         
        /// <response code="401">Unauthorized</response>
        [HttpPost(ApiRoutes.User.UserList)]
        [ProducesResponseType(typeof(List<UserListResponse>), statusCode: 200)]
        [CustomAuthorizeAttribute(FeatureId = FeatureMasterInfra.FEATURE_GETALLUSERS)]
        public async Task<IActionResult> UserList([FromBody] UserListRequest request)
        {
            var userobj = _mapper.Map<UserMaster>(request);
            if (string.IsNullOrEmpty(userobj.RequesterUserId))
            {
                return ReturnErrorIfUserIDIsEmpty("RequesterUserId");
            }

            var UserList = await _user.GetAllUsersAsync(userobj);

            var responseObj = _mapper.Map<List<UserListResponse>>(UserList);
            return Ok(responseObj);

        }

        /// <summary>
        /// Get All Users
        /// </summary>
        /// <param name="EmailAddress"></param>
        /// <param name="UserID"></param>   
        /// <returns>User List based on Search Criteria</returns>
        /// <response code="200">True if email doesn not exists else error message</response>         
        /// <response code="401">Unauthorized</response>
        [HttpGet(ApiRoutes.User.IsEmailInUse)]
        [ProducesResponseType(typeof(List<UserListResponse>), statusCode: 200)]
        [CustomAuthorizeAttribute(FeatureId = FeatureMasterInfra.FEATURE_ADDEDITUSER)]
        public async Task<IActionResult> IsEmailInUse([FromQuery] string EmailAddress, string UserID = "")
        {
            if (string.IsNullOrEmpty(EmailAddress))
            {
                return ReturnErrorIfUserIDIsEmpty("EmailAddress");
            }


            var user = await _user.IsEmailInUseAsync(EmailAddress, UserID);
            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email or UserName is already in use");
            }

        }

        /// <summary>
        /// Get All Users
        /// </summary>
        /// <param name="LoginID"></param>
        /// <param name="UserID"></param>   
        /// <returns>User List based on Search Criteria</returns>
        /// <response code="200">True if email doesn not exists else error message</response>         
        /// <response code="401">Unauthorized</response>
        [HttpGet(ApiRoutes.User.IsLoginIDInUse)]
        [ProducesResponseType(typeof(List<UserListResponse>), statusCode: 200)]
        [CustomAuthorizeAttribute(FeatureId = FeatureMasterInfra.FEATURE_ADDEDITUSER)]
        public async Task<IActionResult> IsLoginIDInUse([FromQuery] string LoginID, string UserID = "")
        {
            if (string.IsNullOrEmpty(LoginID))
            {
                return ReturnErrorIfUserIDIsEmpty("LoginID");
            }


            var user = await _user.IsLoginIDInUseAsync(LoginID, UserID);
            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"LoginID {LoginID} is already in use");
            }

        }

        private IActionResult ReturnErrorIfUserIDIsEmpty(string FieldName)
        {
            ErrorModel errorModel = new ErrorModel();
            errorModel.FieldName = FieldName;
            errorModel.Message = "Field is Mandatory";
            ErrorResponse errorResponse = new ErrorResponse();
            errorResponse.Errors = new List<ErrorModel>();
            errorResponse.Errors.Add(errorModel);
            return BadRequest(errorResponse);
        }

        /// <summary>
        /// Login By Email
        /// </summary>
        /// <param name="request"></param>
        /// <returns>User Object with Token</returns>
        /// <response code="200">User Object with Token</response>
        /// <response code="400">Unable to Login due to InValid Credentials</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost(ApiRoutes.User.TestLogin)]
        [ProducesResponseType(typeof(AuthSuccessResponse), statusCode: 200)]
        [ProducesResponseType(typeof(ErrorResponse), statusCode: 400)]
        public IActionResult TestLogin([FromBody] UserLoginRequest request)
        {
            //string encrypted = SHA256CipherManager.Encrypt("PlainPassword", "8888112233");
            //string decrypted = SHA256CipherManager.Decrypt(encrypted, "8888112233");
            //var res = CryptographyHelper.GenerateKeys();
            //  string jsEnc = request.EmailAddress;
            // string privKey = "PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0idXRmLTE2Ij8+DQo8UlNBUGFyYW1ldGVycyB4bWxuczp4c2k9Imh0dHA6Ly93d3cudzMub3JnLzIwMDEvWE1MU2NoZW1hLWluc3RhbmNlIiB4bWxuczp4c2Q9Imh0dHA6Ly93d3cudzMub3JnLzIwMDEvWE1MU2NoZW1hIj4NCiAgPEQ+Wm50azFIY21hZytZK2owMGYzOFNmMURyOTZNYWhKaGErcGNIMzA0MUdGKzY4cGo4SjFVYkRMaFZIQTlZQ2FENStFK3BhSXJ0MmpFQWErYmF6ak9scGJIcDNHM0RNRDc1VUo1TlMxbHJJc2dPWHlxazk3aEkvSlpDcFh0Z0dXOE8yaUlaMFZiK0c1b3RWSDU1NC84YnpCZmQ2b21XNzR1NlJCZnZSSWxJWU1PVDFHeDhLQmxiT0RJb0R6dzdMc0lKOUdEL2NQRWg5MlBuL01OaW5MS0IwbWVRSW1pak5MQno2RXVFd1VtZk8yN0VXaUI0QSt5aFhYMG5Tc3ltQjBZN3BVT3ZQeDNVVHp0Z0ZnT3VSbDBrSXdGT24rVUEyVlgreUZUSlB5MGxKeDczOGE4UmtsR1diZkZRaE5uMThaYitJMnl0UndyTjZNeDVSUitKZ3dXS05RPT08L0Q+DQogIDxEUD5SV1hzZ01EOE5tWXJRaUJFNWloWDE4TStVak1jc3V2RmRUV2xkZ3JVenhmcEk0WjhlOXJaYzd1aWJ6Ymtvd081TU5xQkw1SHF4VXhzYzk5QnIxbXNzYSthSCtqa1NEWEY4bW94dUtwMDJaTlFkVGJxTEdQMzh6bFYzdVl4blgrWFpsRlBZN280NjZhNk9oTXh4aHFIVy9xZnNyaXJhK01EeExGWU5CYU1uckU9PC9EUD4NCiAgPERRPmlVZzZaNVVHOXZBNFRzVk5TejljTXhjN2lFaGtoaU1Qd3FDaHI4ZkNDckM1c3lWeWQ4L3c0UXJBWUtNbjdtR25JUjJzNkNkcDRIeW5iVEppMGROTWZwa3ZmMHFCM0dGWElON0l0SEhtY2EvWmVGMjBqY1dMN0NRYnR2NDZGak1pUVV6c1JlN2taZ1BhRnhwMk5QT2pMd0IzbXJ0Ty9PYUdDTC9LZXdvbzd6OD08L0RRPg0KICA8RXhwb25lbnQ+QVFBQjwvRXhwb25lbnQ+DQogIDxJbnZlcnNlUT5acXdRQ2t3QzZiaUJObEFOZmRtcHdwM1U5UmszcSt1YlBkc1Vibm9zdzgrYWZMYkh3dVc3anFWZG5MdDlZVHhFM1FQRUl0eTRvSnprK01IUDk0M2VOOXVrTGlEVEZEVDJnbjJnUkdrOG5mTDdxSDM4UzQzZ3JUR1BhTGNENzRtRjZFcy94OUpqcmp1eGk0MnVMeGNrSTE2VGptUWo3WDlDQnVDQm5sNzE5YlU9PC9JbnZlcnNlUT4NCiAgPE1vZHVsdXM+MGo5cVhaM2k2M0hjS1QySEUra3BJMGV0YmpqVnZhOWNxN3c1SkJJVkVKcG5hWkNkVkRYM3dxeDBmY211OU54b1dZTEtZYWlXdzV0bVpqT3RzT1JEVzhjNkZCeU8yb1ZZZmlwd3MvSnR0YzFRYVhncTZKNHFYaWZpN0lqalR5VXh1QTlhbjQrTDJtYlMvZlIybDNQS0U3SDJRaXg4L2U0RC9Vcis3QUtTWFN6RGR0Vnk4UnVkRFZOQWxyNjhDYUdYcGlIbWR1OFhhSjdFNnAyZGc3WmV3a0hkYnc3T0hFYXo1V28xdExQRVlSdVhxSHNGRlkrUFNxWExxUFJ0K2ZuVXFjVzVkb0xjN1lqZmVSZW54QkxZdlFpN0xVTmJGYjduOVdXUktYUmt0cGErQ05HMnd0U05NaHlhNVV4OXZ0VllBRSszRVNOZUxaZ0V3VlRqZm1FZTRRPT08L01vZHVsdXM+DQogIDxQPjRzVzNsWkNvTEkxWTE4L2tUcGUzUzg3YjBhaVRoNTBPYVIxeFpNcFRDeGFzR3loVnprTU4rTFowV0NReWtpRTlvaG9xei9HaHZScmcyK1lpU3VSN0Z5SHpWT1V5ajM4Ynh3V3JTQ1ZYa0MvcTNuK2lnWFQ1M09PZ1VuVTd5S3RvOGxObDN6M3d3Z01qY1o2OEFqTUdUK0RtOHJyWVZmWEl3S2NFM21OMzJNTT08L1A+DQogIDxRPjdWaDVTZ3YvWjh6d2ZvUFcrR0UrcXVXZWFSaHVzUG5jU1R4M0RSckRMT2lJNEY1Q3pLRS9oYWRJcDhjUE5sUDdTcFU0L1pQa2F5Y04zUmpwelpmNUpzRmo0UG1jd2lyaGJjMXRpb3lxU2tuL2E3Qy9xRGhxWS9hdDY1UFJmRlMramxlTVBWQ2hkemdNQTVITE9hY0lNamM0T3RyamFoWGNOenBXVUUxSkQ0cz08L1E+DQo8L1JTQVBhcmFtZXRlcnM+";

            //  var dec = CryptographyHelper.DecryptValue(jsEnc, privKey);

            var userobj = _mapper.Map<UserMaster>(request);
            var user = _user.TestLogin(userobj);

            if (user != null && !string.IsNullOrEmpty(user.UserId))
            {
                user = GenerateToken(user);
                var responseObj = _mapper.Map<AuthSuccessResponse>(user);
                var responseFeaturesObj = _mapper.Map<List<FeatureMasterResponse>>(user.Features);
                responseObj.UserFeatures = responseFeaturesObj;
                responseObj.IsSuccess = true;

                return Ok(responseObj);
            }

            ErrorModel errorModel = new ErrorModel();
            errorModel.FieldName = "Username/Password";
            errorModel.Message = "Invalid Username or Password";
            ErrorResponse errorResponse = new ErrorResponse();
            errorResponse.Errors = new List<ErrorModel>();
            errorResponse.Errors.Add(errorModel);

            return BadRequest(errorResponse);
        }

        /// <summary>
        /// Test User
        /// </summary>       
        /// <returns></returns>
        /// <response code="401">Unauthorized</response>
        [CustomAuthorizeAttribute(FeatureId = "Dashboard")]
        [HttpGet(ApiRoutes.User.GetUser)]
        public IActionResult GetUser()
        {
            UserLoginRequest request = new UserLoginRequest();
            request.EmailAddress = "test@test.com";
            var userobj = _mapper.Map<UserMaster>(request);

            return Ok(
                  _mapper.Map<AuthSuccessResponse>(userobj)
                  );
        }

        /// <summary>
        /// Set new encrypted password in UserMaster table
        /// </summary>
        /// <param name="request"></param>
        /// <returns>User Object </returns>
        /// <response code="200">User Object</response>
        /// <response code="400">Invalid EmailAddress</response>
        [HttpPost(ApiRoutes.User.ForgotPassword)]
        public async Task<IActionResult> ForgotPassword([FromBody] UserForgotPasswordRequest request)
        {
            var userObj = _mapper.Map<UserMaster>(request);
            bool IsValidEmail = false;
            var objUser = await _user.GetByEmailAsync(userObj);
            if (!string.IsNullOrEmpty(objUser?.EmailAddress) && new EmailAddressAttribute().IsValid(objUser?.EmailAddress))
                IsValidEmail = true;
            if (string.IsNullOrEmpty(objUser?.EmailAddress) || !IsValidEmail)
            {
                //ErrorModel errorModel = new ErrorModel();
                //errorModel.FieldName = "Email Address";
                //errorModel.Message = "Invalid Email Address";
                //ErrorResponse errorResponse = new ErrorResponse();
                //errorResponse.Errors = new List<ErrorModel>();
                //errorResponse.Errors.Add(errorModel);

                //return BadRequest(errorResponse);
                return Ok();
            }
            var obj = await _user.SetNewPassword(userObj);

            return Ok();
        }

        /// <summary>
        /// Get  User by Email Address
        /// </summary>
        /// <param name="request"></param>
        /// <returns>User Object </returns>
        /// <response code="200">User Object</response>
        /// <response code="400">Invalid EmailAddress</response>
        [HttpPost(ApiRoutes.User.GetByEmail)]
        public async Task<IActionResult> GetByEmail([FromBody] UserForgotPasswordRequest request)
        {
            var userObj = _mapper.Map<UserMaster>(request);
            var obj = await _user.GetByEmailAsync(userObj);
            var responseObj = _mapper.Map<UserForgotPasswordResponse>(obj);
            if (!string.IsNullOrEmpty(obj?.EmailAddress))
            {
                responseObj.IsSuccess = true;
                return Ok(responseObj);
            }

            ErrorModel errorModel = new ErrorModel();
            errorModel.FieldName = "EmailAddress";
            errorModel.Message = "Invalid EmailAddress";
            ErrorResponse errorResponse = new ErrorResponse();
            errorResponse.Errors = new List<ErrorModel>();
            errorResponse.Errors.Add(errorModel);

            return BadRequest(errorResponse);
        }

        private UserMaster GenerateToken(UserMaster loggedInUser)
        {
            string strUserFeatures = string.Empty;
            if (loggedInUser.Features != null && loggedInUser.Features.Count > 0)
            {
                strUserFeatures = string.Join(",", loggedInUser.Features.Select(x => x.FeatureId));
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var claims = new List<Claim>
                {
                    new Claim(type:JwtRegisteredClaimNames.Sub,value:loggedInUser.FirstName),
                    new Claim(type:JwtRegisteredClaimNames.Jti,value:Guid.NewGuid().ToString()),
                    new Claim(type:JwtRegisteredClaimNames.Email,value:loggedInUser.EmailAddress),
                    new Claim(type:"id",value:loggedInUser.UserId),
                    new Claim(type:"Features",value:strUserFeatures)
                };

            DateTime expiryDate = DateTime.UtcNow.Add(_jwtSettings.TokenLifeTime);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(claims

                    ),
                Expires = expiryDate,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                algorithm: SecurityAlgorithms.HmacSha256Signature)

            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            loggedInUser.Token = tokenHandler.WriteToken(token);



            var refreshToken = new RefreshToken
            {
                JwtId = token.Id,
                UserId = loggedInUser.UserId,
                Creationdate = DateTime.UtcNow,
                EmailAddress = loggedInUser.EmailAddress,
                ExpiryDate = DateTime.UtcNow.AddMonths(6),
            };
            refreshToken.Token = GenerateRefreshToken();
            loggedInUser.RefreshToken = refreshToken.Token;
            loggedInUser.ExpiryDate = expiryDate;
            _iRefreshTokenService.SaveRefreshToken(refreshToken);

            // remove password before returning



            return loggedInUser;
        }


        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }


        /// <summary>
        /// Get All Lookups for Usere Management
        /// </summary>   
        /// <returns>Get All Lookups for Usere Management</returns>
        /// <response code="200">Lists of all Dropdown Values</response>         
        /// <response code="401">Unauthorized</response>
        [HttpGet(ApiRoutes.User.GetLookUps)]
        [ProducesResponseType(typeof(UserSearchModel), statusCode: 200)]
        [ProducesResponseType(typeof(ErrorResponse), statusCode: 400)]
        [CustomAuthorizeAttribute]
        public IActionResult GetLookUps()
        {

            var userSearchModel = _user.GetLookupsForUserSearch();

            if (userSearchModel != null)
            {

                var responseObj = _mapper.Map<UserSearchModelResponse>(userSearchModel);
                //  var ListDepartments = _mapper.Map<List<DepartmentMasterResponse>>(userSearchModel.ListDepartments);
                //var ListJobRoles = _mapper.Map<List<JobRoleMasterResponse>>(userSearchModel.ListJobRoles);
                var ListUserStatus = _mapper.Map<List<LookUpMasterResponse>>(userSearchModel.ListUserStatus);
                //var ListCountryMaster = _mapper.Map<List<CountryMasterResponse>>(userSearchModel.ListCountries);
                //var ListTimeZoneMaster = _mapper.Map<List<TimeZoneMasterResponse>>(userSearchModel.ListTimeZones);
                //var ListStateMaster = _mapper.Map<List<StateMasterResponse>>(userSearchModel.ListStates);
                //var ListCityMaster = _mapper.Map<List<CityMasterResponse>>(userSearchModel.ListCities);
                //var ListEmpManagerJobRole = _mapper.Map<List<LookUpMasterResponse>>(userSearchModel.ListEmpManagerJobRole);

                // responseObj.ListDepartments = ListDepartments;
                //responseObj.ListJobRoles = ListJobRoles;
                responseObj.ListUserStatus = ListUserStatus;
                //responseObj.ListCountries = ListCountryMaster;
                //responseObj.ListTimeZones = ListTimeZoneMaster;
                //responseObj.ListStates = ListStateMaster;
                //responseObj.ListCities = ListCityMaster;
                //responseObj.ListEmpManagerJobRole = ListEmpManagerJobRole;

                responseObj.IsSuccess = true;

                return Ok(responseObj);
            }

            ErrorModel errorModel = new ErrorModel();
            errorModel.FieldName = "Error";
            errorModel.Message = "Error While Fetching Records";
            ErrorResponse errorResponse = new ErrorResponse();
            errorResponse.Errors = new List<ErrorModel>();
            errorResponse.Errors.Add(errorModel);

            return BadRequest(errorResponse);
        }


        /// <summary>
        /// Add Edit User
        /// </summary>
        /// <param name="request"></param>       
        /// <returns>UserID</returns>
        /// <response code="200">UserId of User</response>
        /// <response code="400">ErrorResponse</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost(ApiRoutes.User.AddEditUser), DisableRequestSizeLimit]
        [ProducesResponseType(typeof(string), statusCode: 200)]
        [ProducesResponseType(typeof(ErrorResponse), statusCode: 401)]
        [ProducesResponseType(typeof(ErrorResponse), statusCode: 400)]
        [CustomAuthorizeAttribute(FeatureId = FeatureMasterInfra.FEATURE_ADDEDITUSER)]
        public async Task<IActionResult> AddEditUser([FromBody] CreateUserRequest request)
        {
            var userobj = _mapper.Map<UserMaster>(request);
            var userid = await _user.AddEditUserAsync(userobj);
            return Ok(userid);
        }





        /// <summary>
        /// Add Edit User
        /// </summary>
        /// <param name="strUserID"></param>
        /// <param name="uploadedFile"></param>       
        /// <returns>IsSuccess as Boolean</returns>
        /// <response code="200">True or False</response>
        /// <response code="400">BadRequest</response>
        /// <response code="401">Unauthorized</response>
        //[HttpPost(ApiRoutes.User.UploadUserProfilePic), DisableRequestSizeLimit]
        //[ProducesResponseType(typeof(bool), statusCode: 200)]
        //[ProducesResponseType(typeof(ErrorResponse), statusCode: 401)]
        //[ProducesResponseType(typeof(ErrorResponse), statusCode: 400)]
        //[CustomAuthorizeAttribute(FeatureId = FeatureMasterInfra.FEATURE_ADDEDITUSER)]
        //public async Task<IActionResult> UploadUserProfilePic([FromForm] string strUserID, [FromForm] IFormFile uploadedFile)
        //{
        //    if (!string.IsNullOrEmpty(strUserID) && uploadedFile != null && uploadedFile.Length > 0)
        //    {
        //        var IsSuccess = await _user.UploadProfilePic(strUserID, uploadedFile);
        //        return Ok(true);
        //    }

        //    return BadRequest(false);

        //}

        /// <summary>
        /// Make User Active or InActive
        /// </summary>
        /// <param name="request"></param>
        /// <returns>200 Ok</returns>
        /// <response code="200">Integer Number</response>
        /// <response code="400">ErrorResponse</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost(ApiRoutes.User.ActiveInActiveUser)]
        [ProducesResponseType(typeof(int), statusCode: 200)]
        [ProducesResponseType(typeof(ErrorResponse), statusCode: 401)]
        [ProducesResponseType(typeof(ErrorResponse), statusCode: 400)]
        [CustomAuthorizeAttribute(FeatureId = FeatureMasterInfra.FEATURE_ADDEDITUSER)]
        public async Task<IActionResult> ActiveInActiveUser([FromBody] UserActiveInActiveRequest request)
        {
            var userobj = _mapper.Map<UserMaster>(request);
            var user = await _user.ActiveInActiveUser(userobj);
            return Ok();

        }

        /// <summary>
        /// Get  User by User Id
        /// </summary>
        /// <param name="request"></param>
        /// <returns>User Object </returns>
        /// <response code="200">User Object</response>
        /// <response code="400">Invalid UserId or RequesterUserId</response>
        [HttpPost(ApiRoutes.User.GetUserData)]
        public async Task<IActionResult> GetUserData([FromBody] GetUserRequest request)
        {
            var userObj = _mapper.Map<UserMaster>(request);
            var obj = await _user.GetAsync(userObj);

            if (obj == null)
            {
                ErrorModel errorModel1 = new ErrorModel();
                errorModel1.FieldName = "UserId/RequesterUserId";
                errorModel1.Message = "Invalid UserId or RequesterUserId";
                ErrorResponse errorResponse1 = new ErrorResponse();
                errorResponse1.Errors = new List<ErrorModel>();
                errorResponse1.Errors.Add(errorModel1);

                return BadRequest(errorResponse1);
            }

            var responseObj = _mapper.Map<GetUserResponse>(obj);
            if (!string.IsNullOrEmpty(obj?.LoginId))
            {
                responseObj.IsSuccess = true;
                return Ok(responseObj);
            }

            ErrorModel errorModel = new ErrorModel();
            errorModel.FieldName = "UserId/RequesterUserId";
            errorModel.Message = "Invalid UserId or RequesterUserId";
            ErrorResponse errorResponse = new ErrorResponse();
            errorResponse.Errors = new List<ErrorModel>();
            errorResponse.Errors.Add(errorModel);

            return BadRequest(errorResponse);
        }

        ///// <summary>
        ///// Get User Template xls file
        ///// </summary>
        ///// <returns></returns>
        ////[ProducesResponseType(typeof(File), statusCode: 200)]
        ////[CustomAuthorizeAttribute(FeatureId = FeatureMasterInfra.FEATURE_ADDEDITUSER)]
        ////[HttpGet(ApiRoutes.User.GetUserTemplateExcel)]
        //public async Task<IActionResult> GetUserTemplateExcel()
        //{
        //    Stream stream = await _user.GetUserTemplateFile();
        //    string fileName = _options.Value.UserTemplateFileName;
        //    return File(stream, GetContentType(fileName), fileName);
        //}





        /// <summary>
        /// Change User's Password. Password must MD5 Hashed from client side        
        /// </summary>
        /// <param name="request">User object</param>
        /// <returns></returns>
        /// <response code="200">success if password changed successfully</response>         
        /// <response code="401">Unauthorized</response>
        /// <response code="400">Invalid User</response>
        /// <response code="400">Incorrect Current Password</response>
        [ProducesResponseType(typeof(ErrorResponse), statusCode: 401)]
        [ProducesResponseType(typeof(ErrorResponse), statusCode: 400)]
        [HttpPost(ApiRoutes.User.ChangePassword)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            DateTime startTime = DateTime.Now;

            var userObj = _mapper.Map<UserMaster>(request);
            UserMaster obj = new UserMaster();

            if (request.IsAdminPasswordChangeRequest == true)
            {
                obj = await _user.GetAsync(userObj);
            }

            if (request.IsAdminPasswordChangeRequest == false)
            {
                obj = await _user.GetSelfUserInfo(userObj);
                obj = await _user.GetByEmailAsync(obj);
                if (string.IsNullOrEmpty(obj?.UserId))
                {
                    ErrorModel errorModel = new ErrorModel();
                    errorModel.FieldName = "UserId";
                    errorModel.Message = "Invalid User ID";
                    ErrorResponse errorResponse = new ErrorResponse();
                    errorResponse.Errors = new List<ErrorModel>();
                    errorResponse.Errors.Add(errorModel);

                    return BadRequest(errorResponse);
                }

                if (userObj?.CurrentPassword != obj.Password)
                {
                    ErrorModel errorModel = new ErrorModel();
                    errorModel.FieldName = "CurrentPassowrd";
                    errorModel.Message = "Incorrect Current Password";
                    ErrorResponse errorResponse = new ErrorResponse();
                    errorResponse.Errors = new List<ErrorModel>();
                    errorResponse.Errors.Add(errorModel);

                    return BadRequest(errorResponse);
                }
            }
            userObj.UserId = obj.UserId;
            userObj.EmailAddress = obj.EmailAddress;
            userObj.IsPasswordChanged = request.IsPasswordChanged;
            userObj.RequesterUserId = request.RequesterUserId;
            await _user.ChangePassword(userObj);

            DateTime endTime = DateTime.Now;

            _iRefreshTokenService.SaveSaltTime(startTime, endTime, "ChangePassword");
            return Ok();
        }



        /// <summary>
        /// Download User data based on filters applied. 
        /// </summary>
        /// <param name="request">User object with filters if any</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(File), statusCode: 200)]
        [CustomAuthorize(FeatureId = FeatureMasterInfra.FEATURE_ADDEDITUSER)]
        [HttpPost(ApiRoutes.User.ExportUserData)]
        public async Task<IActionResult> ExportUserData([FromBody] UserListRequest request)
        {
            var userObj = _mapper.Map<UserMaster>(request);
            Stream stream = await _user.GetUserExportData(userObj);
            string fileName = "User Data_" + DateTime.Now.ToString("MM-dd-yyyy hh mm ss") + ".xlsx";
            return File(stream, GetContentType(fileName), fileName);
        }




        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }

        [HttpGet(ApiRoutes.User.GetSelfUserInfo)]
        public async Task<IActionResult> GetSelfUserInfo(GetUserRequest request)
        {
            var userObj = _mapper.Map<UserMaster>(request);

            userObj = await _user.GetSelfUserInfo(userObj);

            return Ok(_mapper.Map<GetUserResponse>(userObj));
        }


        public async Task<AuthSuccessResponse> RefreshTokenAsync(string token, string refreshToken)
        {

            var validatedToken = GetPrincipalFromToken(token);

            var expiryDateUnix = long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
            var expiryDate = new DateTime(year: 1970, month: 1, day: 1, hour: 0, minute: 0, second: 0, DateTimeKind.Utc).
                AddSeconds(expiryDateUnix);



            var JtiId = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            RefreshToken validateRefreshToken = new RefreshToken();
            validateRefreshToken.Token = refreshToken;
            var storedRefreshToken = await _iRefreshTokenService.GetRefreshToken(validateRefreshToken);


            storedRefreshToken.Used = true;


            await _iRefreshTokenService.UpdateRefreshToken(storedRefreshToken);

            UserMaster entrequest = new UserMaster();
            entrequest.EmailAddress = storedRefreshToken.EmailAddress;

            var userobj = _mapper.Map<UserMaster>(entrequest);
            var user = _user.LoginAndGetFeaturesWithEmail(userobj);

            if (user != null && !string.IsNullOrEmpty(user.UserId))
            {
                user = GenerateToken(user);
                var responseObj = _mapper.Map<AuthSuccessResponse>(user);
                var responseFeaturesObj = _mapper.Map<List<FeatureMasterResponse>>(user.Features);
                //var responseHirarchyObj = _mapper.Map<UserHierarchyResponse>(user.userHierarchy);
                var responseRolesObj = _mapper.Map<List<UserRoleResponse>>(user.userRoles);


                responseObj.UserFeatures = responseFeaturesObj;
                responseObj.userRoles = responseRolesObj;
                // responseObj.userHierarchy = responseHirarchyObj;
                responseObj.IsSuccess = true;

                return responseObj;

            }


            return null;



        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenValidator = new JwtSecurityTokenHandler();
            _TokenValidationParameters.ValidateLifetime = false;
            try
            {
                var principal = tokenValidator.ValidateToken(token, _TokenValidationParameters, out var ValidatedToken);
                if (!IsJwtWithvalididSecurityAlogorithm(ValidatedToken))
                {
                    return null;
                }
                return principal;
            }
            catch
            {
                return null;
            }
        }

        private bool IsJwtWithvalididSecurityAlogorithm(SecurityToken validateToken)
        {
            return (validateToken is JwtSecurityToken jwtSecurityToken) && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase);
        }



        /// <summary>
        /// Refresh Token 
        /// </summary>
        /// <param name="request"></param>
        /// <returns>User Object with Token</returns>
        /// <response code="200">User Object with Token</response>
        /// <response code="400">Unable to Login due to InValid Credentials</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost(ApiRoutes.User.Refresh)]
        [ProducesResponseType(typeof(AuthSuccessResponse), statusCode: 200)]
        [ProducesResponseType(typeof(ErrorResponse), statusCode: 400)]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            var authresult = await RefreshTokenAsync(request.Token, request.RefreshToken);

            if (authresult == null)
            {
                return BadRequest(authresult);
            }

            return Ok(authresult);
        }


        /// <summary>
        /// Get All Roles by UserId
        /// </summary>
        /// <param name="request"></param>
        /// <returns>User List based on Search Criteria</returns>
        /// <response code="200">User List</response>         
        /// <response code="401">Unauthorized</response>
        [HttpGet(ApiRoutes.User.GetRoleListByUserId)]
        [ProducesResponseType(typeof(List<UserListResponse>), statusCode: 200)]
        [CustomAuthorizeAttribute(FeatureId = FeatureMasterInfra.FEATURE_GETALLUSERS)]
        public async Task<IActionResult> GetRoleListByUserId(GetUserRequest request)
        {
            var userobj = _mapper.Map<UserMaster>(request);
            if (string.IsNullOrEmpty(userobj.UserId))
            {
                return ReturnErrorIfUserIDIsEmpty("UserId");
            }

            var UserList = await _user.GetUserRolesbyUserId(userobj);

            var responseObj = _mapper.Map<List<UserRoleResponse>>(UserList);
            return Ok(responseObj);

        }

        /// <summary>
        /// Get Job Roles
        /// </summary>
        /// <returns></returns>
        //[HttpGet(ApiRoutes.User.GetJobRoles)]
        //[ProducesResponseType(typeof(List<JobRoleMasterResponse>), statusCode: 200)]
        //[CustomAuthorize]
        //public async Task<IActionResult> GetJobRoles()
        //{
        //    var lstJobRoles = await iJobRoleMaster.GetAllAsync(new JobRoleMaster());

        //    var responseObj = _mapper.Map<List<JobRoleMasterResponse>>(lstJobRoles);
        //    return Ok(responseObj);

        //}

        /// <summary>
        /// Check whether User is enrolled through Order Code
        /// </summary>
        /// <param name="OrderCode"></param>
        /// <param name="RequesterUserId"></param>
        /// <returns></returns>
        //[HttpGet(ApiRoutes.User.IsUserEnrolledByOrderCode)]
        //public async Task<IActionResult> IsUserEnrolledByOrderCode(string OrderCode, string RequesterUserId)
        //{
        //    var ret = await _user.IsUserEnrolledByOrderCode(OrderCode, RequesterUserId);
        //    return Ok(ret?.ErrorMessage);
        //}

        //[HttpGet(ApiRoutes.User.IsOrderCodeExists)]
        //public async Task<IActionResult> IsOrderCodeExists(string OrderCode)
        //{
        //    var ret = await iCourseMaster.IsOrderCodeExists(OrderCode);
        //    if (ret == null)
        //    {
        //        return Json($"Enrollment Code does not exists");
        //    }
        //    else
        //    {
        //        return Json(true);
        //    }
        //}

        //[HttpPost(ApiRoutes.User.RemoveGroupAdmin)]
        //[CustomAuthorize(FeatureId = FeatureMasterInfra.FEATURE_ADDEDITUSER)]
        public async Task<IActionResult> RemoveGroupAdmin(string UserRoleId)
        {
            if (string.IsNullOrEmpty(UserRoleId))
            {
                return ReturnErrorIfUserIDIsEmpty("UserRoleId");
            }

            UserMaster obj = new UserMaster()
            {
                UserRoleId = UserRoleId
            };
            await _user.RemoveGroupAdmin(obj);

            return Ok();
        }






        [HttpGet(ApiRoutes.User.TrackSignOutTime)]
        [ProducesResponseType(typeof(string), statusCode: 200)]
        [ProducesResponseType(typeof(ErrorResponse), statusCode: 400)]
        public async Task<IActionResult> TrackSignOutTime(string loginUniqueId)
        {
            UserMaster userMaster = new UserMaster() { LoginUniqueId = loginUniqueId };
            await _user.TrackSignOutTime(userMaster);
            return Ok();
        }

        //[HttpGet(ApiRoutes.User.WelcomeEmailForNotLoggedInUsers)]
        //[CustomAuthorize(FeatureId = FeatureMasterInfra.FEATURE_ADDEDITUSER)]
        //public async Task<IActionResult> WelcomeEmailForNotLoggedInUsers(string RequesterUserId)
        //{
        //    await _user.SendWelcomeEmailToUsersNotLogIn(RequesterUserId);

        //    return Ok();
        //}

        //[HttpGet(ApiRoutes.User.WelcomeEmailLog)]
        //[CustomAuthorize(FeatureId = FeatureMasterInfra.FEATURE_ADDEDITUSER)]
        public async Task<IActionResult> WelcomeEmailLog(string RequesterUserId)
        {
            UserMaster obj = new UserMaster() { RequesterUserId = RequesterUserId };
            var lstLog = await _user.GetWelcomeEmailLog(obj);

            var responseObj = _mapper.Map<List<WelcomeEmailLogResponse>>(lstLog);
            return Ok(responseObj);
        }



        ////
        ///


        /// <summary>
        ///Get Features By UserRole
        /// </summary>
        /// <param name="request"></param>
        /// <returns>User Object with Token</returns>
        /// <response code="200">User Object with Token</response>
        /// <response code="400">Unable to Login due to InValid Credentials</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost(ApiRoutes.User.GetFeaturesByUserRole)]
        [ProducesResponseType(typeof(AuthSuccessResponse), statusCode: 200)]
        [ProducesResponseType(typeof(ErrorResponse), statusCode: 400)]
        [ProducesResponseType(typeof(ErrorResponse), statusCode: 401)]
        [CustomAuthorizeAttribute]
        public IActionResult GetFeaturesByUserRole([FromBody] UserFeaturesRequest request)
        //public IActionResult GetFeaturesByUserRole(UserFeaturesRequest request)
        {
            DateTime startTime = DateTime.Now;
            //AuthSuccessResponse obj = new AuthSuccessResponse();
            var userobj = _mapper.Map<UserMaster>(request);
            var user = _user.GetFeaturesByUserRole(userobj);

            if (user != null && !string.IsNullOrEmpty(user.UserId))
            {
                user = GenerateToken(user);
                //obj.UserFeatures = _mapper.Map<List<FeatureMasterResponse>>(user.Features);

                var responseObj = _mapper.Map<AuthSuccessResponse>(user);
                var responseFeaturesObj = _mapper.Map<List<FeatureMasterResponse>>(user.Features);
                var responseRolesObj = _mapper.Map<List<UserRoleResponse>>(user.userRoles);

                responseObj.UserFeatures = responseFeaturesObj;
                responseObj.userRoles = responseRolesObj;

                DateTime endTime = DateTime.Now;
                _iRefreshTokenService.SaveSaltTime(startTime, endTime, "Login");

                return Ok(responseObj);
            }

            ErrorModel errorModel = new ErrorModel();
            errorModel.FieldName = "Error";
            errorModel.Message = "Error While Fetching Records";
            ErrorResponse errorResponse = new ErrorResponse();
            errorResponse.Errors = new List<ErrorModel>();
            errorResponse.Errors.Add(errorModel);

            return BadRequest(errorResponse);
        }


    }
}