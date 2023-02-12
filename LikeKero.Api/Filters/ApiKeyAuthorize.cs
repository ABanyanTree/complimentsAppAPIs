using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace LikeKero.Api.Filters
{
    /// </summary>
    [AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyAuthorizeAttribute : Attribute, IAsyncActionFilter
    {
        public const string Apikey = "ApiKey";
        /// <summary>
        /// Validate API key
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if(!context.HttpContext.Request.Headers.TryGetValue(Apikey, out var  PotentialKey) )
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            var configuration = context.HttpContext.RequestServices.GetService<IConfiguration>();
            var  apiKey = configuration.GetValue<string>(key: Apikey);
            if (!apiKey.Equals(PotentialKey))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            await next();
        }
    }
}
