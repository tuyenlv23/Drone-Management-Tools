using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Drone_Management_Tools.Models
{
    public class ConfigModel : DeviceModel
    {
        [JsonProperty(PropertyName = "config")]
        public ConfigInfo ConfigInfo { get; set; }

        [JsonIgnore]
        public ConfigInfo VirtualConfigInfo { get; set; }

        public ConfigModel()
        {
            this.ConfigInfo = new ConfigInfo();
            this.VirtualConfigInfo = new ConfigInfo();
            this.Devices = new List<DeviceConfig>();
            this.VirtualDevices = new Dictionary<byte, DeviceConfig>();
            this.DeviceIds = new List<byte>();
            this.SingleCommDevices = new Dictionary<byte, DeviceSingleComm>();
            this.DoubleCommDevices = new Dictionary<byte, DeviceDoubleComm>();
        }

        public void InitDevices()
        {
            this.Devices = new List<DeviceConfig>();
            DeviceConfig _dev1Config = new DeviceConfig();
            _dev1Config.Id = 1;
            _dev1Config.Name = "Drone Side 1";
            this.Devices.Add(_dev1Config);

            DeviceConfig _dev2Config = new DeviceConfig();
            _dev2Config.Id = 2;
            _dev2Config.Name = "Drone Side 2";
            this.Devices.Add(_dev2Config);

            ValidateDevices();
        }

        public override void Save()
        {
            this.Devices.Clear();
            this.Devices = this.VirtualDevices.Values.ToList();

            this.ConfigInfo = this.VirtualConfigInfo.Clone();
        }
    }
}
