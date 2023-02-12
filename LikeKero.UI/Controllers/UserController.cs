using DataTables.Mvc;
using LikeKero.UI.Models;
using LikeKero.UI.RefitClientFactory;
using LikeKero.UI.Utility;
using LikeKero.UI.ViewModels;

using LikeKero.UI.ViewModels.Request.User;
using LikeKero.UI.ViewModels.Response.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Refit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LikeKero.UI.Controllers
{
    public class UserController : Controller
    {
        private readonly IOptions<MySettingsModel> _options;
        public UserController(IOptions<MySettingsModel> options)
        {
            _options = options;
        }
        public async Task<IActionResult> ManageUsers()
        {
            var objSessionUser = HttpContext.Session.GetSessionUser();
            var cachedToken = HttpContext.Session.GetBearerToken();
            var userAPI = RestService.For<IUserApi>(hostUrl: ApplicationSettings.WebApiUrl, new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken)
            });
            var apiResponse = await userAPI.GetLookUps();
            var response = apiResponse.Content;

            //ViewBag.lstDepartments = new SelectList(response.ListDepartments, "DepartmentId", "DepartmentName");
            //ViewBag.lstJobRoles = new SelectList(response.ListJobRoles, "JobCodeId", "JobCode");
            ViewBag.lstUserStatus = new SelectList(response.ListUserStatus, "LookUpValue", "LookUpName");

            UserListRequestVM model = new UserListRequestVM();
            model.Status = true;
            model.UserFeatures = objSessionUser.UserFeatures;
            return View(model);
        }

        //[HttpGet]
        //public async Task<IActionResult> SearchEntityAsync(string Prefix)
        //{
        //    var objSessionUser = HttpContext.Session.GetSessionUser();
        //    var cachedToken = HttpContext.Session.GetBearerToken();
        //    var userAPI = RestService.For<IUserApi>(hostUrl: ApplicationSettings.WebApiUrl, new RefitSettings
        //    {
        //        AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken)
        //    });

        //    var apiResponse = await userAPI.EntityList(RequesterUserId: objSessionUser.UserId, SearchPhrase: Prefix, GroupId: "");

        //    var response = apiResponse.Content;
        //    return Json(response);

        //    //ViewBag.Response = response;
        //    //return PartialView("_ViewEntityTree", response); //Json(response);
        //}

        //[HttpPost]
        //public async Task<IActionResult> SearchEntityByGroupId(string Prefix)
        //{
        //    var objSessionUser = HttpContext.Session.GetSessionUser();
        //    var cachedToken = HttpContext.Session.GetBearerToken();
        //    var userAPI = RestService.For<IUserApi>(hostUrl: ApplicationSettings.WebApiUrl, new RefitSettings
        //    {
        //        AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken)
        //    });

        //    var apiResponse = await userAPI.EntityList(RequesterUserId: objSessionUser.UserId, SearchPhrase: "", GroupId: Prefix);

        //    var response = apiResponse.Content;
        //    //return Json(response);

        //    ViewBag.Response = response;
        //    return PartialView("_ViewEntityTree", response); //Json(response);
        //}

        [HttpPost("/User/FillTableUsersAsync")]
        public async Task<IActionResult> FillTableUsersAsync([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest dt, string Active = "1")
        {
            var objSessionUSer = HttpContext.Session.GetSessionUser();

            List<ColumnNameValue> colList = new List<ColumnNameValue>();
            ColumnNameValue columns = new ColumnNameValue();

            int iPageSize;
            string sortdir = " asc ";
            int TotalCount = 0;

            if (dt.Length <= 0)
                iPageSize = 10;
            else
                iPageSize = dt.Length;

            if (dt.SortDirection.Equals(Column.OrderDirection.Ascendant))
                sortdir = " asc ";
            else
                sortdir = " desc ";

            UserListRequestVM searchObj = new UserListRequestVM();
            searchObj.RequesterUserId = objSessionUSer.UserId;

            if (!string.IsNullOrEmpty(dt.SortColumnName))
                searchObj.SortExp = dt.SortColumnName + " " + sortdir;

            searchObj.PageSize = iPageSize;

            if (dt.PageIndex == 0)
                searchObj.PageIndex = 1;
            else
                searchObj.PageIndex = dt.PageIndex;

            if (Active == "1")
                searchObj.Status = true;

            foreach (var col in dt.Columns)
            {
                columns = new ColumnNameValue();
                if (!string.IsNullOrEmpty(col.Search.Value))
                {
                    columns.ColName = col.Name;
                    columns.ColValue = col.Search.Value;
                    colList.Add(columns);
                    switch (col.Name)
                    {
                        case "FirstName":
                            if (!string.IsNullOrEmpty(col.Search.Value))
                            {
                                searchObj.FirstName = col.Search.Value.Trim();
                            }
                            break;
                        case "LastName":
                            if (!string.IsNullOrEmpty(col.Search.Value))
                            {
                                searchObj.LastName = col.Search.Value.Trim();
                            }
                            break;
                        case "EmailAddress":
                            if (!string.IsNullOrEmpty(col.Search.Value))
                            {
                                searchObj.EmailAddress = col.Search.Value.Trim();
                            }
                            break;
                        case "Group1Name":
                            if (!string.IsNullOrEmpty(col.Search.Value))
                            {
                                string[] arrValues = col.Search.Value.Split('~');
                                searchObj.GroupId = arrValues[0];
                                searchObj.SearchOnGroupId = arrValues[1];
                            }
                            break;
                        case "DepartmentId":
                            if (!string.IsNullOrEmpty(col.Search.Value))
                            {
                                searchObj.DepartmentId = col.Search.Value.Trim();
                            }
                            break;
                        case "JobCodeId":
                            if (!string.IsNullOrEmpty(col.Search.Value))
                            {
                                searchObj.JobCodeId = col.Search.Value.Trim();
                            }
                            break;
                        case "Status":
                            if (!string.IsNullOrEmpty(col.Search.Value))
                            {
                                if (!string.IsNullOrEmpty(col.Search.Value))
                                {
                                    searchObj.Status = col.Search.Value.Trim().ToLower() == "true" ? true : false;
                                }
                            }
                            break;
                        case "HiringDateFrom":
                            if (!string.IsNullOrEmpty(col.Search.Value))
                            {
                                searchObj.HiringDateFrom = Convert.ToDateTime(col.Search.Value.Trim());
                            }
                            break;
                        case "HiringDateTo":
                            if (!string.IsNullOrEmpty(col.Search.Value))
                            {
                                searchObj.HiringDateTo = Convert.ToDateTime(col.Search.Value.Trim());
                            }
                            break;
                        case "RoleChangeDateFrom":
                            if (!string.IsNullOrEmpty(col.Search.Value))
                            {
                                searchObj.RoleChangeDateFrom = Convert.ToDateTime(col.Search.Value.Trim());
                            }
                            break;
                        case "RoleChangeDateTo":
                            if (!string.IsNullOrEmpty(col.Search.Value))
                            {
                                searchObj.RoleChangeDateTo = Convert.ToDateTime(col.Search.Value.Trim());
                            }
                            break;
                        case "IsAdmin":
                            if (!string.IsNullOrEmpty(col.Search.Value))
                            {
                                searchObj.IsAdmin = Convert.ToBoolean(col.Search.Value.Trim());
                            }
                            break;
                    }
                }
            }

            var cachedToken = HttpContext.Session.GetBearerToken();


            var userAPI = RestService.For<IUserApi>(hostUrl: ApplicationSettings.WebApiUrl, new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken)
            });

            HttpContext.Session.Remove("SearchUser");
            HttpContext.Session.SetObject("SearchUser", searchObj);

            var apiResponse = await userAPI.GetUserList(searchObj);
            var response = apiResponse.Content;

            var lst = response;

            var lst1 = lst?.ToList();

            if (lst1 != null && lst1.Count > 0)
            {
                TotalCount = lst1[0].TotalCount;
            }
            bool set = false;
            if (searchObj.PageIndex > 1 && TotalCount == 0)
                set = true;
            return Json(new DataTablesResponse(dt.Draw, lst1, TotalCount, TotalCount, colList, set));
        }

        //[HttpGet]
        //[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        //public async Task<IActionResult> AddEditUser(string UserId)
        //{
        //    string Group5Name = "";
        //    var objSessionUser = HttpContext.Session.GetSessionUser();
        //    var cachedToken = HttpContext.Session.GetBearerToken();
        //    var userAPI = RestService.For<IUserApi>(hostUrl: ApplicationSettings.WebApiUrl, new RefitSettings
        //    {
        //        AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken)
        //    });

        //    var apiResponse = await userAPI.GetLookUps();
        //    var response = apiResponse.Content;
        //    ViewBag.lstCountries = new SelectList(response.ListCountries, "CountryId", "CountryName");
        //    ViewBag.lstTimeZones = new SelectList(response.ListTimeZones, "TimeZoneId", "Description");
        //    //ViewBag.lstDepartments = new SelectList(response.ListDepartments, "DepartmentId", "DepartmentName");
        //    //ViewBag.lstJobRoles = new SelectList(response.ListJobRoles, "JobCodeId", "JobCode");
        //    ViewBag.lstStates = new SelectList(response.ListStates, "StateId", "StateName");
        //    ViewBag.lstCities = new SelectList(response.ListCities, "CityId", "CityName");
        //    ViewBag.lstDisplayJobRoles = new SelectList(response.ListCities, "LookUpValue", "LookUpName");

        //    GetUserRequestVM updateModel = new GetUserRequestVM();
        //    updateModel.UserId = UserId;
        //    updateModel.RequesterUserId = objSessionUser.UserId;

        //    UserMasterRequestVM model = new UserMasterRequestVM();
        //    model.UserStatus = true;
        //    if (!string.IsNullOrEmpty(UserId))
        //    {
        //        var updateResponse = await userAPI.GetUserDataAsync(updateModel);
        //        model = updateResponse.Content;
        //        if (model != null)
        //        {
        //            model.UserStatus = model.Status;
        //            model.FileName = Path.GetFileName(model.ProfilePic);
        //            model.ProfilePic = Path.GetFileName(model.ProfilePic).Contains("NoImage") ? null : model.ProfilePic;
        //        }
        //    }
        //    else
        //    {
        //        if (objSessionUser.userRoles.Count() == 1 && objSessionUser.userRoles[0].RoleId == "Level3Admin")
        //        {
        //            Group5Name = objSessionUser.userRoles[0].Group3Id;
        //        }
        //    }
        //    ViewBag.Level5AdminGroupName = Group5Name;


        //    // return View(model);
        //    return PartialView("AddEditUser", model);
        //}

        [HttpGet]
        public async Task<JsonResult> IsEmailExists(string UserId, string EmailAddress)
        {
            if (EmailAddress == null || string.IsNullOrEmpty(EmailAddress.Trim()))
            {
                return Json("Please enter Email Or UserName");
            }
            else
            {
                var objSessionUser = HttpContext.Session.GetSessionUser();
                var cachedToken = HttpContext.Session.GetBearerToken();
                var userAPI = RestService.For<IUserApi>(hostUrl: ApplicationSettings.WebApiUrl, new RefitSettings
                {
                    AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken)
                });

                var res = "";
                var apiResponse = await userAPI.IsEmailInUse(EmailAddress, UserId);
                if (apiResponse != null)
                {
                    res = apiResponse.Content.ReadAsStringAsync().Result;
                }

                return Json(res);
            }
        }

        [HttpGet]
        public async Task<JsonResult> IsLoginIDExists(string UserId, string LoginId)
        {
            if (LoginId == null || string.IsNullOrEmpty(LoginId.Trim()))
            {
                return Json("Please enter Login ID");
            }
            else
            {
                var objSessionUser = HttpContext.Session.GetSessionUser();
                var cachedToken = HttpContext.Session.GetBearerToken();
                var userAPI = RestService.For<IUserApi>(hostUrl: ApplicationSettings.WebApiUrl, new RefitSettings
                {
                    AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken)
                });

                var res = "";
                var apiResponse = await userAPI.IsLoginIDInUse(LoginId, UserId);
                if (apiResponse != null)
                {
                    res = apiResponse.Content.ReadAsStringAsync().Result;
                }

                return Json(res);
            }
        }

        [HttpPost]
        public ActionResult ValidateDateEqualOrGreater(DateTime date)
        {
            // validate your date here and return True if validated
            if (date >= DateTime.Now)
            {
                return Json(true);
            }
            return Json(false);
        }
        //[HttpPost]
        //public async Task<IActionResult> AddEditUser(UserMasterRequestVM model)
        //{
        //    //FileUploadTest(ProfilePicformFile, ProfilePicformFile.FileName);
        //    bool IsNewRecord = string.IsNullOrEmpty(model.UserId) ? true : false;
        //    UserAddEditResponseVM modelResponse = new UserAddEditResponseVM();
        //    var objSessionUser = HttpContext.Session.GetSessionUser();
        //    var cachedToken = HttpContext.Session.GetBearerToken();
        //    var userAPI = RestService.For<IUserApi>(hostUrl: ApplicationSettings.WebApiUrl, new RefitSettings
        //    {
        //        AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken),

        //    });
        //    string msg = IsNewRecord ? "User added successfully" : "User updated successfully";
        //    bool isAddEdit = false;
        //    if (!model.IsOIGChecked && model.UserStatus == true)
        //    {
        //        OIGCheckRequestVM oigCheck = new OIGCheckRequestVM();
        //        oigCheck.FirstName = model.FirstName;
        //        oigCheck.LastName = model.LastName;
        //        oigCheck.GroupId = model.GroupId;
        //        var oigResponse = await userAPI.CheckOIG(oigCheck);

        //        if (oigResponse?.Content != null)
        //        {
        //            model.Status = model.UserStatus;
        //            model.RequesterUserId = objSessionUser.UserId;
        //            var apiResponse = await userAPI.AddEditUser(model);
        //            string userId = apiResponse?.Content?.ReadAsStringAsync().Result;
        //            modelResponse.Message = msg;


        //            modelResponse.IsOIGChecked = true;
        //            //modelResponse.OIGCheckURL = _options.Value.OIGCheckURL;
        //            modelResponse.OIGCheckURL = Url.Action("ManageHitsByUserId", "OIG", new { userId = userId });
        //            modelResponse.NoOfHits = Convert.ToInt32(oigResponse?.Content?.NoOfHits);
        //            modelResponse.UserId = model.UserId;
        //            //modelResponse.OIGMessage = "Click here to Verify OIG button to identify the User Profile. You may choose to create the user post verification.";


        //            return Json(modelResponse);
        //        }

        //        else
        //        {
        //            isAddEdit = true;
        //        }
        //    }

        //    else
        //    {
        //        isAddEdit = true;
        //    }

        //    if (isAddEdit)
        //    {
        //        model.Status = model.UserStatus;
        //        model.RequesterUserId = objSessionUser.UserId;

        //        IFormFile uploadedFile = model.ProfilePicformFile;

        //        var apiResponse = await userAPI.AddEditUser(model);

        //        string userId = apiResponse?.Content?.ReadAsStringAsync().Result;
        //        StreamPart profilePicStream = null;
        //        bool isProfilePicSaved = true;
        //        //string msg = "";
        //        if (uploadedFile != null && !string.IsNullOrEmpty(userId))
        //        {
        //            modelResponse.Message = msg;
        //            using (var file = uploadedFile.OpenReadStream())
        //            {
        //                profilePicStream = new StreamPart(file, uploadedFile.FileName, GetContentType(uploadedFile.FileName));
        //                var res = await userAPI.UploadUserProfilePic(userId, profilePicStream);
        //                if (res?.Content?.ReadAsStringAsync().Result != "1")
        //                {
        //                    isProfilePicSaved = false;
        //                    msg += "<br />Problem occured while saving Profile Picture";
        //                }
        //            }

        //        }


        //        modelResponse.UserId = userId;

        //        if (apiResponse != null && apiResponse.IsSuccessStatusCode)
        //        {
        //            if (isProfilePicSaved == true)
        //            {
        //                modelResponse.Message = msg;
        //                modelResponse.IsSuccess = true;
        //            }
        //            else
        //            {
        //                TempData["alertMsg"] = msg;
        //            }

        //            //return RedirectToAction("ManageUsers", "User");
        //            return Json(modelResponse);
        //        }
        //        else
        //        {
        //            //var lstErrors = JsonConvert.DeserializeObject<ErrorResponseVM>(apiResponse?.Error?.Content);
        //            //string msg = "";
        //            //foreach (var singleError in lstErrors.Errors)
        //            //{
        //            //    if (!string.IsNullOrEmpty(singleError.FieldName))
        //            //    {
        //            //        msg += singleError.FieldName + ": " + singleError.Message + "<br />";
        //            //    }
        //            //}
        //            //return View();
        //            modelResponse.IsSuccess = false;
        //            return Json(modelResponse);
        //        }
        //    }
        //    return View();
        //}

        [HttpGet]
        public async Task<IActionResult> GetStateForCountry(string CountryId)
        {
            UserMasterRequestVM model = new UserMasterRequestVM()
            {
                CountryId = CountryId
            };
            var objSessionUser = HttpContext.Session.GetSessionUser();
            var cachedToken = HttpContext.Session.GetBearerToken();
            var userAPI = RestService.For<IUserApi>(hostUrl: ApplicationSettings.WebApiUrl, new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken)
            });

            var apiResponse = await userAPI.CountryStateList(model);
            var response = apiResponse.Content;

            return Json(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetCitiesForState(string StateId)
        {
            UserMasterRequestVM model = new UserMasterRequestVM()
            {
                StateId = StateId
            };
            var objSessionUser = HttpContext.Session.GetSessionUser();
            var cachedToken = HttpContext.Session.GetBearerToken();
            var userAPI = RestService.For<IUserApi>(hostUrl: ApplicationSettings.WebApiUrl, new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken)
            });

            var apiResponse = await userAPI.StateCitiesList(model);
            var response = apiResponse.Content;

            return Json(response);
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        [HttpPost]
        public async Task<IActionResult> ActiveInactiveUsers(string UserIDs, bool TFStatus)
        {
            string[] userIds = UserIDs.TrimEnd(',').Split(',');
            string[] userId = userIds.Distinct().ToArray();
            UserActiveInActiveRequestVM model = new UserActiveInActiveRequestVM();

            var cachedToken = HttpContext.Session.GetBearerToken();
            var userAPI = RestService.For<IUserApi>(hostUrl: ApplicationSettings.WebApiUrl, new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken),

            });
            foreach (string UserId in userIds)
            {
                model.UserId = UserId;
                model.Status = TFStatus;
                var res = await userAPI.ActiveInActiveUserAsync(model);
            }
            if (TFStatus == true)
                TempData["StatusMsg"] = "User(s) activated successfully";
            else if (TFStatus == false)
                TempData["StatusMsg"] = "User(s) inactivated successfully";
            return Json("");
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

       
        public async Task<IActionResult> ChangePassword(string UserId, string ShowLayout = "0")
        {
            ChangePasswordRequestVM model = new ChangePasswordRequestVM();
            var objSessionUser = HttpContext.Session.GetSessionUser();
            var cachedToken = HttpContext.Session.GetBearerToken();
            var userAPI = RestService.For<IUserApi>(hostUrl: ApplicationSettings.WebApiUrl, new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken)
            });

            if (!string.IsNullOrEmpty(UserId))
            {
                GetUserRequestVM updateModel = new GetUserRequestVM();
                updateModel.UserId = UserId;
                updateModel.RequesterUserId = objSessionUser.UserId;
                UserMasterRequestVM userModel = new UserMasterRequestVM();
                var updateResponse = await userAPI.GetUserDataAsync(updateModel);
                userModel = updateResponse.Content;
                model.UserId = userModel.LoginId;
                model.LoginId = UserId;
                model.IsAdminPasswordChangeRequest = true;
                model.FirstName = userModel.FirstName;
                model.LastName = userModel.LastName;
                //model.IsPasswordChanged = userModel.
            }
            else
            {
                model.FirstName = objSessionUser.FirstName;
                model.LastName = objSessionUser.LastName;
                model.UserId = objSessionUser.LoginId;
                model.IsPasswordChanged = objSessionUser.IsPasswordChanged;
            }
            ViewBag.ShowLayout = ShowLayout;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequestVM model)
        {
            var objSessionUser = HttpContext.Session.GetSessionUser();
            var cachedToken = HttpContext.Session.GetBearerToken();
            var userAPI = RestService.For<IUserApi>(hostUrl: ApplicationSettings.WebApiUrl, new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken)
            });
            bool IsAdmin = model.IsAdminPasswordChangeRequest;
            if (!model.IsAdminPasswordChangeRequest)
            {
                model.LoginId = objSessionUser.LoginId;
                model.UserId = objSessionUser.UserId;
            }

            model.RequesterUserId = objSessionUser.UserId;

            model.IsPasswordChanged = IsAdmin == true ? false : true;
            var apiResponse = await userAPI.ChangePassword(model);

            string msg = "";
            if (apiResponse.IsSuccessStatusCode)
            {
                msg = "success";

            }
            else
            {
                var lstErrors = JsonConvert.DeserializeObject<ErrorResponseVM>(apiResponse?.Error?.Content);

                foreach (var singleError in lstErrors.Errors)
                {
                    if (!string.IsNullOrEmpty(singleError.FieldName))
                    {
                        msg += singleError.Message + "<br />";
                    }
                }


            }
            var result = new { Message = msg, IsAdmin = IsAdmin };
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadUserData()
        {
            UserListRequestVM request = HttpContext.Session.GetObject<UserListRequestVM>("SearchUser");
            var cachedToken = HttpContext.Session.GetBearerToken();
            var userAPI = RestService.For<IUserApi>(hostUrl: ApplicationSettings.WebApiUrl, new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken),

            });

            var res = await userAPI.ExportUserData(request);
            byte[] arrByte = await res.Content.ReadAsByteArrayAsync();
            string fileName = "User Data_" + DateTime.Now.ToString("MM-dd-yyyy hh mm ss") + ".xlsx";
            return File(arrByte, GetContentType(fileName), fileName);
        }


        [HttpGet]
        public IActionResult UserRoleListByUserId(string UserId, string FirstName, string LastName)
        {
            ViewBag.UserId = UserId;
            ViewBag.Name = string.Concat(FirstName, " ", LastName);
            return PartialView("_UserRoleList");
        }

        [HttpGet]
        public async Task<IActionResult> UserRoleListData(GetUserRequestVM model)
        {
            var cachedToken = HttpContext.Session.GetBearerToken();
            var userAPI = RestService.For<IUserApi>(hostUrl: ApplicationSettings.WebApiUrl, new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken)
            });
            var apiResponse = await userAPI.GetRoleListByUserId(model);
            var response = apiResponse.Content;

            return PartialView("_UserRoleListData", response);
        }

        //[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        //[HttpGet]
        //public IActionResult EnrollByOrderCode()
        //{
        //    UserEnrollmentRequestVM model = new UserEnrollmentRequestVM();
        //    return PartialView("_EnrollByOrderCode", model);
        //}

        //[HttpGet]
        //public async Task<IActionResult> IsUserEnrolled(string OrderCode)
        //{
        //    var objSessionUser = HttpContext.Session.GetSessionUser();
        //    var cachedToken = HttpContext.Session.GetBearerToken();
        //    var userAPI = RestService.For<IUserApi>(hostUrl: ApplicationSettings.WebApiUrl, new RefitSettings
        //    {
        //        AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken)
        //    });

        //    var apiResponse = await userAPI.IsUserEnrolledByOrderCode(OrderCode, objSessionUser.UserId);
        //    var res = apiResponse.Content.ReadAsStringAsync().Result;

        //    return Json(res);
        //}

        //[HttpPost]
        //public async Task<IActionResult> EnrollUserByOrderCode(string OrderCode)
        //{
        //    var objSessionUser = HttpContext.Session.GetSessionUser();
        //    var cachedToken = HttpContext.Session.GetBearerToken();
        //    var userAPI = RestService.For<IUserApi>(hostUrl: ApplicationSettings.WebApiUrl, new RefitSettings
        //    {
        //        AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken)
        //    });

        //    var apiResponse = await userAPI.EnrollUserByOrderCode(OrderCode, objSessionUser.UserId);
        //    if (apiResponse.StatusCode == System.Net.HttpStatusCode.OK)
        //        return Json("1");

        //    return Json("0");
        //}

        //[HttpGet]
        //public async Task<JsonResult> IsOrderCodeExists(string OrderCode)
        //{
        //    var objSessionUser = HttpContext.Session.GetSessionUser();
        //    var cachedToken = HttpContext.Session.GetBearerToken();
        //    var userAPI = RestService.For<IUserApi>(hostUrl: ApplicationSettings.WebApiUrl, new RefitSettings
        //    {
        //        AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken)
        //    });

        //    var res = "";
        //    var apiResponse = await userAPI.IsOrderCodeExists(OrderCode);
        //    if (apiResponse != null)
        //    {
        //        res = apiResponse.Content.ReadAsStringAsync().Result;
        //    }

        //    return Json(res);
        //}

        //[HttpPost]
        //public async Task<IActionResult> RemoveGroupAdmin(string UserRoleId)
        //{
        //    var objSessionUser = HttpContext.Session.GetSessionUser();
        //    var cachedToken = HttpContext.Session.GetBearerToken();
        //    var userAPI = RestService.For<IUserApi>(hostUrl: ApplicationSettings.WebApiUrl, new RefitSettings
        //    {
        //        AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken)
        //    });

        //    await userAPI.RemoveGroupAdmin(UserRoleId);

        //    return Json("1");
        //}

        //[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        //[HttpGet]
        //public IActionResult EntityListToMakeAdmin()
        //{
        //    return PartialView("_EntityListToMakeAdmin");
        //}

        //[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        //public async Task<IActionResult> FillTableEntityListToMakeAdmin([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest dt, string UserId = "")
        //{
        //    var objSessionUSer = HttpContext.Session.GetSessionUser();

        //    List<ColumnNameValue> colList = new List<ColumnNameValue>();
        //    ColumnNameValue columns = new ColumnNameValue();

        //    int iPageSize;
        //    string sortdir = " asc ";
        //    int TotalCount = 0;

        //    if (dt.Length <= 0)
        //        iPageSize = 10;
        //    else
        //        iPageSize = dt.Length;

        //    if (dt.SortDirection.Equals(Column.OrderDirection.Ascendant))
        //        sortdir = " asc ";
        //    else
        //        sortdir = " desc ";

        //    SearchEntityRequestVM searchObj = new SearchEntityRequestVM();
        //    searchObj.RequesterUserId = objSessionUSer.UserId;
        //    searchObj.UserId = UserId;
        //    if (!string.IsNullOrEmpty(dt.SortColumnName))
        //        searchObj.SortExp = dt.SortColumnName + " " + sortdir;

        //    searchObj.PageSize = iPageSize;

        //    if (dt.PageIndex == 0)
        //        searchObj.PageIndex = 1;
        //    else
        //        searchObj.PageIndex = dt.PageIndex;


        //    foreach (var col in dt.Columns)
        //    {
        //        columns = new ColumnNameValue();
        //        if (!string.IsNullOrEmpty(col.Search.Value))
        //        {
        //            columns.ColName = col.Name;
        //            columns.ColValue = col.Search.Value;
        //            colList.Add(columns);
        //            switch (col.Name)
        //            {
        //                case "GroupName":
        //                    if (!string.IsNullOrEmpty(col.Search.Value))
        //                    {
        //                        searchObj.GroupName = col.Search.Value.Trim();
        //                    }
        //                    break;
        //                case "UserId":
        //                    if (!string.IsNullOrEmpty(col.Search.Value))
        //                    {
        //                        searchObj.UserId = col.Search.Value.Trim();
        //                    }
        //                    break;
        //            }
        //        }
        //    }

        //    var cachedToken = HttpContext.Session.GetBearerToken();


        //    var userAPI = RestService.For<IUserApi>(hostUrl: ApplicationSettings.WebApiUrl, new RefitSettings
        //    {
        //        AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken)
        //    });

        //    var apiResponse = await userAPI.EntityListToMakeAdmin(searchObj);
        //    var response = apiResponse.Content;

        //    var lst = response;

        //    var lst1 = lst?.ToList();

        //    if (lst1 != null && lst1.Count > 0)
        //    {
        //        TotalCount = lst1[0].TotalCount;
        //    }
        //    bool set = false;
        //    if (searchObj.PageIndex > 1 && TotalCount == 0)
        //        set = true;
        //    return Json(new DataTablesResponse(dt.Draw, lst1, TotalCount, TotalCount, colList, set));
        //}

        //[HttpPost]
        //public async Task<IActionResult> MakeGroupAdmin(string GroupId, string UserId, int GroupLevel)
        //{
        //    var objSessionUser = HttpContext.Session.GetSessionUser();
        //    var cachedToken = HttpContext.Session.GetBearerToken();
        //    var GroupApi = RestService.For<IGroupApi>(hostUrl: ApplicationSettings.WebApiUrl, new RefitSettings
        //    {
        //        AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken)
        //    });

        //    string roleId = string.Concat("Level", GroupLevel.ToString(), "Admin");
        //    SystemRoleCreateUpdateRequestVM request = new SystemRoleCreateUpdateRequestVM();
        //    request.RoleId = roleId;
        //    request.UserId = UserId;
        //    request.GroupId = GroupId;
        //    request.RequesterUserId = objSessionUser.UserId;

        //    var apiResponse = await GroupApi.AssignRole(request);
        //    string UserRoleId = apiResponse.Content?.ReadAsStringAsync().Result;

        //    return Json(UserRoleId);
        //}

        

        //[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        //[HttpGet]
        //public IActionResult WelcomeEmailForNotLoggedInUsers()
        //{
        //    return View("_WelcomeEmailForNotLoggedInUsers");
        //}

        //[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        //[HttpGet]
        //public IActionResult SendWelcomeEmailForNotLoggedInUsers()
        //{
        //    var objSessionUser = HttpContext.Session.GetSessionUser();
        //    var cachedToken = HttpContext.Session.GetBearerToken();
        //    var userApi = RestService.For<IUserApi>(hostUrl: ApplicationSettings.WebApiUrl, new RefitSettings
        //    {
        //        AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken)
        //    });
        //    userApi.WelcomeEmailForNotLoggedInUsers(objSessionUser.UserId);

        //    return Json('1');
        //}

        //[HttpGet]
        //public async Task<IActionResult> SearchEntityWithDescr(string Prefix)
        //{
        //    var objSessionUser = HttpContext.Session.GetSessionUser();
        //    var cachedToken = HttpContext.Session.GetBearerToken();
        //    var userAPI = RestService.For<IUserApi>(hostUrl: ApplicationSettings.WebApiUrl, new RefitSettings
        //    {
        //        AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken)
        //    });

        //    var apiResponse = await userAPI.GroupListWithDescr(RequesterUserId: objSessionUser.UserId, SearchPhrase: Prefix, GroupId: "");

        //    var response = apiResponse.Content;
        //    return Json(response);

        //    //ViewBag.Response = response;
        //    //return PartialView("_ViewEntityTree", response); //Json(response);
        //}
    }
}

