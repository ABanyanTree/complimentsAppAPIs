using LikeKero.UI.Models;
using LikeKero.UI.RefitClientFactory;
using LikeKero.UI.Utility;
using LikeKero.UI.ViewModels;
//using LikeKero.UI.ViewModels.Response.Dashboard;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Refit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LikeKero.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOptions<MySettingsModel> appSettings;

        public HomeController(IOptions<MySettingsModel> app)
        {
            appSettings = app;
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> Dashboard()
        {
            //HttpContext.Session.SetObject("LEVEL1_MENU", "Dashboard");

            //var objSessionUSer = HttpContext.Session.GetSessionUser();
            //objSessionUSer.IsOnLearnerSide = false;
            //HttpContext.Session.SetSessionUser(objSessionUSer);
            //ViewBag.LoggedInUser = objSessionUSer.FirstName;

            //var cachedToken = HttpContext.Session.GetBearerToken();

            //var courseApi = RestService.For<ICourseApi>(hostUrl: ApplicationSettings.WebApiUrl, new RefitSettings
            //{
            //    AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken)
            //});

            //var apiGetLookupResponse = await courseApi.GetCoursesForDropDownDashboard(objSessionUSer.UserId, false);
            //ViewBag.CourseList = new SelectList(apiGetLookupResponse.Content, "CourseId", "CourseName");

            //var apiGetACPELookupResponse = await courseApi.GetCoursesForDropDownDashboard(objSessionUSer.UserId, true);
            //ViewBag.ACPECourseList = new SelectList(apiGetACPELookupResponse.Content, "CourseId", "CourseName");


            //var oigAPi = RestService.For<IOIGApi>(hostUrl: ApplicationSettings.WebApiUrl, new RefitSettings
            //{
            //    AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken)
            //});

            //var apiYearResponse = await oigAPi.GetYears(10);
            //var selectedYear = apiYearResponse.Content.OrderByDescending(x => x.LookUpValue).Select(x => x.LookUpValue).FirstOrDefault();
            //ViewBag.YearList = new SelectList(apiYearResponse.Content, "LookUpValue", "LookUpName", selectedYear);

            //ViewBag.showLeaderNetVsMSIGraph = appSettings.Value.ShowLeaderNetVsMSIGraph;

            return View();
        }

      
    }


}

