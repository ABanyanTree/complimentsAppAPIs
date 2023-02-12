using DataTables.Mvc;
using LikeKero.UI.RefitClientFactory;
using LikeKero.UI.Utility;
using LikeKero.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LikeKero.UI.Controllers
{
    public class EmailLogController : Controller
    {
        public IActionResult ManageEmailLog()
        {
            return View();
        }

        public async Task<IActionResult> FillEmailLogList([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest dt)
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

            EmailSentLogRequestVM searchObj = new EmailSentLogRequestVM();
            //searchObj.RequesterUserId = objSessionUSer.UserId;

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
                        case "EmailTo":
                            if (!string.IsNullOrEmpty(col.Search.Value))
                            {
                                searchObj.EmailTo = col.Search.Value.Trim();
                            }
                            break;
                        case "Subject":
                            if (!string.IsNullOrEmpty(col.Search.Value))
                            {
                                searchObj.Subject = col.Search.Value.Trim();
                            }
                            break;
                        case "ActualSentDate":
                            if (!string.IsNullOrEmpty(col.Search.Value))
                            {
                                searchObj.ActualSentDate = Convert.ToDateTime(col.Search.Value.Trim());
                            }
                            break;
                    }
                }
            }

            var cachedToken = HttpContext.Session.GetBearerToken();

           
            var emailAPI = RestService.For<IEmailApi>(hostUrl: ApplicationSettings.WebApiUrl, new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken)
            });

            searchObj.RequesterUserId = objSessionUSer.UserId;
            var apiResponse = await emailAPI.EmailLogList(searchObj);
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

        public async Task<IActionResult> GetEmailDetails(string EmailLogId)
        {
            EmailGetRequestVM model = new EmailGetRequestVM();
            model.EmailLogId = EmailLogId;

            var cachedToken = HttpContext.Session.GetBearerToken();
            var emailAPI = RestService.For<IEmailApi>(hostUrl: ApplicationSettings.WebApiUrl, new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken)
            });

            var apiResponse = await emailAPI.GetEmailLog(model);
            var response = apiResponse.Content;
            EmailGetResponseVM responseModel = new EmailGetResponseVM();
            responseModel = response;
            return PartialView("_ViewEmailDetails", responseModel);
        }
    }
}