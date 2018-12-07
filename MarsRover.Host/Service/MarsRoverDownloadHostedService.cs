using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MarsRover.Service;

namespace MarsRover.Host.Service
{
    public class MarsRoverDownloadHostedService : IHostedService
    {
        private readonly ILogger<MarsRoverDownloadHostedService> _logger;
        private readonly IApplicationLifetime _appLifetime;
        private readonly IConfiguration _configuration;
        private readonly IMarsRoverService _marsRoverService;

        public MarsRoverDownloadHostedService(
            ILogger<MarsRoverDownloadHostedService> logger, 
            IApplicationLifetime appLifetime,
            IConfiguration configuration, 
            IMarsRoverService marsRoverService
            )
        {
            _logger = logger;
            _appLifetime = appLifetime;
            _configuration = configuration;
            _marsRoverService = marsRoverService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _appLifetime.ApplicationStarted.Register(async (a) => { await OnStarted(); }, true);
            _appLifetime.ApplicationStopping.Register(OnStopping);
            _appLifetime.ApplicationStopped.Register(OnStopped);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private async Task OnStarted()
        {
            _logger.LogInformation("OnStarted has been called.");

            // Perform post-startup activities here

            var filename = _configuration.GetValue<string>("MarsRover:DateFilename");
            var dates = await GetDatesFromFileAsync(filename);
            var roversResponse = await _marsRoverService.GetRoversAsync();
            var rovers = roversResponse.Rovers.OrderBy(a => a.Name).ToList();
            _logger.LogInformation($"Received {roversResponse.Rovers.Count} rovers, {string.Join(", ", rovers.Select(a => a.Name))}");

            var tasks = dates.Select(date => { return _marsRoverService.DownloadRoverPhotosAsync(date, rovers); });
            await Task.WhenAll(tasks);            

            _appLifetime.StopApplication();
        }

        private void OnStopping()
        {
            _logger.LogInformation("OnStopping has been called.");

            // Perform on-stopping activities here
        }

        private void OnStopped()
        {
            _logger.LogInformation("OnStopped has been called.");

            // Perform post-stopped activities here
        }


        private async Task<List<DateTime>> GetDatesFromFileAsync(string filename)
        {
            _logger.LogInformation($"Get dates from text file: {filename}");
            var filepath = $"{Directory.GetCurrentDirectory()}\\{filename}";
            var result = new List<DateTime>();
            var lines = await File.ReadAllLinesAsync(filepath);
            foreach(var line in lines)
            {
                var parsedOk = DateTime.TryParse(line, out var parsedDate);
                if (parsedOk)
                {
                    _logger.LogWarning($"{line} - Parsed OK -> {parsedDate.ToString("yyyy-MM-dd")}");
                    result.Add(parsedDate);
                }
                else
                {
                    _logger.LogWarning($"{line} - Count not be parsed");
                }
            }

            return result;            
        }
    }
}
