using System.ComponentModel;
using Newtonsoft.Json;

namespace Drone_Management_Tools.Models
{
    public class DeviceAttribute
    {
        [JsonProperty(PropertyName = "state")]
        [ReadOnly(true)]
        [Description("Device State")]
        [DisplayName("Device State")]
        public DeviceStates DeviceState { get; set; }

        [JsonProperty(PropertyName = "position")]
        [ReadOnly(true)]
        [Description("Device Position")]
        [DisplayName("Device Position")]
        public DevicePositions DevicePosition { get; set; }

        public override string ToString()
        {
            return DeviceState.ToString() + "/" + DevicePosition.ToString();
        }
    }
}
