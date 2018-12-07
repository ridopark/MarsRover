using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MarsRover.Core;
using MarsRover.Host.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MarsRover.CMD
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                    configHost.AddJsonFile("hostsettings.json", optional: true);
                    configHost.AddEnvironmentVariables(prefix: "PREFIX_");
                    configHost.AddCommandLine(args);
                })
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    configApp.AddJsonFile("appsettings.json", optional: true);
                    configApp.AddJsonFile(
                        $"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json",
                        optional: true);
                    configApp.AddEnvironmentVariables(prefix: "PREFIX_");
                    configApp.AddCommandLine(args);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    //services.AddHostedService<LifetimeEventsHostedService>();
                    //services.AddHostedService<TimedHostedService>();
                    services.AddSingleton<IHostedService, MarsRoverDownloadHostedService>();

                    services.Scan(x => 
                    {
                        var entryAssembly = Assembly.GetEntryAssembly();
                        var referencedAssemblies = entryAssembly.GetReferencedAssemblies().Select(Assembly.Load);
                        var assemblies = new List<Assembly> { entryAssembly }.Concat(referencedAssemblies);

                        x.FromAssemblies(assemblies)
                        .AddClasses(classes => classes.AssignableTo(typeof(ISingletonDependency)))
                        .AsImplementedInterfaces()
                        .WithSingletonLifetime();
                    });
                })
                .ConfigureLogging((hostContext, configLogging) =>
                {
                    configLogging.AddConsole();
                    configLogging.AddDebug();
                })
                .UseConsoleLifetime()
                .Build();

            await host.RunAsync();
        }
    }    
}
