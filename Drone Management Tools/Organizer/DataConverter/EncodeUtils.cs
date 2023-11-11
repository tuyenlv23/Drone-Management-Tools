using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Drone_Management_Tools.Models;
using Drone_Management_Tools.Models.Device;

namespace Drone_Management_Tools.Organizer
{
    public class EncodeUtils
    {
        private ConfigManager _configManager;
        public ConfigManager ConfigManager
        {
            get { return _configManager; }
            set { _configManager = value; }
        }

        private UICmdTypes _uicmdType;
        public UICmdTypes UiCmdType
        {
            get { return _uicmdType; }
            set { _uicmdType = value; }
        }

        public EncodeUtils()
        {

        }

        //--- Encode Device Id
        public byte[] Encode(byte deviceId)
        {         
            return EncodeDataFrame(deviceId);
        }

        private byte[] EncodeDataFrame(byte deviceId)
        {
            try
            {
                var _startCode = DataFrameCodes.StartCode[0];

                var _cmdTypeCode = ConvertUICmdTypesToHex(UICmdTypes.Register_New_Device);

                var _idCode = ConvertByteToHex(deviceId);

                byte[] _result = new byte[] { _startCode, _cmdTypeCode, _idCode, DataFrameCodes.EndCode[0] };

                return _result;
            }
            catch
            {
                return null;
            }
        }

        //--- Encode Device Config
        public Dictionary<string, byte[]> Encode(ConfigManager cfgManager)
        {
            return EncodeDataFrame(cfgManager);
        }

        private Dictionary<string, byte[]> EncodeDataFrame(ConfigManager cfgManager)
        {
            try
            {
                Dictionary<string, byte[]> _result = new Dictionary<string, byte[]>();

                var _dataHeaderFrame = EncodeDataHeader(cfgManager);
                if (cfgManager != null && cfgManager.Model.Devices.Count > 0)
                {
                    foreach (var device in cfgManager.Model.Devices)
                    {
                        var _devDataFrame = EncodeDataFrame(_dataHeaderFrame, device);
                        if (_devDataFrame.Count > 0)
                        {
                            foreach (var data in _devDataFrame)
                            {
                                if (!_result.ContainsKey(data.Key))
                                    _result.Add(data.Key, data.Value);
                            }
                        }
                    }
                }

                return _result;
            }
            catch
            {
                return null;
            }
        }

        private Dictionary<string, byte[]> EncodeDataFrame(byte[] dataHeaderFrame, DeviceConfig deviceConfig)
        {
            try
            {
                Dictionary<string, byte[]> _result = new Dictionary<string, byte[]>();

                var _devDataFrames = deviceConfig.Encode(this);
                if (_devDataFrames.Count > 0)
                {
                    foreach (var devDataFrame in _devDataFrames)
                    {
                        var _dataLength = (byte)(dataHeaderFrame.Length + devDataFrame.Value.Length);
                        var _headerFrame = EncodeHeader();
                        _headerFrame = _headerFrame.ToList().Concat(dataHeaderFrame).ToArray();
                        var _devConfigFrame = _headerFrame.ToList().Concat(devDataFrame.Value).Concat(DataFrameCodes.EndCode.ToList()).ToArray();
                        _result.Add(devDataFrame.Key, _devConfigFrame);
                    }
                }

                return _result;
            }
            catch
            {
                return null;
            }
        }

        //--- Encode Drone Config
        public byte[] Encode(DroneModel droneModel)
        {
            return EncodeDataFrame(droneModel);
        }

        private byte[] EncodeDataFrame(DroneModel droneModel)
        {
            try
            {
                List<byte> _result = new List<byte>();

                if (droneModel != null)
                {
                    var _droneModelFrame = droneModel.Encode(this);
                    var _dataHeaderFrame = EncodeDefaultDataHeader(1, 14);
                    var _dataLength = (byte)(_droneModelFrame.Length + _dataHeaderFrame.Length);
                    var _headerFrame = EncodeHeader();
                    _headerFrame = _headerFrame.ToList().Concat(_dataHeaderFrame).ToArray();
                    _result.AddRange(_headerFrame);
                    _result.AddRange(_droneModelFrame);
                }

                _result.Add(DataFrameCodes.EndCode[0]);

                return _result.ToArray();
            }
            catch
            {
                return null;
            }
        }

        //--- Encode Voltage Regulator
        public byte[] Encode(VoltageRegulator voltageRegulator)
        {
            return EncodeDataFrame(voltageRegulator);
        }                
       
        private byte[] EncodeDataFrame(VoltageRegulator voltageRegulator)
        {
            try
            {
                List<byte> _result = new List<byte>();

                if (voltageRegulator != null)
                {
                    var _voltRegulatorFrame = voltageRegulator.Encode(this);
                    var _dataHeaderFrame = EncodeDefaultDataHeader(1, 1);
                    var _dataLength = (byte)(_voltRegulatorFrame.Length + _dataHeaderFrame.Length);
                    var _headerFrame = EncodeHeader();
                    _headerFrame = _headerFrame.ToList().Concat(_dataHeaderFrame).ToArray();
                    _result.AddRange(_headerFrame);
                    _result.AddRange(_voltRegulatorFrame);
                }

                _result.Add(DataFrameCodes.EndCode[0]);

                return _result.ToArray();
            }
            catch
            {
                return null;
            }
        }

        //--- Encode Memory Format
        public byte[] EncodeMemoryFormat()
        {
            try
            {
                var _startCode = DataFrameCodes.StartCode[0];
                var _cmdTypeCode = DataFrameCodes.MemoryFormat;
                var _endCode = DataFrameCodes.StartCode[0];
                byte[] _result = new byte[] { _startCode, _cmdTypeCode, _endCode };

                return _result;
            }
            catch
            {
                return null;
            }
        }

        //--- Encode Header
        private byte[] EncodeHeader() //--- 1 bytes
        {
            try
            {
                var _startCode = DataFrameCodes.StartCode[0];
                byte[] _result = new byte[] { _startCode };

                return _result;
            }
            catch
            {
                return null;
            }
        }

        private byte[] EncodeHeader(byte dataLength) //--- 2 bytes
        {
            try
            {
                var _startCode = DataFrameCodes.StartCode[0];
                var _dataLenCode = ConvertByteToHex(dataLength);

                byte[] _result = new byte[] { _startCode, _dataLenCode };

                return _result;
            }
            catch
            {
                return null;
            }
        }

        private byte[] EncodeDataHeader(ConfigManager cfgManager) //--- 3 bytes
        {
            try
            {
                var _cmdTypeCode = ConvertUICmdTypesToHex(this._uicmdType);

                var _configVersionCode = ConvertByteToHex(cfgManager.Model.ConfigInfo.Version);

                var _totalDevices = ConvertByteToHex((byte)cfgManager.Model.Devices.Count);

                byte[] _result = new byte[] { _cmdTypeCode, _configVersionCode, _totalDevices };

                return _result;
            }
            catch
            {
                return null;
            }
        }

        private byte[] EncodeDefaultDataHeader(byte version, byte totalMessages)
        {
            try
            {
                var _cmdTypeCode = ConvertUICmdTypesToHex(this._uicmdType);

                var _configVersionCode = ConvertByteToHex(version);

                var _totalDevices = ConvertByteToHex(totalMessages);

                byte[] _result = new byte[] { _cmdTypeCode, _configVersionCode, _totalDevices };

                return _result;
            }
            catch
            {
                return null;
            }
        } //--- 3 bytes

        //--- Encode Protocol
        public byte[] EncodeI2CProtocol(DeviceComm devComm)
        {
            List<byte> _result = new List<byte>();

            var _typeCode = ConvertByteToHex((byte)devComm.Type);
            _result.Add(_typeCode);

            var _parameterCode = ((Protocol_I2C)devComm.Parameters).Encode(this);
            _result.AddRange(_parameterCode);

            return _result.ToArray();
        }

        //--- CAN Protocol
        public byte[] EncodeCANProtocol(DeviceComm devComm)
        {
            List<byte> _result = new List<byte>();

            var _typeCode = ConvertByteToHex((byte)devComm.Type);
            _result.Add(_typeCode);

            var _parameterCode = ((Protocol_CAN)devComm.Parameters).Encode(this);
            _result.AddRange(_parameterCode);

            return _result.ToArray();
        }

        //--- TCP Protocol
        public byte[] EncodeTCPProtocol(DeviceComm devComm)
        {
            List<byte> _result = new List<byte>();

            var _typeCode = ConvertByteToHex((byte)devComm.Type);
            _result.Add(_typeCode);

            var _parameterCode = ((Protocol_TCP)devComm.Parameters).Encode(this);
            _result.AddRange(_parameterCode);

            return _result.ToArray();
        }

        //--- UART Protocol
        public byte[] EncodeUARTProtocol(DeviceComm devComm)
        {
            List<byte> _result = new List<byte>();

            var _typeCode = ConvertByteToHex((byte)devComm.Type);
            _result.Add(_typeCode);

            var _parameterCode = ((Protocol_UART)devComm.Parameters).Encode(this);
            _result.AddRange(_parameterCode);

            return _result.ToArray();
        }

        //--- PWM Protocol
        public byte[] EncodePWMProtocol(Protocol_PWM pwmProtocol)
        {
            return pwmProtocol.Encode(this);
        }

        public byte[] EncodeDevicePWM(DevicePwm devicePwm)
        {
            return devicePwm.Encode(this);
        }

        //--- Convert to Hex
        public byte ConvertUICmdTypesToHex(UICmdTypes cmdType)
        {
            return (byte)cmdType;
        }

        public byte[] ConvertStringToHex(string value)
        {
            try
            {
                return Encoding.ASCII.GetBytes(value);
            }
            catch
            {
                return null;
            }
        }

        public byte ConvertByteToHex(byte value)
        {
            return value;
        }

        public byte[] ConvertIntToHex(int value)
        {
            try
            {
                byte[] _result = BitConverter.GetBytes(value);
                if (BitConverter.IsLittleEndian)
                    _result = _result.Reverse().ToArray();

                return _result;
            }
            catch
            {
                return null;
            }
        }

        public byte[] ConvertUintToHex(uint value)
        {
            try
            {
                byte[] _result = BitConverter.GetBytes(value);
                if (BitConverter.IsLittleEndian)
                    _result = _result.Reverse().ToArray();

                return _result;
            }
            catch
            {
                return null;
            }
        }

        public byte[] ConvertFloatToHex(float value)
        {
            try
            {
                byte[] _result = BitConverter.GetBytes(value);
                if (BitConverter.IsLittleEndian)
                    _result = _result.Reverse().ToArray();

                return _result;
            }
            catch
            {
                return null;
            }
        }

        public byte[] ConvertDoubleToHex(double value)
        {
            try
            {
                byte[] _result = BitConverter.GetBytes(value);
                if (BitConverter.IsLittleEndian)
                    _result = _result.Reverse().ToArray();

                return _result;
            }
            catch
            {
                return null;
            }
        }
    }
}
