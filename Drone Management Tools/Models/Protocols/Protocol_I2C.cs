using System.ComponentModel;
using System.Collections.Generic;
using Newtonsoft.Json;
using Drone_Management_Tools.Organizer;

namespace Drone_Management_Tools.Models
{
    public class Protocol_I2C
    {
        [JsonProperty(PropertyName = "address")]
        [ReadOnly(true)]
        [DisplayName("Address")]
        [Description("I2C Protocol - Address")]
        public byte Address { get; set; }

        [JsonProperty(PropertyName = "speed")]
        [ReadOnly(true)]
        [DisplayName("Speed")]
        [Description("I2C Protocol - Speed")]
        public byte Speed { get; set; }

        [JsonProperty(PropertyName = "masterSlaveMode")]
        [ReadOnly(true)]
        [DisplayName("Master/Slave Mode")]
        [Description("I2C Protocol - Master/Slave Mode")]
        public MasterSlaveModes MasterSlaveMode { get; set; }

        [JsonProperty(PropertyName = "dataSize")]
        [ReadOnly(true)]
        [DisplayName("Data Size")]
        [Description("I2C Protocol - Data Size")]
        public byte DataSize { get; set; }

        [JsonProperty(PropertyName = "ackMode")]
        [ReadOnly(true)]
        [DisplayName("ACK Mode")]
        [Description("I2C Protocol - ACK Mode")]
        public PermissionModes AckMode { get; set; }

        [JsonProperty(PropertyName = "startStopBits")]
        [ReadOnly(true)]
        [DisplayName("Start/Stop Bits")]
        [Description("I2C Protocol - Start/Stop Bits")]
        public byte StartStopBit { get; set; }

        [JsonProperty(PropertyName = "startConditionRepeat")]
        [ReadOnly(true)]
        [DisplayName("Start Condition Repeat")]
        [Description("I2C Protocol - Start Condition Repeat")]
        public PermissionModes StartConditionRepeat { get; set; }

        public override string ToString()
        {
            return "";
        }

        public Protocol_I2C()
        {
            this.Address = 0;
            this.Speed = 0;
            this.MasterSlaveMode = MasterSlaveModes.Slave;
            this.DataSize = 0;
            this.AckMode = PermissionModes.No;
            this.StartStopBit = 8;
            this.StartConditionRepeat = PermissionModes.No;
        }

        //--- 7 bytes
        public byte[] Encode(EncodeUtils encodeUtils)
        {
            try
            {
                List<byte> _result = new List<byte>();

                var _addressCode = encodeUtils.ConvertByteToHex(this.Address);
                _result.Add(_addressCode);

                var _speedCode = encodeUtils.ConvertByteToHex(this.Speed);
                _result.Add(_speedCode);

                var _masterSlaveCode = encodeUtils.ConvertByteToHex((byte)this.MasterSlaveMode);
                _result.Add(_masterSlaveCode);

                var _readWriteCode = encodeUtils.ConvertByteToHex(this.DataSize);
                _result.Add(_readWriteCode);

                var _ackModeCode = encodeUtils.ConvertByteToHex((byte)this.AckMode);
                _result.Add(_ackModeCode);

                var _startStopBitCode = encodeUtils.ConvertByteToHex(this.StartStopBit);
                _result.Add(_startStopBitCode);

                var _conditionRepeatCode = encodeUtils.ConvertByteToHex((byte)this.StartConditionRepeat);
                _result.Add(_conditionRepeatCode);

                return _result.ToArray();
            }
            catch
            {
                return null;
            }
        }

        //--- 2 bytes
        public byte[] EncodeDrone(EncodeUtils encodeUtils)
        {
            try
            {
                List<byte> _result = new List<byte>();

                var _addressCode = encodeUtils.ConvertByteToHex(this.Address);
                _result.Add(_addressCode);

                var _speedCode = encodeUtils.ConvertByteToHex(this.Speed);
                _result.Add(_speedCode);

                return _result.ToArray();
            }
            catch
            {
                return null;
            }
        }

        //--- 7 bytes
        public bool Decode(DecodeUtils decodeUtils, byte[] bytes)
        {
            if (bytes.Length >= 7)
            {
                try
                {
                    this.Address = decodeUtils.ConvertHexToByte(bytes[0]);
                    this.Speed = decodeUtils.ConvertHexToByte(bytes[1]);
                    this.MasterSlaveMode = (MasterSlaveModes)decodeUtils.ConvertHexToByte(bytes[2]);
                    this.DataSize = decodeUtils.ConvertHexToByte(bytes[3]);
                    this.AckMode = (PermissionModes)decodeUtils.ConvertHexToByte(bytes[4]);
                    this.StartStopBit = decodeUtils.ConvertHexToByte(bytes[5]);
                    this.StartConditionRepeat = (PermissionModes)decodeUtils.ConvertHexToByte(bytes[6]);

                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        //--- 2 bytes
        public bool DecodeDrone(DecodeUtils decodeUtils, byte[] bytes)
        {
            if (bytes.Length >= 2)
            {
                try
                {
                    this.Address = decodeUtils.ConvertHexToByte(bytes[0]);
                    this.Speed = decodeUtils.ConvertHexToByte(bytes[1]);

                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
