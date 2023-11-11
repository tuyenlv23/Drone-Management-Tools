using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using Drone_Management_Tools.Utilities;

namespace Drone_Management_Tools.Models
{
    public abstract class DeviceModel
    {
        [JsonProperty(PropertyName = "devices")]
        public List<DeviceConfig> Devices { get; set; }

        [JsonIgnore]
        public Dictionary<byte, DeviceConfig> VirtualDevices { get; set; }

        [JsonIgnore]
        public List<byte> DeviceIds { get; set; }

        [JsonIgnore]
        public Dictionary<byte, DeviceSingleComm> SingleCommDevices { get; set; }

        [JsonIgnore]
        public Dictionary<byte, DeviceDoubleComm> DoubleCommDevices { get; set; }

        public virtual void ValidateDevices()
        {
            this.VirtualDevices.Clear();
            this.DeviceIds.Clear();
            this.SingleCommDevices.Clear();
            this.DoubleCommDevices.Clear();

            if (this.Devices.Count > 0)
            {
                foreach (var device in this.Devices)
                {
                    if (device != null)
                    {
                        UpdateVirtualDevices(device);
                        ConvertDevice(device);
                    }
                }

                this.DeviceIds = this.VirtualDevices.Keys.ToList();
            }            
        }

        public virtual void UpdateDevice(DeviceConfig newDevConfig)
        {
            if (newDevConfig != null)
            {
                UpdateVirtualDevices(newDevConfig);
                ConvertDevice(newDevConfig);

                this.DeviceIds.Clear();
                this.DeviceIds = this.VirtualDevices.Keys.ToList();
            }
        }

        private void UpdateVirtualDevices(DeviceConfig newDevConfig)
        {
            if (!this.VirtualDevices.ContainsKey(newDevConfig.Id))
                this.VirtualDevices.Add(newDevConfig.Id, newDevConfig);
            else
                this.VirtualDevices[newDevConfig.Id] = newDevConfig;
        }

        private void ConvertDevice(DeviceConfig deviceConfig)
        {
            try
            {
                if (deviceConfig.Communications.Count > 1) //--- Double Communication
                {
                    DeviceDoubleComm _newDevDoubleComm = deviceConfig.GetDoubleCommDevice();

                    if (!this.DoubleCommDevices.ContainsKey(_newDevDoubleComm.Id))
                        this.DoubleCommDevices.Add(_newDevDoubleComm.Id, _newDevDoubleComm);
                    else
                        this.DoubleCommDevices[_newDevDoubleComm.Id] = _newDevDoubleComm;

                    if (this.SingleCommDevices.ContainsKey(_newDevDoubleComm.Id))
                        this.SingleCommDevices.Remove(_newDevDoubleComm.Id);
                }
                else //--- Single Communication
                {
                    DeviceSingleComm _newDevSingleComm = deviceConfig.GetSingleCommDevice();

                    if (!this.SingleCommDevices.ContainsKey(_newDevSingleComm.Id))
                        this.SingleCommDevices.Add(_newDevSingleComm.Id, _newDevSingleComm);
                    else
                        this.SingleCommDevices[_newDevSingleComm.Id] = _newDevSingleComm;

                    if (this.DoubleCommDevices.ContainsKey(_newDevSingleComm.Id))
                        this.DoubleCommDevices.Remove(_newDevSingleComm.Id);
                }
            }
            catch (Exception ex)
            {
                LogUtils.AddLog(ex.Message, LogTypes.Error);
            }
        }

        public virtual void RemoveDevice(byte id)
        {
            if (this.VirtualDevices.ContainsKey(id))
            {
                this.VirtualDevices.Remove(id);

                this.DeviceIds.Clear();
                this.SingleCommDevices.Clear();
                this.DoubleCommDevices.Clear();

                if (this.VirtualDevices.Count > 0)
                {
                    this.DeviceIds = this.VirtualDevices.Keys.ToList();
                    foreach (var device in this.VirtualDevices.Values.ToList())
                    {
                        ConvertDevice(device);
                    }
                }
            }
        }

        public virtual void RemoveDevice(DeviceConfig deviceConfig)
        {
            if (this.VirtualDevices.ContainsKey(deviceConfig.Id))
            {
                this.VirtualDevices.Remove(deviceConfig.Id);

                this.DeviceIds.Clear();
                this.SingleCommDevices.Clear();
                this.DoubleCommDevices.Clear();

                if (this.VirtualDevices.Count > 0)
                {                    
                    this.DeviceIds = this.VirtualDevices.Keys.ToList();                    
                    foreach (var device in this.VirtualDevices.Values.ToList())
                    {
                        ConvertDevice(device);
                    }
                }
            }
        }

        public abstract void Save();
    }
}
