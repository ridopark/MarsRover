using MarsRover.Core;
using MarsRover.Data.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MarsRover.Service
{
    public interface IMarsRoverService : ISingletonDependency
    {
        Task<RoverResponse> GetRoversAsync();

        Task<PhotoResponse> GetRoverPhotosAsync(Rover rover, DateTime earthDate);

        Task<List<Photo>> GetRoverPhotosAsync(Rover rover, DateTime earthDate, string camera, int page = 1);

        Task DownloadRoverPhotosAsync(DateTime dates, IList<Rover> rovers);

        Task DownloadRoverPhotosAsync(DateTime date, Rover rover);

        Task DownloadRoverPhotosAsync(DateTime date, Rover rover, Camera camera);

        Task DownloadRoverPhotoAsync(Photo photo);

        string GetPhotoDownloadPath(Photo photo);
    }
}
