﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LikeKero.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args)
                .Build()
                .Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>().UseIISIntegration();
                //.UseKestrel(options =>
                //{
                //    options.Limits.MaxRequestBodySize = Int32.MaxValue;
                //    options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(120);
                //    options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(120);
                //});
            
       // .ConfigureKestrel(o => { o.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(30); });
        
    }
}
