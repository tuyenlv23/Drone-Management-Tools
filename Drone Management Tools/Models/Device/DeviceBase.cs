using System.ComponentModel;
using Drone_Management_Tools.Models.Device;
using Newtonsoft.Json;

namespace Drone_Management_Tools.Models
{
    public class DeviceBase
    {
        [JsonProperty(PropertyName = "name")]
        [ReadOnly(true)]
        [DisplayName("Name")]
        [Description("Device Name")]
        [Category("Common Information")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "id")]
        [ReadOnly(true)]
        [DisplayName("Id")]
        [Description("Device Id")]
        [Category("Common Information")]
        public byte Id { get; set; }

        [JsonProperty(PropertyName = "voltage")]
        [ReadOnly(true)]
        [DisplayName("Voltage")]
        [Description("Device Voltage")]
        [Category("Common Information")]
        public byte Voltage { get; set; }

        [JsonProperty(PropertyName = "deviceCommTypes")]
        [Browsable(false)]
        public DeviceCommTypes DeviceCommType { get; set; }

        [JsonProperty(PropertyName = "pwm")]
        [ReadOnly(true)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [DisplayName("PWM Parameters")]
        [Description("PWM Parameters")]
        [Category("Common Information")]
        public DevicePwm Pwm { get; set; }

        [JsonProperty(PropertyName = "attribute")]
        [Browsable(false)]
        [ReadOnly(true)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [DisplayName("State/Position")]
        [Description("Device Attribute")]
        [Category("Common Information")]
        public DeviceAttribute Attribute { get; set; }

        public DeviceBase()
        {
            this.Name = "New Device";
            this.Id = 0;
            this.Voltage = 0;
            this.DeviceCommType = DeviceCommTypes.None;
            this.Pwm = new DevicePwm();
            this.Attribute = new DeviceAttribute();
        }
    }
}
