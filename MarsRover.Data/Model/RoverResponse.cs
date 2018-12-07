using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarsRover.Data.Model
{
    public class RoverResponse
    {
        [JsonProperty("rovers")]
        public List<Rover> Rovers { get; set; }
    }
}
