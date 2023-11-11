using System;
using System.Linq;
using System.Collections.Generic;

namespace Drone_Management_Tools.Models
{
    public class DeviceScan : DeviceModel
    {
        public DeviceScan()
        {
            this.Devices = new List<DeviceConfig>();
            this.VirtualDevices = new Dictionary<byte, DeviceConfig>();
            this.DeviceIds = new List<byte>();
            this.SingleCommDevices = new Dictionary<byte, DeviceSingleComm>();
            this.DoubleCommDevices = new Dictionary<byte, DeviceDoubleComm>();
        }

        public override void Save()
        {
            try
            {
                if (this.VirtualDevices.Count == 0)
                {
                    this.Devices.Clear();
                }

                if (this.VirtualDevices.Count == 1)
                {
                    if (this.Devices[0] != null && this.Devices[0].Id == this.VirtualDevices.First().Value.Id)
                    {
                        this.Devices[0] = this.VirtualDevices.First().Value;
                        this.Devices.RemoveAt(1);
                        return;
                    }

                    if (this.Devices[1] != null && this.Devices[1].Id == this.VirtualDevices.First().Value.Id)
                    {
                        this.Devices[1] = this.VirtualDevices.First().Value;
                        this.Devices.RemoveAt(0);
                        return;
                    }

                }

                if (this.VirtualDevices.Count == 2)
                {
                    if (this.Devices[0] != null)
                    {
                        this.Devices[0] = this.VirtualDevices.First().Value;
                    }

                    if (this.Devices[1] != null)
                    {
                        this.Devices[1] = this.VirtualDevices.Last().Value;
                    }
                }
            }   
            catch
            {

            }
        }
    }
}
