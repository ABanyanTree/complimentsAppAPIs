using DataTables.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Refit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LikeKero.UI.RefitClientFactory;
using LikeKero.UI.Utility;
using LikeKero.UI.ViewModels;
using System;


namespace LikeKero.UI.Controllers
{
    public class TestCustomerController : Controller
    {

        private readonly ILogger<ErrorController> Logger;

        public TestCustomerController(ILogger<ErrorController> _Logger)
        {
            Logger = _Logger;
        }

        public async Task<ActionResult> ManageTestCustomer()
        {

            Logger.LogInformation(" Open Manage Test Customer Page");

            SearchTestCustomerRequestVM obj = new SearchTestCustomerRequestVM();
          
            return View(obj);
        }

        [HttpPost("/TestCustomer/FillTableTestCustomerLstAsync")]
        public async Task<IActionResult> FillTableTestCustomerLstAsync([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest dt, string Active = "1")
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

            SearchTestCustomerRequestVM searchObj = new SearchTestCustomerRequestVM();
            searchObj.RequesterUserId = objSessionUSer.UserId;
            //if (Active == "1")
            //    searchObj.Status = true;

            if (!string.IsNullOrEmpty(dt.SortColumnName))
                searchObj.SortExp = dt.SortColumnName + " " + sortdir;

            searchObj.PageSize = iPageSize;

            if (dt.PageIndex == 0)
                searchObj.PageIndex = 1;
            else
                searchObj.PageIndex = dt.PageIndex;

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
                    }
                }
            }

            var cachedToken = HttpContext.Session.GetBearerToken();

            var TestCustomerAPIApi = RestService.For<ITestCustomerAPI>(hostUrl: ApplicationSettings.WebApiUrl, new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken)
            });
            var apiResponse = await TestCustomerAPIApi.GetAllTestCustomer(searchObj);
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
           // return Json(new DataTablesResponse(dt.Draw, lst1, TotalCount, TotalCount, colList, set), new JsonSerializerOptions { PropertyNamingPolicy = null });
            return Json(new DataTablesResponse(dt.Draw, lst1, TotalCount, TotalCount, colList, set));
        }

        [HttpGet]
        public async Task<IActionResult> AddEditTestCustomer(string CustID)
        {
            var objSessionUser = HttpContext.Session.GetSessionUser();
            var cachedToken = HttpContext.Session.GetBearerToken();
            var TestCustomerAPI = RestService.For<ITestCustomerAPI>(hostUrl: ApplicationSettings.WebApiUrl, new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken)
            });

            CreateTestCustomerRequestVM model = new CreateTestCustomerRequestVM();            
            if (!string.IsNullOrEmpty(CustID))
            {
                var updateResponse = await TestCustomerAPI.GetTestCustomer(CustID);
                model = updateResponse.Content;
            }
            return PartialView("_AddEditTestCustomer", model);
        }

        [HttpPost]
        //[RequestFormLimits(MultipartBodyLengthLimit = 2147483647)]
        //[RequestSizeLimit(2147483647)]
        public async Task<IActionResult> AddEditTestCustomer(CreateTestCustomerRequestVM model)
        {

            bool IsNewRecord = string.IsNullOrEmpty(model.CustID) ? true : false;
            CreateTestCustomerRequestVM modelobj = new CreateTestCustomerRequestVM();

            var objSessionUSer = HttpContext.Session.GetSessionUser();
            var cachedToken = HttpContext.Session.GetBearerToken();
            var ITestCustomerAPI = RestService.For<ITestCustomerAPI>(hostUrl: ApplicationSettings.WebApiUrl, new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken),

            });
            
            var apiResponse = await ITestCustomerAPI.SaveTestCustomer(model);
            string CustID = apiResponse?.Content?.ReadAsStringAsync().Result;

            if (apiResponse != null && apiResponse.IsSuccessStatusCode)
            {
                HttpContext.Session.Remove("TestCustID");
                HttpContext.Session.SetObject("TestCustID", CustID);

                if (model.uploadedFile != null)
                {
                    foreach (var item in model.uploadedFile)
                    {
                        StreamPart CourseFileStream = null;
                        IFormFile uploadedFile = item;
                        if (uploadedFile != null)
                        {
                            var file = uploadedFile.OpenReadStream();
                            CourseFileStream = new StreamPart(file, uploadedFile.FileName, CommonMethods.GetContentType(uploadedFile.FileName));
                        }
                        var res = await ITestCustomerAPI.UploadTestCustomerFile(CustID, CourseFileStream);
                        var result = res?.Content?.ReadAsStringAsync().Result;
                    }
                    //var updateResponse = await IEventApi.GetEvent(EventId, objSessionUSer.UserId);
                    //modelobj = updateResponse.Content;
                }



                string msg = IsNewRecord ? "Test customer added successfully." : "Test customer updated successfully.";
                return Json(new { custID = CustID, isSuccess = true, message = msg });
            }
            else
            {              
                Logger.LogError("Error while Add new customer ---> " + CustID );
                return Json(new { custID = 0, isSuccess = false, message = "" });
            }

        }

        public async Task<IActionResult> UploadTestCustomerFiles(string CustID)
        {
            var cachedToken = HttpContext.Session.GetBearerToken();
            var ITestCustomerAPI = RestService.For<ITestCustomerAPI>(hostUrl: ApplicationSettings.WebApiUrl, new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken),

            });

            string TestCustID = HttpContext.Session.GetObject<string>("TestCustID");


            CreateTestCustomerRequestVM model = new CreateTestCustomerRequestVM();
            model.CustID = TestCustID;
            model.uploadedFile = (List<IFormFile>)Request.Form.Files;


            if (model.uploadedFile.Count > 0)
            {
                foreach (var item in model.uploadedFile)
                {
                    StreamPart CourseFileStream = null;
                    IFormFile uploadedFile = item;
                    if (uploadedFile != null)
                    {
                        var file = uploadedFile.OpenReadStream();
                        CourseFileStream = new StreamPart(file, uploadedFile.FileName, CommonMethods.GetContentType(uploadedFile.FileName));
                    }
                    var res = await ITestCustomerAPI.UploadTestCustomerFile(TestCustID, CourseFileStream);
                    var result = res?.Content?.ReadAsStringAsync().Result;
                }               
            }
            var data = "";

            // return Json(data, JsonRequestBehavior.AllowGet);
            return Json(data);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTestCustomer(string CustID)
        {          
            CreateTestCustomerRequestVM modelobj = new CreateTestCustomerRequestVM();

            var objSessionUSer = HttpContext.Session.GetSessionUser();
            var cachedToken = HttpContext.Session.GetBearerToken();
            var ITestCustomerAPI = RestService.For<ITestCustomerAPI>(hostUrl: ApplicationSettings.WebApiUrl, new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken),

            });
            var apiResponse = await ITestCustomerAPI.DeleteTestCustomer(CustID);            

            if (apiResponse != null && apiResponse.IsSuccessStatusCode)
            {                
                return Json(new { isSuccess = true });
            }
            else
            {
                return Json(new {  isSuccess = false });
            }

        }


    }
}

