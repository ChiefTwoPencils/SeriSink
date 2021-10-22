using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using SeriSink.Interfaces;
using SeriSink.Services;
using System.IO;
using System.Reflection;

namespace SeriSink
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location))
                .AddJsonFile("appsettings.json", true, false)
                .AddEnvironmentVariables()
                .Build();

            // Use the logger in Program#Main
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            Log.Logger.Information("Welcome to SeriSink :)");

            var services = new ServiceCollection();
            services.AddLogging(opt =>
            {
                opt.AddConfiguration(configuration.GetSection("Logging"));
                opt.AddConfiguration(configuration.GetSection("Serilog"));
                opt.AddConsole();
                opt.AddSerilog();
            });

            services.AddTransient<IService, Service>();

            var provider = services.BuildServiceProvider();

            var logger = provider.GetRequiredService<ILogger<Program>>();
            logger.LogInformation(
                "You can also log with required service which is a {@Type}.",
                logger.GetType());

            var service = provider.GetService<IService>();
            // Use the service and get the logger injected
            service.Do();

            // Make sure to flush it...
            Log.CloseAndFlush();
        }
    }
}
