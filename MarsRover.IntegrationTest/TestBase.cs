using MarsRover.Host.Service;
using MarsRover.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MarsRover.IntegrationTest
{
    public class TestBase : IDisposable
    {
        protected ServiceProvider _serviceProvider;

        public void Init()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.Test.json").Build();
            _serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddHostedService<MarsRoverDownloadHostedService>()
                .AddSingleton<IConfiguration>(config)
                .AddSingleton<IMarsRoverService, MarsRover.Service.MarsRoverService>()
                .BuildServiceProvider();

            var configuration = _serviceProvider.GetService<IConfiguration>();
            var downloadBasePath = configuration.GetValue<string>("MarsRover:DownloadBasePath");
            System.IO.Directory.Delete(downloadBasePath, true);
        }

        public void Dispose()
        {
        }
    }
}
