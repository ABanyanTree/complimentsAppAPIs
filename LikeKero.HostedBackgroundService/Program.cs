using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace LikeKero.HostedBackgroundService
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var isService = !(Debugger.IsAttached || args.Contains("--console"));

            var builder = new HostBuilder()
                .ConfigureHostConfiguration(configHost =>
                {

                    configHost.AddJsonFile("appsettings.json", optional: false);
                  
                })
                .ConfigureServices((hostContext,services) =>
                {
                    services.Configure<LMSBackGroundServiceConfiguration>(hostContext.Configuration.GetSection("LMSBackGroundServiceConfiguration"));
                    services.AddHostedService<LMSBackgroundService>();
                   
                });

            if (isService)
            {
                await builder.RunAsServiceAsync();
            }
            else
            {
                await builder.RunConsoleAsync();
            }
        }
    }
}
