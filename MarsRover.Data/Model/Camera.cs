using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarsRover.Data.Model
{
    public class Camera
    {
        [JsonProperty("id", NullValueHandling=NullValueHandling.Ignore)]
        public int? Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("full_name")]
        public string FullName { get; set; }

        [JsonProperty("rover_id", NullValueHandling=NullValueHandling.Ignore)]
        public int? RoverId { get; set; }
    }
}
