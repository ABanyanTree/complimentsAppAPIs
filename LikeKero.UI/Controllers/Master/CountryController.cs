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
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LikeKero.UI.Controllers
{
    public class CountryController : Controller
    {

        public CountryController()
        {
           
        }

        public async Task<ActionResult> ManageCountry()
        {
            SearchCountryRequestVM obj = new SearchCountryRequestVM();
          
            return View(obj);
        }

        [HttpPost("/Country/FillTableCountryLstAsync")]
        public async Task<IActionResult> FillTableCountryLstAsync([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest dt, string Active = "1")
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

            SearchCountryRequestVM searchObj = new SearchCountryRequestVM();
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
                        case "CountryName":
                            if (!string.IsNullOrEmpty(col.Search.Value))
                            {
                                searchObj.CountryName = col.Search.Value.Trim();
                            }
                            break;
                       
                        case "CountryDescription":
                            if (!string.IsNullOrEmpty(col.Search.Value))
                            {
                                searchObj.CountryDescription = col.Search.Value.Trim();
                            }
                            break;
                    }
                }
            }

            var cachedToken = HttpContext.Session.GetBearerToken();

            var CountryAPI = RestService.For<ICountryAPI>(hostUrl: ApplicationSettings.WebApiUrl, new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken)
            });
            var apiResponse = await CountryAPI.GetAllCountryList(searchObj);
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

        [HttpGet]
        public async Task<IActionResult> AddEditCountry(string CountryID)
        {

            var objSessionUser = HttpContext.Session.GetSessionUser();
            var cachedToken = HttpContext.Session.GetBearerToken();
            var CountryAPI = RestService.For<ICountryAPI>(hostUrl: ApplicationSettings.WebApiUrl, new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken)
            });

            var RegionAPI = RestService.For<IRegionAPI>(hostUrl: ApplicationSettings.WebApiUrl, new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken)
            });

            CreateCountryRequestVM model = new CreateCountryRequestVM();
            model.IsActive = true;
            if (!string.IsNullOrEmpty(CountryID))
            {
                var updateResponse = await CountryAPI.GetCountry(CountryID);
                model = updateResponse.Content;
            }
            
            var RegionResponse = await RegionAPI.GetAllRegion();
            ViewBag.lstRegion = new SelectList(RegionResponse.Content, "RegionID", "RegionName");

            return PartialView("_AddEditCountry", model);
        }

        [HttpPost]
        public async Task<IActionResult> AddEditCountry(CreateCountryRequestVM model)
        {

            bool IsNewRecord = string.IsNullOrEmpty(model.CountryID) ? true : false;
            CreateCountryRequestVM modelobj = new CreateCountryRequestVM();

            var objSessionUSer = HttpContext.Session.GetSessionUser();
            var cachedToken = HttpContext.Session.GetBearerToken();
            var CountryAPI = RestService.For<ICountryAPI>(hostUrl: ApplicationSettings.WebApiUrl, new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken),

            });
            model.RequesterUserId = objSessionUSer.UserId;
            var apiResponse = await CountryAPI.AddEditCountry(model);
            string CountryID = apiResponse?.Content?.ReadAsStringAsync().Result;

            if (apiResponse != null && apiResponse.IsSuccessStatusCode)
            {
                string msg = IsNewRecord ? "Country added successfully." : "Country updated successfully.";
                return Json(new { CountryID = CountryID, isSuccess = true, message = msg });
            }
            else
            {              
              
                return Json(new { CountryID = 0, isSuccess = false, message = "" });
            }

        }       

        [HttpDelete]
        public async Task<IActionResult> DeleteCountry(string CountryID)
        {          
            CreateCountryRequestVM modelobj = new CreateCountryRequestVM();

            var objSessionUSer = HttpContext.Session.GetSessionUser();
            var cachedToken = HttpContext.Session.GetBearerToken();
            var iCountryAPI = RestService.For<ICountryAPI>(hostUrl: ApplicationSettings.WebApiUrl, new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken),

            });
            var apiResponse = await iCountryAPI.DeleteCountry(CountryID);            

            if (apiResponse != null && apiResponse.IsSuccessStatusCode)
            {                
                return Json(new { isSuccess = true });
            }
            else
            {
                return Json(new {  isSuccess = false });
            }

        }


        [HttpPost]
        public async Task<IActionResult> IsInUseCount(string CountryID)
        {
            CreateCountryRequestVM modelobj = new CreateCountryRequestVM();

            var objSessionUSer = HttpContext.Session.GetSessionUser();
            var cachedToken = HttpContext.Session.GetBearerToken();
            var CountryAPI = RestService.For<ICountryAPI>(hostUrl: ApplicationSettings.WebApiUrl, new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken),

            });
            var apiResponse = await CountryAPI.IsInUseCount(CountryID);
            string Count = apiResponse?.Content?.ReadAsStringAsync().Result;

            if (apiResponse != null && apiResponse.IsSuccessStatusCode)
            {
                return Json(new { isSuccess = true, Count = Count });
            }
            else
            {
                return Json(new { isSuccess = false, Count = 0 });
            }

        }


        [HttpGet]
        public async Task<JsonResult> IsCountryNameInUse(string CountryID, string CountryName)
        {
            if (CountryName == null || string.IsNullOrEmpty(CountryName.Trim()))
            {
                return Json("Please enter Country Name");
            }
            else
            {
                var objSessionUser = HttpContext.Session.GetSessionUser();
                var cachedToken = HttpContext.Session.GetBearerToken();
                var CountryAPI = RestService.For<ICountryAPI>(hostUrl: ApplicationSettings.WebApiUrl, new RefitSettings
                {
                    AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken)
                });

                var res = "";
                var apiResponse = await CountryAPI.IsCountryNameInUse(CountryName.Trim(), CountryID);
                if (apiResponse != null)
                {
                    res = apiResponse.Content.ReadAsStringAsync().Result;
                }

                return Json(res);
            }
        }


    }
}

