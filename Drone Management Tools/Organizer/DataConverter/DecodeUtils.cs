using System;
using System.Linq;
using System.Text;
using Drone_Management_Tools.Models;
using Drone_Management_Tools.Utilities;
using Drone_Management_Tools.Models.Device;

namespace Drone_Management_Tools.Organizer
{
    public class DecodeUtils
    {
        private ConfigModel _configModel;
        public ConfigModel ConfigModel
        {
            get { return _configModel; }
            set { _configModel = value; }
        }

        private DroneModel _droneModel;
        public DroneModel DroneModel
        {
            get { return _droneModel; }
            set { _droneModel = value; }
        }

        private VoltageRegulator _voltageRegulator;
        public VoltageRegulator VoltageRegualtor
        {
            get { return _voltageRegulator; }
            set { _voltageRegulator = value; }
        }

        private UICmdTypes _uicmdType;
        public UICmdTypes UiCmdType
        {
            get { return _uicmdType; }
            set { _uicmdType = value; }
        }

        private byte _id = 0;

        public DecodeUtils()
        {

        }        

        //--- Decode Device Config
        public void DecodeDeviceConfig(byte[] datas)
        {
            try
            {
                this._configModel = new ConfigModel();
                while (true)
                {
                    if (datas.Length == 0)
                        break;

                    if (datas[0] == 0x40)
                    {
                        if (DecodeHeader(ref datas, 4))
                        {
                            var _devConfig = DecodeDevice(ref datas);
                            if (_devConfig != null)
                            {
                                this._configModel.Devices.Add(_devConfig);
                                if (!this._configModel.VirtualDevices.ContainsKey(_devConfig.Id))
                                    this._configModel.VirtualDevices.Add(_devConfig.Id, _devConfig);
                                else
                                {
                                    if (_devConfig.Communications.Count > 0)
                                        this._configModel.VirtualDevices[_devConfig.Id].Communications.Add(_devConfig.Communications[0]);
                                }
                            }
                            else
                            {
                                LogUtils.AddLog($"Device id: [{this._id}] is wrong format", LogTypes.Info);
                            }

                            while (GetDataFrame(ref datas, 1)[0] != 0x23)
                            {
                                if (GetDataFrame(ref datas, 1)[0] == 0x23)
                                    break;
                                else
                                    continue;
                            }
                        }
                    }
                    else
                        GetDataFrame(ref datas, 1);
                }
            }
            catch
            {
                this._configModel.Devices.Clear();
            }
        }

        //--- Decode Drone Config
        public void DecodeDroneConfig(byte[] datas)
        {
            try
            {                
                while (true)
                {
                    if (datas.Length == 0)
                        break;

                    if (datas[0] == 0x40)
                    {
                        GetDataFrame(ref datas, 4);
                        this._droneModel = new DroneModel();
                        this._droneModel.Decode(this, ref datas);

                        while (GetDataFrame(ref datas, 1)[0] != 0x23)
                        {
                            if (GetDataFrame(ref datas, 1)[0] == 0x23)
                                break;
                            else
                                continue;
                        }
                    }
                    else
                        GetDataFrame(ref datas, 1);
                }
            }
            catch
            {
                this._droneModel = null;
            }
        }

        //--- Decode Scan Data
        public bool DecodeScanData(byte[] datas, DeviceManager deviceManager)
        {
            try
            {
                deviceManager.DeviceScan = new DeviceScan();
                if (datas[0] == 0x40 && datas[3] == 0x23)
                {                    
                    var _id1 = ConvertHexToByte(datas[1]);
                    if (deviceManager.DeviceLibrary.VirtualDevices.ContainsKey(_id1))
                    {
                        deviceManager.DeviceScan.Devices.Add(deviceManager.DeviceLibrary.VirtualDevices[_id1]);
                    }
                    else
                    {
                        if (_id1 != 0)
                        {
                            DeviceConfig _newDevConfig = new DeviceConfig();
                            _newDevConfig.Id = _id1;
                            _newDevConfig.Name = "Device Onboard 1";
                            deviceManager.DeviceScan.Devices.Add(_newDevConfig);
                        }
                        else
                            deviceManager.DeviceScan.Devices.Add(null);
                    }

                    var _id2 = ConvertHexToByte(datas[2]);
                    if (deviceManager.DeviceLibrary.VirtualDevices.ContainsKey(_id2))
                    {
                        deviceManager.DeviceScan.Devices.Add(deviceManager.DeviceLibrary.VirtualDevices[_id2]);
                    }
                    else
                    {
                        if (_id2 != 0)
                        {
                            DeviceConfig _newDevConfig = new DeviceConfig();
                            _newDevConfig.Id = _id2;
                            _newDevConfig.Name = "Device Onboard 2";
                            deviceManager.DeviceScan.Devices.Add(_newDevConfig);
                        }
                        else
                            deviceManager.DeviceScan.Devices.Add(null);
                    }

                    deviceManager.DeviceScan.ValidateDevices();
                    return true;
                }
                else
                {
                    deviceManager.DeviceScan.Devices.Clear();
                    return false;
                }
            }
            catch
            {
                deviceManager.DeviceScan.Devices.Clear();
                return false;
            }
        }

        //--- Decode Drone Config
        public void DecodeVoltageRegulator(byte[] datas)
        {
            try
            {
                while (true)
                {
                    if (datas.Length == 0)
                        break;

                    if (datas[0] == 0x40)
                    {
                        GetDataFrame(ref datas, 4);
                        this._voltageRegulator = new VoltageRegulator();
                        this._voltageRegulator.Decode(this, ref datas);

                        while (GetDataFrame(ref datas, 1)[0] != 0x23)
                        {
                            if (GetDataFrame(ref datas, 1)[0] == 0x23)
                                break;
                            else
                                continue;
                        }
                    }
                    else
                        GetDataFrame(ref datas, 1);
                }
            }
            catch
            {
                this._voltageRegulator = null;
            }
        }

        //--- Decode Response Data
        public DataResponseStates ValidateResponseState(ref byte[] datas, int byteCount)
        {
            try
            {
                if (datas.Length > 0)
                {
                    byte[] _bytes = GetDataFrame(ref datas, byteCount);
                    var _hexStr = BitConverter.ToString(_bytes).Replace("-", string.Empty);
                    if (_hexStr == DataFrameCodes.RESPONSE_SUCCESS)
                        return DataResponseStates.Response_Success;
                }

                return DataResponseStates.Response_Failed;
            }
            catch
            {
                return DataResponseStates.None;
            }
        }

        //--- Decode Header
        private bool DecodeHeader(ref byte[] datas, int byteCount)
        {
            try
            {
                byte[] _bytes = GetDataFrame(ref datas, byteCount);
                var _startCode = ConvertHexToString(new byte[] { _bytes[0] });
                if (_startCode == DataFrameCodes.START_FRAME_CODE)
                {
                    var _uiCmdCode = (UICmdTypes)ConvertHexToByte(_bytes[1]);                    
                    var _cfgVersionCode = ConvertHexToByte(_bytes[2]);
                    this._configModel.ConfigInfo.Version = _cfgVersionCode;
                    var _totalDevicesCode = ConvertHexToByte(_bytes[3]);

                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }        

        //--- Decode Device Configuration
        private DeviceConfig DecodeDevice(ref byte[] datas)
        {
            try
            {
                DeviceConfig _devConfig = new DeviceConfig();

                byte[] _devCommonInfo = GetDataFrame(ref datas, 4);
                var _devId = ConvertHexToByte(_devCommonInfo[0]);
                _devConfig.Id = _devId;
                this._id = _devId;
                _devConfig.Name = $"Device {_devId}";

                var _devVolt = ConvertHexToByte(_devCommonInfo[1]);
                _devConfig.Voltage = _devVolt;

                var _devCommType = (DeviceCommTypes)ConvertHexToByte(_devCommonInfo[2]);
                _devConfig.DeviceCommType = _devCommType;

                var _commType = (CommTypes)ConvertHexToByte(_devCommonInfo[3]);

                switch (_commType)
                {
                    case CommTypes.TCP:   //--- 6 bytes
                        {
                            byte[] _devComm = GetDataFrame(ref datas, 6);
                            _devConfig.Communications.Add(DecodeTCPProtocol(_devComm));
                            break;
                        }
                    case CommTypes.UART:  //--- 7 bytes
                        {
                            byte[] _devComm = GetDataFrame(ref datas, 7);
                            _devConfig.Communications.Add(DecodeUARTProtocol(_devComm));
                            break;
                        }
                    case CommTypes.I2C:   //--- 7 bytes
                        {
                            byte[] _devComm = GetDataFrame(ref datas, 7);
                            _devConfig.Communications.Add(DecodeI2CProtocol(_devComm));
                            break;
                        }
                    case CommTypes.CAN:   //--- 5 bytes
                        {
                            byte[] _devComm = GetDataFrame(ref datas, 5);
                            _devConfig.Communications.Add(DecodeCANProtocol(_devComm));
                            break;
                        }
                }

                //--- 48 bytes
                _devConfig.Pwm = DecodeDevicePWM(GetDataFrame(ref datas, 48));

                return _devConfig;
            }
            catch
            {
                return null;
            }
        }

        //--- Decode Protocol
        private DeviceComm DecodeI2CProtocol(byte[] bytes)
        {
            try
            {
                DeviceComm _result = new DeviceComm();
                _result.Type = CommTypes.I2C;

                Protocol_I2C _i2c = new Protocol_I2C();
                if (_i2c.Decode(this, bytes))
                    _result.Parameters = _i2c;
                else
                    _result.Parameters = null;

                return _result;
            }
            catch
            {
                return null;
            }
        }

        private DeviceComm DecodeCANProtocol(byte[] bytes)
        {
            try
            {
                DeviceComm _result = new DeviceComm();
                _result.Type = CommTypes.CAN;

                Protocol_CAN _can = new Protocol_CAN();
                if (_can.Decode(this, bytes))
                    _result.Parameters = _can;
                else
                    _result.Parameters = null;

                return _result;
            }
            catch
            {
                return null;
            }
        }

        private DeviceComm DecodeTCPProtocol(byte[] bytes)
        {
            try
            {
                DeviceComm _result = new DeviceComm();
                _result.Type = CommTypes.TCP;

                Protocol_TCP _tcp = new Protocol_TCP();
                if (_tcp.Decode(this, bytes))
                    _result.Parameters = _tcp;
                else
                    _result.Parameters = null;

                return _result;
            }
            catch
            {
                return null;
            }
        }

        private DeviceComm DecodeUARTProtocol(byte[] bytes)
        {
            try
            {
                DeviceComm _result = new DeviceComm();
                _result.Type = CommTypes.UART;

                Protocol_UART _uart = new Protocol_UART();
                if (_uart.Decode(this, bytes))
                    _result.Parameters = _uart;
                else
                    _result.Parameters = null;

                return _result;
            }
            catch
            {
                return null;
            }
        }

        private Protocol_PWM DecodePWMProtocol(byte[] bytes)
        {
            try
            {
                Protocol_PWM _result = new Protocol_PWM();
                if (_result.Decode(this, bytes))
                    return _result;
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }

        private DevicePwm DecodeDevicePWM(byte[] bytes)
        {
            try
            {
                DevicePwm _result = new DevicePwm();
                if (_result.Decode(this, bytes))
                    return _result;
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }

        public byte[] GetDataFrame(ref byte[] datas, int byteCount)
        {
            try
            {
                var _newDatas = datas.ToList();
                var _result = _newDatas.GetRange(0, byteCount);
                datas = _newDatas.GetRange(byteCount, _newDatas.Count - byteCount).ToArray();

                return _result.ToArray();
            }
            catch
            {
                return null;
            }
        }

        //--- Convert from Hex
        public string ConvertHexToString(byte[] bytes)
        {
            try
            {
                return Encoding.ASCII.GetString(bytes); ;
            }
            catch
            {
                return "";
            }
        }

        public string ConvertHexToString(string hexCode)
        {
            try
            {
                hexCode = hexCode.ToLower().Replace("0x", "");
                byte[] bytes = Enumerable.Range(0, hexCode.Length)
                     .Where(x => x % 2 == 0)
                     .Select(x => Convert.ToByte(hexCode.Substring(x, 2), 16))
                     .ToArray();
                var strVal = Encoding.ASCII.GetString(bytes);

                return strVal;
            }
            catch
            {
                return "";
            }
        }

        public byte ConvertHexToByte(byte bytes)
        {
            try
            {
                return bytes;
            }
            catch
            {
                return 0;
            }
        }

        public byte ConvertHexToByte(string hexCode)
        {
            try
            {
                var byteVal = Convert.ToByte(hexCode, 16);

                return byteVal;
            }
            catch
            {
                return 0;
            }
        }

        public int ConvertHexToInt(byte[] bytes)
        {
            try
            {
                if (BitConverter.IsLittleEndian)
                    bytes = bytes.Reverse().ToArray();

                return BitConverter.ToInt32(bytes, 0);
            }
            catch
            {
                return 0;
            }
        }

        public int ConvertHexToInt(string hexCode)
        {
            try
            {
                var _uintVal = Convert.ToInt32(hexCode, 16);

                return _uintVal;
            }
            catch
            {
                return 0;
            }
        }

        public uint ConvertHexToUint(byte[] bytes)
        {
            try
            {
                if (BitConverter.IsLittleEndian)
                    bytes = bytes.Reverse().ToArray();

                return BitConverter.ToUInt32(bytes, 0);
            }
            catch
            {
                return 0;
            }
        }

        public uint ConvertHexToUint(string hexCode)
        {
            try
            {
                var _uintVal = Convert.ToUInt32(hexCode, 16);

                return _uintVal;
            }
            catch
            {
                return 0;
            }
        }

        public float ConvertHexToFloat(byte[] bytes)
        {
            try
            {
                if (BitConverter.IsLittleEndian)
                    bytes = bytes.Reverse().ToArray();

                return BitConverter.ToSingle(bytes, 0);
            }
            catch
            {
                return 0.0f;
            }
        }

        public float ConvertHexToFloat(string hexCode)
        {
            try
            {
                var _int32Val = Convert.ToInt32(hexCode, 16);
                byte[] _bytes = BitConverter.GetBytes(_int32Val);
                if (BitConverter.IsLittleEndian)
                    _bytes = _bytes.Reverse().ToArray();
                float floatVal = BitConverter.ToSingle(_bytes, 0);

                return floatVal;
            }
            catch
            {
                return 0.0f;
            }
        }

        public double ConvertHexToDouble(byte[] bytes)
        {
            try
            {
                if (BitConverter.IsLittleEndian)
                    bytes = bytes.Reverse().ToArray();

                return BitConverter.ToDouble(bytes, 0);
            }
            catch
            {
                return 0.0;
            }
        }

        public double ConvertHexToDouble(string hexCode)
        {
            try
            {
                var _int64Val = Convert.ToInt64(hexCode, 16);
                byte[] _bytes = BitConverter.GetBytes(_int64Val);
                if (BitConverter.IsLittleEndian)
                    _bytes = _bytes.Reverse().ToArray();
                var doubleVal = BitConverter.ToDouble(_bytes, 0);

                return doubleVal;
            }
            catch
            {
                return 0.0;
            }
        }
    }
}
