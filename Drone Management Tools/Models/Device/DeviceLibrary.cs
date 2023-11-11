using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using DevExpress.XtraEditors.Popup;

namespace Drone_Management_Tools.Models
{
    public class DeviceLibrary : DeviceModel
    {
        [JsonProperty(PropertyName = "library")]
        public LibraryInfo LibraryInfo { get; set; }

        [JsonIgnore]
        public LibraryInfo VirtualLibraryInfo { get; set; }

        public DeviceLibrary()
        {
            this.LibraryInfo = new LibraryInfo();
            this.VirtualLibraryInfo = new LibraryInfo();
            this.Devices = new List<DeviceConfig>();
            this.VirtualDevices = new Dictionary<byte, DeviceConfig>();
            this.DeviceIds = new List<byte>();
            this.SingleCommDevices = new Dictionary<byte, DeviceSingleComm>();
            this.DoubleCommDevices = new Dictionary<byte, DeviceDoubleComm>();
        }

        public override void Save()
        {
            this.Devices.Clear();
            this.Devices = this.VirtualDevices.Values.ToList();

            this.LibraryInfo = this.VirtualLibraryInfo.Clone();
        }
    }

    public class LibraryInfo
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "updateTime")]
        public DateTime UpdateTime { get; set; }

        public LibraryInfo() 
        {
            this.Name = "Drone_Device_Library";
            this.Description = "Devices of project";
            this.UpdateTime = new DateTime();
        }

        public LibraryInfo Clone()
        {
            return (LibraryInfo)this.MemberwiseClone();
        }
    }
}
