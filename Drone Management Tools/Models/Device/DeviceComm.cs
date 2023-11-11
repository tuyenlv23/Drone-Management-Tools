using System.ComponentModel;
using Newtonsoft.Json;

namespace Drone_Management_Tools.Models
{
    public class DeviceComm
    {
        [JsonProperty(PropertyName = "type")]
        [ReadOnly(true)]        
        [DisplayName("Comm. Type")]
        [Description("Communication Type")]
        public CommTypes Type { get; set; }

        [JsonProperty(PropertyName = "parameters")]
        [ReadOnly(true)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [DisplayName("Parameters")]
        [Description("Communication Parameters")]
        public object Parameters { get; set; }

        public override string ToString()
        {
            return Type.ToString();
        }

        public DeviceComm()
        {
            this.Type = CommTypes.None;
            this.Parameters = null;
        }
    }
}
