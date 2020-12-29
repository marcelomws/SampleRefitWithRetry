using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;

namespace SamplePollyRefit
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var hostBuilder = CreateHostBuilder(args).Build();
                Log.Information("Iniciando Web Host");
                hostBuilder.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host encerrado inesperadamente");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).ConfigureAppConfiguration((hostingContext) =>
                {
                    Log.Logger = new LoggerConfiguration()
                        .Enrich.FromLogContext()
                        .ReadFrom.AppSettings()
                        .WriteTo.Console()
                        .WriteTo.File(@"c:\Log\HeroApiSample\log-.log", rollingInterval: RollingInterval.Day)
                        .CreateLogger();
                })
                .UseSerilog();
    }
}
