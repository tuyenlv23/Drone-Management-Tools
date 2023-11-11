using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Drone_Management_Tools.Utilities;
using Drone_Management_Tools.Organizer;

namespace Drone_Management_Tools.Models
{
    public class DeviceConfig : DeviceBase
    {
        [JsonProperty(PropertyName = "communications")]
        public List<DeviceComm> Communications { get; set; }

        public DeviceConfig()
        {
            this.Communications = new List<DeviceComm>();
        }

        public DeviceSingleComm GetSingleCommDevice()
        {
            try
            {
                DeviceSingleComm _newDevSingleComm = new DeviceSingleComm();
                if (this.Communications.Count <= 1) //--- Single Communication
                {
                    
                    _newDevSingleComm.Name = this.Name;
                    _newDevSingleComm.Id = this.Id;
                    _newDevSingleComm.Voltage = this.Voltage;
                    _newDevSingleComm.DeviceCommType = this.DeviceCommType;
                    if (this.Communications.Count > 0)
                        _newDevSingleComm.Communication = this.Communications[0];
                    _newDevSingleComm.Pwm = this.Pwm;
                    _newDevSingleComm.Attribute = this.Attribute;                    
                }

                return _newDevSingleComm;
            }
            catch (Exception ex)
            {
                LogUtils.AddLog(ex.Message, LogTypes.Error);
                return null;
            }
        }

        public DeviceDoubleComm GetDoubleCommDevice()
        {
            try
            {
                DeviceDoubleComm _newDevDoubleComm = new DeviceDoubleComm();
                if (this.Communications.Count > 1) //--- Double Communication
                {
                    
                    _newDevDoubleComm.Name = this.Name;
                    _newDevDoubleComm.Id = this.Id;
                    _newDevDoubleComm.Voltage = this.Voltage;
                    _newDevDoubleComm.DeviceCommType = this.DeviceCommType;
                    _newDevDoubleComm.Communication1 = this.Communications[0];
                    _newDevDoubleComm.Communication2 = this.Communications[1];
                    _newDevDoubleComm.Pwm = this.Pwm;
                    _newDevDoubleComm.Attribute = this.Attribute;                    
                }

                return _newDevDoubleComm;
            }
            catch (Exception ex)
            {
                LogUtils.AddLog(ex.Message, LogTypes.Error);
                return null;
            }
        }

        public Dictionary<string, byte[]> Encode(EncodeUtils encodeUtils)
        {
            Dictionary<string, byte[]> _result = new Dictionary<string, byte[]>();

            var _devCommonInfo = EncodeDeviceCommonInfo(encodeUtils).ToList();
            var _pwmDataFrame = encodeUtils.EncodeDevicePWM(this.Pwm).ToList();
            var _devComms = EncodeDeviceComm(encodeUtils).ToList();
            if (_devComms != null && _devComms.Count > 0)
            {
                foreach (var devComm in _devComms)
                {
                    var _devDataFrame = _devCommonInfo.Concat(devComm).Concat(_pwmDataFrame).ToArray();
                    _result.Add($"[name: {this.Name} | id: {this.Id} | comm. type: {(CommTypes)devComm[0]}]", _devDataFrame);
                }
            }

            return _result;
        }

        private byte[] EncodeDeviceCommonInfo(EncodeUtils encodeUtils)
        {
            try
            {
                var _devIdCode = encodeUtils.ConvertByteToHex(this.Id);

                var _devVoltageCode = encodeUtils.ConvertByteToHex(this.Voltage);

                var _devCommTypeCode = encodeUtils.ConvertByteToHex((byte)this.DeviceCommType);

                byte[] _result = new byte[] { _devIdCode, _devVoltageCode, _devCommTypeCode };

                return _result;
            }
            catch
            {
                return null;
            }
        }

        private List<byte[]> EncodeDeviceComm(EncodeUtils encodeUtils)
        {
            try
            {
                List<byte[]> _result = new List<byte[]>();

                switch (this.DeviceCommType)
                {
                    case DeviceCommTypes.TCP:
                        {
                            if (this.Communications.Count > 0)
                            {
                                if (this.Communications[0].Type == CommTypes.TCP)
                                {
                                    var _protocolCode = encodeUtils.EncodeTCPProtocol(this.Communications[0]);
                                    _result.Add(_protocolCode);
                                }
                            }

                            break;
                        }
                    case DeviceCommTypes.UART:
                        {
                            if (this.Communications.Count > 0)
                            {
                                if (this.Communications[0].Type == CommTypes.UART)
                                {
                                    var _protocolCode = encodeUtils.EncodeUARTProtocol(this.Communications[0]);
                                    _result.Add(_protocolCode);
                                }
                            }

                            break;
                        }
                    case DeviceCommTypes.I2C:
                        {
                            if (this.Communications.Count > 0)
                            {
                                if (this.Communications[0].Type == CommTypes.I2C)
                                {
                                    var _protocolCode = encodeUtils.EncodeI2CProtocol(this.Communications[0]);
                                    _result.Add(_protocolCode);
                                }
                            }

                            break;
                        }
                    case DeviceCommTypes.CAN:
                        {
                            if (this.Communications.Count > 0)
                            {
                                if (this.Communications[0].Type == CommTypes.CAN)
                                {
                                    var _protocolCode = encodeUtils.EncodeCANProtocol(this.Communications[0]);
                                    _result.Add(_protocolCode);
                                }
                            }

                            break;
                        }
                    case DeviceCommTypes.TCP_UART:
                        {
                            if (this.Communications.Count > 0)
                            {
                                foreach (var devComm in this.Communications)
                                {
                                    if (devComm.Type == CommTypes.TCP)
                                    {
                                        var _protocol1Code = encodeUtils.EncodeTCPProtocol(devComm);
                                        _result.Add(_protocol1Code);
                                        continue;
                                    }

                                    if (devComm.Type == CommTypes.UART)
                                    {
                                        var _protocol2Code = encodeUtils.EncodeUARTProtocol(devComm);
                                        _result.Add(_protocol2Code);
                                        continue;
                                    }
                                }
                            }

                            break;
                        }
                    case DeviceCommTypes.TCP_I2C:
                        {
                            foreach (var devComm in this.Communications)
                            {
                                if (devComm.Type == CommTypes.TCP)
                                {
                                    var _protocol1Code = encodeUtils.EncodeTCPProtocol(devComm);
                                    _result.Add(_protocol1Code);
                                    continue;
                                }

                                if (devComm.Type == CommTypes.I2C)
                                {
                                    var _protocol2Code = encodeUtils.EncodeI2CProtocol(devComm);
                                    _result.Add(_protocol2Code);
                                    continue;
                                }
                            }

                            break;
                        }
                    case DeviceCommTypes.TCP_CAN:
                        {
                            foreach (var devComm in this.Communications)
                            {
                                if (devComm.Type == CommTypes.TCP)
                                {
                                    var _protocol1Code = encodeUtils.EncodeTCPProtocol(devComm);
                                    _result.Add(_protocol1Code);
                                    continue;
                                }

                                if (devComm.Type == CommTypes.CAN)
                                {
                                    var _protocol2Code = encodeUtils.EncodeCANProtocol(devComm);
                                    _result.Add(_protocol2Code);
                                    continue;
                                }
                            }

                            break;
                        }
                    case DeviceCommTypes.UART_CAN:
                        {
                            foreach (var devComm in this.Communications)
                            {
                                if (devComm.Type == CommTypes.UART)
                                {
                                    var _protocol1Code = encodeUtils.EncodeUARTProtocol(devComm);
                                    _result.Add(_protocol1Code);
                                    continue;
                                }

                                if (devComm.Type == CommTypes.CAN)
                                {
                                    var _protocol2Code = encodeUtils.EncodeCANProtocol(devComm);
                                    _result.Add(_protocol2Code);
                                    continue;
                                }
                            }

                            break;
                        }
                    case DeviceCommTypes.UART_I2C:
                        {
                            foreach (var devComm in this.Communications)
                            {
                                if (devComm.Type == CommTypes.UART)
                                {
                                    var _protocol1Code = encodeUtils.EncodeUARTProtocol(devComm);
                                    _result.Add(_protocol1Code);
                                    continue;
                                }

                                if (devComm.Type == CommTypes.I2C)
                                {
                                    var _protocol2Code = encodeUtils.EncodeI2CProtocol(devComm);
                                    _result.Add(_protocol2Code);
                                    continue;
                                }
                            }

                            break;
                        }
                    case DeviceCommTypes.I2C_CAN:
                        {
                            foreach (var devComm in this.Communications)
                            {
                                if (devComm.Type == CommTypes.I2C)
                                {
                                    var _protocol1Code = encodeUtils.EncodeI2CProtocol(devComm);
                                    _result.Add(_protocol1Code);
                                    continue;
                                }

                                if (devComm.Type == CommTypes.CAN)
                                {
                                    var _protocol2Code = encodeUtils.EncodeCANProtocol(devComm);
                                    _result.Add(_protocol2Code);
                                    continue;
                                }
                            }

                            break;
                        }
                }

                return _result;
            }
            catch
            {
                return null;
            }
        }
    }
}
