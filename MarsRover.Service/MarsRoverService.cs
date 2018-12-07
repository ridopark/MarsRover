using MarsRover.Data.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MarsRover.Service
{
    public class MarsRoverService : IMarsRoverService
    {
        private readonly string _roverBaseApiBaseUrl;
        private readonly string _roverBaseApiKey;
        private readonly string _downloadBasePath;
        private readonly ILogger<MarsRoverService> _logger;
        private readonly IConfiguration _configuration;

        public MarsRoverService(
            ILogger<MarsRoverService> logger,
            IConfiguration configuration
            )
        {
            _logger = logger;
            _configuration = configuration;
            _roverBaseApiBaseUrl = _configuration.GetValue<string>("MarsRover:APIBaseUrl");
            _roverBaseApiKey = _configuration.GetValue<string>("MarsRover:APIKey");
            _downloadBasePath = _configuration.GetValue<string>("MarsRover:DownloadBasePath");
        }

        public async Task<RoverResponse> GetRoversAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_roverBaseApiBaseUrl);
                var response = await client.GetAsync($"rovers?api_key={_roverBaseApiKey}");
                if (!response.IsSuccessStatusCode)
                    return null;

                var data = await response.Content.ReadAsStringAsync();
                try
                {
                    var result = JsonConvert.DeserializeObject<RoverResponse>(data);
                    return result;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error deserializing rovers mission manifest");
                    return null;
                }
            }
        }

        public async Task<PhotoResponse> GetRoverPhotosAsync(Rover rover, DateTime earthDate)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_roverBaseApiBaseUrl);
                var response = await client.GetAsync($"rovers/{rover.Name}/photos?earth_date={earthDate.ToString("yyyy-MM-dd")}&api_key={_roverBaseApiKey}");
                if (!response.IsSuccessStatusCode)
                    return null;

                var data = await response.Content.ReadAsStringAsync();
                try
                {
                    var result = JsonConvert.DeserializeObject<PhotoResponse>(data);
                    return result;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error deserializing rovers mission manifest");
                    return null;
                }
            }
        }

        public async Task<List<Photo>> GetRoverPhotosAsync(Rover rover, DateTime earthDate, string camera, int page = 1)
        {
            var result = new List<Photo>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_roverBaseApiBaseUrl);

                var response = await client.GetAsync($"rovers/{rover.Name}/photos?earth_date={earthDate.ToString("yyyy-MM-dd")}&camera={camera}&page={page}&api_key={_roverBaseApiKey}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    try
                    {
                        var photoResponse = JsonConvert.DeserializeObject<PhotoResponse>(data);
                        result.AddRange(photoResponse.Photos);
                        if (photoResponse.Photos.Count == 25)
                        {
                            result.AddRange(await GetRoverPhotosAsync(rover, earthDate, camera, page+1));
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error deserializing rovers mission manifest");
                    }
                }
            }

            return result;
        }

        public async Task DownloadRoverPhotosAsync(DateTime date, IList<Rover> rovers)
        {
            var tasks = rovers.Select(rover => { return DownloadRoverPhotosAsync(date, rover); });
            await Task.WhenAll(tasks);
        }

        public async Task DownloadRoverPhotosAsync(DateTime date, Rover rover)
        {
            var tasks = rover.Cameras.Select(camera => { return DownloadRoverPhotosAsync(date, rover, camera); });
            await Task.WhenAll(tasks);
        }

        public async Task DownloadRoverPhotosAsync(DateTime date, Rover rover, Camera camera)
        {
            var photos = await GetRoverPhotosAsync(rover, date, camera.Name);
            _logger.LogInformation($"On {date.ToString("yyyy-MM-dd")}, {rover.Name}'s camara \"{camera.FullName}({camera.Name})\" took {photos.Count} photos.");

            var tasks = photos.Select(photo => { return DownloadRoverPhotoAsync(photo); });
            await Task.WhenAll(tasks);
        }

        public async Task DownloadRoverPhotoAsync(Photo photo)
        {
            var photoFilename = System.IO.Path.GetFileName(photo.ImgSrc);
            var downloadBasePath = GetPhotoDownloadPath(photo);
            var httpClient = new HttpClient();
            var response = await Policy
                .HandleResult<HttpResponseMessage>(message => !message.IsSuccessStatusCode)
                .WaitAndRetryAsync(100, i => TimeSpan.FromSeconds(60), (result, timeSpan, retryCount, context) =>
                {
                    _logger.LogWarning($"Request failed with {result.Result.StatusCode}. Waiting {timeSpan} before next retry. Retry attempt {retryCount}");
                })
                .ExecuteAsync(() => httpClient.GetAsync(photo.ImgSrc));

            response.EnsureSuccessStatusCode();

            System.IO.Directory.CreateDirectory(downloadBasePath);
            using (var fileStream = new FileStream($@"{downloadBasePath}/{photoFilename}", FileMode.Create, FileAccess.Write, FileShare.None)) {
                await response.Content.CopyToAsync(fileStream);
            }
        }

        public string GetPhotoDownloadPath(Photo photo)
        {
            var downloadPath = $"{_downloadBasePath}{photo.EarthDate.ToString("yyyy-MM-dd")}/{photo.Rover.Name}/{photo.Camera.Name}";
            return downloadPath;
        }
    }
}
