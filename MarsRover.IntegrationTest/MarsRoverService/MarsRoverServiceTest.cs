using MarsRover.Data.Model;
using MarsRover.Host.Service;
using MarsRover.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MarsRover.IntegrationTest.MarsRoverService
{
    public class MarsRoverServiceTest : TestBase
    {
        [Fact]
        public async Task GetRoversAsync_ReturnsThreeManifests()
        {
            Init();
            var marsRoverService = _serviceProvider.GetService<IMarsRoverService>();
            var result = await marsRoverService.GetRoversAsync();
            result.Rovers.Count.ShouldBe(3);
        }

        [Theory]
        [InlineData("Curiosity", "Opportunity", "Spirit")]
        public async Task GetRoversAsync_ShouldContainAllRoverNames(params string[] roverNames)
        {
            Init();
            var marsRoverService = _serviceProvider.GetService<IMarsRoverService>();
            var result = await marsRoverService.GetRoversAsync();
            result.Rovers.TrueForAll(a => roverNames.ToList().Contains(a.Name))
                .ShouldBe(true);
        }

        [Fact]
        public async Task GetRoverPhotosAsync_OnDate20150604_ForCuriosity_ReturnsFourPhotos()
        {
            Init();
            var datetime = DateTime.Parse("2015-6-3");
            var rover = new Rover()
            {
                Name = "curiosity"
            };
            var marsRoverService = _serviceProvider.GetService<IMarsRoverService>();
            var result = await marsRoverService.GetRoverPhotosAsync(rover, datetime);
            result.Photos.Count.ShouldBe(4);
        }

        [Fact]
        public async Task GetRoverPhotosAsync_OnDate20150605_ForCuriosity_ReturnsNoPhotos()
        {
            Init();
            var datetime = DateTime.Parse("2015-6-5");
            var rover = new Rover()
            {
                Name = "curiosity"
            };
            var marsRoverService = _serviceProvider.GetService<IMarsRoverService>();
            var result = await marsRoverService.GetRoverPhotosAsync(rover, datetime);
            result.Photos.Count.ShouldBe(0);
        }


        [Fact]
        public async Task DownloadRoverPhotoAsync_DownloadsOK()
        {
            Init();
            var configuration = _serviceProvider.GetService<IConfiguration>();
            var marsRoverService = _serviceProvider.GetService<IMarsRoverService>();

            var photo = Newtonsoft.Json.JsonConvert.DeserializeObject<Photo>(@"
    {
      ""id"": 102685,
      ""sol"": 1004,
      ""camera"": {
        ""id"": 20,
        ""name"": ""FHAZ"",
        ""rover_id"": 5,
        ""full_name"": ""Front Hazard Avoidance Camera""
      },
      ""img_src"": ""http://mars.jpl.nasa.gov/msl-raw-images/proj/msl/redops/ods/surface/sol/01004/opgs/edr/fcam/FLB_486615455EDR_F0481570FHAZ00323M_.JPG"",
      ""earth_date"": ""2015-06-03"",
      ""rover"": {
        ""id"": 5,
        ""name"": ""Curiosity"",
        ""landing_date"": ""2012-08-06"",
        ""launch_date"": ""2011-11-26"",
        ""status"": ""active"",
        ""max_sol"": 2251,
        ""max_date"": ""2018-12-05"",
        ""total_photos"": 343680,
        ""cameras"": [
          {
            ""name"": ""FHAZ"",
            ""full_name"": ""Front Hazard Avoidance Camera""
          },
          {
            ""name"": ""NAVCAM"",
            ""full_name"": ""Navigation Camera""
          },
          {
            ""name"": ""MAST"",
            ""full_name"": ""Mast Camera""
          },
          {
            ""name"": ""CHEMCAM"",
            ""full_name"": ""Chemistry and Camera Complex""
          },
          {
            ""name"": ""MAHLI"",
            ""full_name"": ""Mars Hand Lens Imager""
          },
          {
            ""name"": ""MARDI"",
            ""full_name"": ""Mars Descent Imager""
          },
          {
            ""name"": ""RHAZ"",
            ""full_name"": ""Rear Hazard Avoidance Camera""
          }
        ]
      }
    }
            ");
            var photoFilename = System.IO.Path.GetFileName(photo.ImgSrc);
            var downloadBasePath = marsRoverService.GetPhotoDownloadPath(photo);
            var photoFilepath = $@"{downloadBasePath}/{photoFilename}";
            await marsRoverService.DownloadRoverPhotoAsync(photo);
            
            System.IO.File.Exists(photoFilepath).ShouldBe(true);
        }
    }
}
