using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarsRover.Data.Model
{
    public class Photo
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("sol")]
        public int Sol { get; set; }

        [JsonProperty("img_src")]
        public string ImgSrc { get; set; }


        [JsonProperty("earth_date")]
        private string _earthDateStrVal { get; set; }

        public DateTime EarthDate
        {
            get
            {
                var parsedOk = DateTime.TryParse(_earthDateStrVal, out var parsedDate);
                return parsedDate;
            }
            set
            {
                _earthDateStrVal = value.ToString("yyyy-MM-dd");
            }
        }

        [JsonProperty("rover")]
        public Rover Rover { get; set; }

        [JsonProperty("camera")]
        public Camera Camera { get; set; }
    }
}
