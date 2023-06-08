using NLog.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
namespace EventManagement

{
    class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webHost => { webHost.UseStartup<Startup>(); })
            .ConfigureLogging(builder =>
            {
                builder.AddNLog("Nlog.config");
                builder.SetMinimumLevel(LogLevel.Debug);
            });

    }
}
