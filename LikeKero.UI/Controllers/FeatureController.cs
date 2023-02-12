using DataTables.Mvc;
using LikeKero.UI.Models;
using LikeKero.UI.RefitClientFactory;
using LikeKero.UI.Utility;
using LikeKero.UI.ViewModels.Request.Course;
using LikeKero.UI.ViewModels.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LikeKero.UI.Controllers
{
    public class FeatureController : Controller
    {
        private readonly IOptions<MySettingsModel> _options;
        public FeatureController(IOptions<MySettingsModel> options)
        {
            _options = options;
        }

        public async Task<IActionResult> ManageAdminRights()
        {
            var objSessionUser = HttpContext.Session.GetSessionUser();
            var cachedToken = HttpContext.Session.GetBearerToken();
            var AdminRightsAPI = RestService.For<IAdminRightsApi>(hostUrl: ApplicationSettings.WebApiUrl, new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken)
            });
            var response = await AdminRightsAPI.GetAdminFeatures(objSessionUser.UserId);
            List<RoleFeatureResponseVM> roleFeatureResponseList = new List<RoleFeatureResponseVM>();
            if (response != null)
            {
                roleFeatureResponseList = response.Content.RoleFeatureMasterList;
            }
            return View(roleFeatureResponseList);
        }

        [HttpPost]
        public async Task<JsonResult> SaveAdminRights(AdminRightsRequestVM singleFeature)
        {
            var objSessionUser = HttpContext.Session.GetSessionUser();
            var cachedToken = HttpContext.Session.GetBearerToken();
            var AdminRightsAPI = RestService.For<IAdminRightsApi>(hostUrl: ApplicationSettings.WebApiUrl, new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken)
            });
            singleFeature.RequesterUserId = objSessionUser.UserId;
            var response = await AdminRightsAPI.SaveAdminRights(singleFeature);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Json(new { StatusCode = "1" });
            }
            else
            {
                string msg = "Error while " + (singleFeature.Status ? "assigning" : "unassigning") + " feature";
                return Json(new { msg = msg, StatusCode = "0" });
            }
        }

        //public IActionResult ChartExample()
        //{
        //    string[] Catg = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
        //    List<Series> chartSeries = new List<Series>();
        //    object[] objPoints = new object[Catg.Length];
        //    for (int Seq = 1; Seq <= Catg.Length; Seq++)//Months
        //    {
        //        objPoints[Seq - 1] = new Random();
        //    }
        //    Series series = new Series();
        //    series.Name = "TEST Series";
        //    series.Type = ChartTypes.Column;
        //    series.Data = new Data(objPoints);

        //    chartSeries.Add(series);

        //    Highcharts chart = new Highcharts("chart_KPIReport")
        //       .SetCredits(new Credits { Enabled = false })
        //       .SetExporting(new Exporting
        //       {
        //           Enabled = true
        //       })
        //       .InitChart(new Chart { Type = ChartTypes.Column, ZoomType = ZoomTypes.Xy, SpacingBottom = 50 })
        //       .SetTitle(new Title { Text = " " })
        //       .SetXAxis(new XAxis { Categories = Catg, Labels = new XAxisLabels { /*Rotation = -45*/ }, Title = new XAxisTitle { Text = "" } })
        //       .SetYAxis(new[]
        //       {
        //            new YAxis
        //            {
        //                Title = new YAxisTitle { Text = "" },ReversedStacks=false
        //            }
        //       })
        //       .SetLegend(new Legend
        //       {
        //           Layout = Layouts.Horizontal,
        //           Align = HorizontalAligns.Center,
        //           VerticalAlign = VerticalAligns.Bottom,
        //           Y = 40,
        //           Floating = true,
        //           Enabled = true,
        //       })
        //       .SetPlotOptions(new PlotOptions
        //       {
        //           Column = new PlotOptionsColumn
        //           {
        //               Stacking = Stackings.Normal,
        //               DataLabels = new PlotOptionsColumnDataLabels() { Enabled = true, Crop = false, Overflow = "none", Inside = false }
        //           }
        //       })
        //       .SetSeries(chartSeries.ToArray());

        //    ViewBag.Chart = chart;

        //    return View();
        //}
    }
}