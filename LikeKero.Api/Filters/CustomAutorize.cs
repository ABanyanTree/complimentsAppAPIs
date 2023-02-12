using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LikeKero.Api.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomAuthorizeAttribute : Attribute, IAsyncActionFilter
    {
        
        public string FeatureId { get; set; }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            if(context == null || context.HttpContext == null || context.HttpContext.User == null 
                || context.HttpContext.User.Claims == null ||
                !context.HttpContext.User.HasClaim(x => x.Type == "Features") 
                || string.IsNullOrEmpty(context.HttpContext.User.Claims.Single(x => x.Type == "Features").Value)
                )
            {
                context.Result = new UnauthorizedResult();
                return;

            }
            string userFeatures = context.HttpContext.User.Claims.Single(x => x.Type == "Features").Value;
            string[] arrayUserFeatures = userFeatures.Split(',');

            if(string.IsNullOrEmpty(this.FeatureId))
            {
                await next();
            }

            if (arrayUserFeatures.Contains(this.FeatureId))
                await next();
            else
            {
                context.Result = new UnauthorizedResult();
                return;

            }
             

            

            //After


        }
    }
}
