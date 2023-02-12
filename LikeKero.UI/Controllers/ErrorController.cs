using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LikeKero.UI.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> Logger;

        public ErrorController(ILogger<ErrorController> _Logger)
        {
            Logger = _Logger;
        }

        [Route("Error/{StatusCode}")]
        public IActionResult HttpStatusCodehandler(int statuscode)
        {
            if (statuscode == 404)
            {
                ViewBag.StatusCode = statuscode;
            }

            return View();
        }

        [Route("Error")]
        [AllowAnonymous]
        public IActionResult Error()
        {
            
            var httpstatuscoderesult = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (httpstatuscoderesult != null)
            {
                Logger.LogError($"The error {httpstatuscoderesult.Path} threw an error {httpstatuscoderesult.Error}");
                var path = httpstatuscoderesult.Path;
                var query = httpstatuscoderesult.Error.StackTrace;
                var t = httpstatuscoderesult.Error.Message;
            }

            return View();
        }

    }
}