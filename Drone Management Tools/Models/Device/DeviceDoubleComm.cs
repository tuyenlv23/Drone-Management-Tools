using System.ComponentModel;

namespace Drone_Management_Tools.Models
{
    public class DeviceDoubleComm : DeviceBase
    {
        [ReadOnly(true)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [DisplayName("Communication 1")]
        [Description("Device Communication")]
        [Category("Device Communications")]
        public DeviceComm Communication1 { get; set; }

        [ReadOnly(true)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [DisplayName("Communication 2")]
        [Description("Device Communication")]
        [Category("Device Communications")]
        public DeviceComm Communication2 { get; set; }

        public DeviceDoubleComm()
        {
            this.Communication1 = new DeviceComm();
            this.Communication2 = new DeviceComm();
        }
    }
}
