using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Fanex.Athena.AnalyticsAPI
{
    public class Program
    {
        protected Program()
        {
        }

        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var hostBuilder = WebHost.CreateDefaultBuilder(args);
            var envName = hostBuilder.GetSetting("environment");
            var configBuilder = new ConfigurationBuilder();

            configBuilder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{envName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();   
            var configuration = configBuilder.Build();

            hostBuilder.UseUrls(configuration["hostUrlUsed"])
                .UseKestrel(options =>
                {
                    options.AddServerHeader = false;
                })
                .UseStartup<Startup>();

            return hostBuilder;
        }
    }
}