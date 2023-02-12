using LikeKero.Infra.FileSystem;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LikeKero.Api.Middleware
{
    public class FileHandlerMiddleWare
    {

        private readonly RequestDelegate _next;
        private IOptions<FileSystemPath> _options;

        public FileHandlerMiddleWare(RequestDelegate next, IOptions<FileSystemPath> options)
        {

            _next = next;
            _options = options;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            StringValues referer;
            httpContext.Request.Headers.TryGetValue("Referer", out referer);

            if (httpContext.Request.Path.ToString().ToLower().Contains("courseuploads")
                )
            {
                if (!referer.Any())
                {
                    httpContext.Response.StatusCode = 401;
                    await httpContext.Response.WriteAsync("Unauthorized File Access");
                }
                else if(referer.First().ToLower().Trim().Contains(_options.Value.AllowedReferres1.ToLower().Trim()))
                {
                    
                    await _next.Invoke(httpContext);

                }
                else if (referer.First().ToLower().Trim().Contains(_options.Value.AllowedReferres2.ToLower().Trim()))
                {

                    await _next.Invoke(httpContext);

                }
                else
                {
                    httpContext.Response.StatusCode = 401;
                    await httpContext.Response.WriteAsync("Unauthorized File Access");
                }
                
                
            }
            else
            {
                await _next.Invoke(httpContext);
            }

           
        }
    }
}
