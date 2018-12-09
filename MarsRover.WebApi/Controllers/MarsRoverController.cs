using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarsRover.Data.Model;
using MarsRover.Service;
using Microsoft.AspNetCore.Mvc;

namespace MarsRover.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarsRoverController : Controller
    {
        private readonly IMarsRoverService _marsRoverService;
        public MarsRoverController(
            IMarsRoverService marsRoverService
            )
        {
            _marsRoverService = marsRoverService;
        }


        /// <summary>
        /// get all rovers 
        /// </summary>
        /// <returns></returns>
        [Route("rovers")]
        [HttpGet]
        public async Task<IEnumerable<Rover>> GetRovers()
        {
            var result = await _marsRoverService.GetRoversAsync();
            return result.Rovers;
        }

        /// <summary>
        /// get rover by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("rovers/{id}")]
        public async Task<Rover> GetRover(int id)
        {
            var result = await _marsRoverService.GetRoversAsync();
            return result.Rovers.FirstOrDefault(a => a.Id == id);
        }


        /// <summary>
        /// get all pictures for rover on the specified date
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("rovers/{id}/earthdate/{datetime}/photos")]
        public async Task<IEnumerable<Photo>> GetRoverPhotos(int id, DateTime datetime)
        {
            var rover = (await _marsRoverService.GetRoversAsync())?.Rovers.FirstOrDefault(a => a.Id == id);
            var photos = (await _marsRoverService.GetRoverPhotosAsync(rover, datetime))?.Photos;
            return photos;
        }
    }
}
