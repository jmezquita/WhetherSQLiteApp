using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WhetherSQLiteApp.Models
{
        public partial class WeatherDataSource
        {
            [JsonProperty("location")]
            public Location Location { get; set; }

            [JsonProperty("current")]
            public Current Current { get; set; }
        }
}
