using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Drone_Management_Tools.Models.Drone
{
    public class DroneConfig
    {
        [JsonProperty(PropertyName = "drone_protocols")]
        public DroneModel DroneModel { get; set; }

        public DroneConfig()
        {
            this.DroneModel = new DroneModel();
        }
    }
}
