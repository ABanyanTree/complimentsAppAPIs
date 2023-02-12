using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataTables.Mvc;
using LikeKero.UI.RefitClientFactory;
using LikeKero.UI.Utility;
using LikeKero.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace LikeKero.UI.Controllers
{
    public class ErrorLogController : Controller
    {
        public IActionResult ManageErrorLog()
        {
            return View();
        }
        [HttpPost("/ErrorLog/FillTableErrorLogAsync")]
        public async Task<IActionResult> FillTableErrorLogAsync([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest dt)
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

            ErrorLogRequestVM searchObj = new ErrorLogRequestVM();
            searchObj.RequesterUserId = objSessionUSer.UserId;

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
                        case "ErrorFromDate":
                            if (!string.IsNullOrEmpty(col.Search.Value))
                            {
                                searchObj.ErrorFromDate = Convert.ToDateTime(col.Search.Value);
                            }
                            break;
                        case "ErrorToDate":
                            if (!string.IsNullOrEmpty(col.Search.Value))
                            {
                                searchObj.ErrorToDate = Convert.ToDateTime(col.Search.Value);
                            }
                            break;
                        case "ErrorMessage":
                            if (!string.IsNullOrEmpty(col.Search.Value))
                            {
                                searchObj.ErrorMessage = col.Search.Value.Trim();
                            }
                            break;
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


            var userAPI = RestService.For<IErrorLogApi>(hostUrl: ApplicationSettings.WebApiUrl, new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken)
            });

            var apiResponse = await userAPI.GetErrorLogList(searchObj);
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


        public async Task<IActionResult> ErrorLogDetails(string ErrorLogID)
        {
            ErrorLogGetRequestVM model = new ErrorLogGetRequestVM()
            {
                ErrorLogID = ErrorLogID
            };
            var objSessionUser = HttpContext.Session.GetSessionUser();
            var cachedToken = HttpContext.Session.GetBearerToken();
            var userAPI = RestService.For<IErrorLogApi>(hostUrl: ApplicationSettings.WebApiUrl, new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken)
            });

            var apiResponse = await userAPI.GetErrorLog(model);
            var response = apiResponse.Content;

            //return View(response);
            return PartialView("ErrorLogDetails", response);
        }
    }
}