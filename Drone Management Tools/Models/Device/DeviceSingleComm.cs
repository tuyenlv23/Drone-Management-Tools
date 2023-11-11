using Drone_Management_Tools.Organizer;
using System.ComponentModel;

namespace Drone_Management_Tools.Models
{
    public class DeviceSingleComm : DeviceBase
    {
        [ReadOnly(true)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [DisplayName("Communications")]
        [Description("Device Communication")]
        [Category("Device Communications")]
        public DeviceComm Communication { get; set; }
        
        public DeviceSingleComm() 
        {
            this.Communication = new DeviceComm();
        }
    }
}