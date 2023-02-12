using LikeKero.Domain;
using LikeKero.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace LikeKero.Api.Filters
{
    public class ExceptionActionFilter : ExceptionFilterAttribute
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IErrorLogs _errorLogs;

        public ExceptionActionFilter(
            IWebHostEnvironment hostingEnvironment, IErrorLogs errorLogs
            )
        {
            _hostingEnvironment = hostingEnvironment;
            _errorLogs = errorLogs;

        }

        #region Overrides of ExceptionFilterAttribute

        public override void OnException(ExceptionContext context)
        {
            var actionDescriptor = (Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor;
            Type controllerType = actionDescriptor.ControllerTypeInfo;

            var controllerBase = typeof(ControllerBase);
            var controller = typeof(Controller);

            var action = actionDescriptor.ActionName;
            var controllername = actionDescriptor.ControllerName;

            var InnerException = context.Exception.InnerException;
            var Message = context.Exception.Message;
            var StackTrace = context.Exception.StackTrace;

            string loggedInUserId = "";
            if (context?.HttpContext?.User?.HasClaim(x => x.Type == "id") == true)
            {
                loggedInUserId = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "id").Value;
            }
            if (!string.IsNullOrEmpty(Message)
                && !Message.ToLower().Contains("the client has disconnected")
                && !Message.ToLower().Contains("the sam server returned an error"))
            {
                ErrorLogs errorLogs = new ErrorLogs()
                {
                    ControllerName = controllername,
                    ActionName = action,
                    ErrorMessage = Message,
                    InnerException = InnerException?.ToString(),
                    StackTrace = StackTrace?.ToString(),
                    RequesterUserId = loggedInUserId
                };

                _errorLogs.AddEditAsync(errorLogs);
            }
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.HttpContext.Response.ContentType = "application/json";
            context.Result = new JsonResult(context.Exception.Message);



            // Report exception to insights - Here comes our ErrorService to write in the Database
            //_telemetryClient.TrackException(context.Exception);
            //_telemetryClient.Flush();


            base.OnException(context);
        }

        #endregion
    }
}
