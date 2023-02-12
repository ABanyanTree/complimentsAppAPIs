
using LikeKero.UI.Models;
using LikeKero.UI.RefitClientFactory;
using LikeKero.UI.Utility;
using LikeKero.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Refit;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using LikeKero.UI.Extensions;
using System;

namespace LikeKero.UI.ActionFilters
{
    public class TokenRereshFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            AuthSuccessResponseVM user = null;
            var _ContextAccesser = context.HttpContext.RequestServices.GetRequiredService<IHttpContextAccessor>();
            var _AppSettings = context.HttpContext.RequestServices.GetRequiredService<IOptions<MySettingsModel>>();

            if (_ContextAccesser.HttpContext.Session.Get<AuthSuccessResponseVM>("UserObject") != null)
            {
                user = _ContextAccesser.HttpContext.Session.Get<AuthSuccessResponseVM>("UserObject");
                if (user.ExpiryDate < DateTime.UtcNow)
                {

                    RefreshTokenRequestVM refreshTokenRequest = new RefreshTokenRequestVM();
                    refreshTokenRequest.RefreshToken = user.RefreshToken;
                    refreshTokenRequest.Token = user.Token;
                    var iloginApi = RestService.For<IUserApi>(hostUrl: _AppSettings.Value.WebApiBaseUrl);
                    // var iloginApi = RestService.For<IUserApi>(hostUrl: "https://localhost:44322");
                    var apiResponse = await iloginApi.Refresh(refreshTokenRequest).ConfigureAwait(false);
                    if (apiResponse.IsSuccessStatusCode && apiResponse != null && apiResponse.Content != null)
                    {
                        AuthSuccessResponseVM AuthSuccessResponseVM = apiResponse.Content;
                        user.RefreshToken = AuthSuccessResponseVM.RefreshToken;
                        user.ExpiryDate = AuthSuccessResponseVM.ExpiryDate;
                        user.Token = AuthSuccessResponseVM.Token;

                        _ContextAccesser.HttpContext.Session.Set<AuthSuccessResponseVM>("UserObject", user);
                    }
                }
            }
            else
            {
                //***TODO: Deepak: review pending by Amit Sir | Made changes to ignore NblsRetail/Signup url
                string controllerName = context.RouteData.Values["controller"].ToString();
                string actionName = context.RouteData.Values["action"].ToString();
                if (controllerName?.Contains("Login") == false && controllerName?.Contains("Error") == false
                    && !((controllerName.Contains("NblsRetail") && (actionName.Contains("Signup") || actionName.Contains("Login")))))
                {
                    context.Result = new RedirectToActionResult("Error", "Error", null);
                }
            }
            if (next != null)
            {
                var resultContext = await next();                
            }
        }
    }
}
