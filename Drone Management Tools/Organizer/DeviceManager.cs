using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Drone_Management_Tools.Models;
using Drone_Management_Tools.Utilities;

namespace Drone_Management_Tools.Organizer
{
    public class DeviceManager
    {
        private DeviceLibrary _deviceLibrary;
        public DeviceLibrary DeviceLibrary
        {
            get { return _deviceLibrary; }
            set { _deviceLibrary = value; }
        }

        private DeviceScan _deviceScan;
        public DeviceScan DeviceScan
        {
            get { return _deviceScan; }
            set { _deviceScan = value; }
        }

        private DeviceConfig _selected;
        public DeviceConfig Selected
        {
            get { return _selected; }
            set { _selected = value; }
        }

        private DeviceConfig _newDevice;
        public DeviceConfig NewDevice
        {
            get { return _newDevice; }
            set { _newDevice = value; }
        }

        public DeviceManager() 
        {
            this._deviceLibrary = new DeviceLibrary();
            //this._deviceScan = new DeviceScan();
            this._selected = new DeviceConfig();
            this._newDevice = new DeviceConfig();
        }

        public DeviceLibrary GetDeviceLibrary(string libraryPath)
        {
            try
            {
                if (!File.Exists(libraryPath))
                    HelperUtils.SaveJsonFile(this._deviceLibrary, libraryPath);

                using (StreamReader sr = File.OpenText(libraryPath))
                {
                    var obj = sr.ReadToEnd();
                    this._deviceLibrary = JsonConvert.DeserializeObject<DeviceLibrary>(obj);
                }

                if (this._deviceLibrary.Devices.Count > 0)
                {
                    foreach (var device in this._deviceLibrary.Devices)
                    {
                        if (device.Communications.Count > 0)
                        {
                            foreach (var devComm in device.Communications)
                            {
                                var commStr = devComm.Parameters.ToString();
                                switch (devComm.Type)
                                {
                                    case CommTypes.TCP:
                                        {
                                            devComm.Parameters = JsonConvert.DeserializeObject<Protocol_TCP>(commStr);
                                            break;
                                        }
                                    case CommTypes.UART:
                                        {
                                            devComm.Parameters = JsonConvert.DeserializeObject<Protocol_UART>(commStr);
                                            break;
                                        }
                                    case CommTypes.I2C:
                                        {
                                            devComm.Parameters = JsonConvert.DeserializeObject<Protocol_I2C>(commStr);
                                            break;
                                        }
                                    case CommTypes.CAN:
                                        {
                                            devComm.Parameters = JsonConvert.DeserializeObject<Protocol_CAN>(commStr);
                                            break;
                                        }
                                }
                            }
                        }                        
                    }

                    this._deviceLibrary.Devices.OrderBy(x => x.Id).ToList();
                    this._deviceLibrary.ValidateDevices();
                }

                return this._deviceLibrary;
            }
            catch (Exception ex)
            {
                LogUtils.AddLog(ex.Message, LogTypes.Error);
                return null;
            }
        }
    }
}
