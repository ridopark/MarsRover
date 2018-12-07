using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarsRover.Data.Model
{
    public class PhotoResponse
    {
        [JsonProperty("photos")]
        public List<Photo> Photos { get; set; }
    }
}
