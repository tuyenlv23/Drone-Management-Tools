using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Drone_Management_Tools.Models
{
    public class VoltageConfig
    {
        [JsonProperty(PropertyName = "voltage_regulator")]
        public VoltageRegulator VoltageRegulator { get; set; }

        public VoltageConfig() 
        {
            this.VoltageRegulator = new VoltageRegulator();
        }
    }
}
