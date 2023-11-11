using System.ComponentModel;
using System.Collections.Generic;
using Newtonsoft.Json;
using Drone_Management_Tools.Organizer;

namespace Drone_Management_Tools.Models
{
    public class Protocol_CAN
    {
        [JsonProperty(PropertyName = "baudrate")]
        [ReadOnly(true)]
        [DisplayName("Baudrate")]
        [Description("CAN Protocol - Baudrate")]
        public uint Baudrate { get; set; }

        [JsonProperty(PropertyName = "readWriteMode")]
        [ReadOnly(true)]
        [DisplayName("Read/Write Mode")]
        [Description("CAN Protocol - Read/Write Mode")]
        public AccessModes ReadWriteMode { get; set; }

        public override string ToString()
        {
            return "";
        }
        
        public Protocol_CAN()
        {
            this.Baudrate = CommDefaut.Baudrate;
            this.ReadWriteMode = AccessModes.Read;
        }

        //--- 5 bytes
        public byte[] Encode(EncodeUtils encodeUtils)
        {
            try
            {
                List<byte> _result = new List<byte>();

                var _baudrateCode = encodeUtils.ConvertUintToHex(this.Baudrate);
                _result.AddRange(_baudrateCode);

                var _readWriteCode = encodeUtils.ConvertByteToHex((byte)this.ReadWriteMode);
                _result.Add(_readWriteCode);

                return _result.ToArray();
            }
            catch
            {
                return null;
            }
        }

        //--- 4 bytes
        public byte[] EncodeDrone(EncodeUtils encodeUtils)
        {
            try
            {
                List<byte> _result = new List<byte>();

                var _baudrateCode = encodeUtils.ConvertUintToHex(this.Baudrate);
                _result.AddRange(_baudrateCode);

                return _result.ToArray();
            }
            catch
            {
                return null;
            }
        }

        //--- 5 bytes
        public bool Decode(DecodeUtils decodeUtils, byte[] bytes)
        {
            if (bytes.Length >= 5)
            {
                try
                {
                    var _baudrates = decodeUtils.GetDataFrame(ref bytes, 4);
                    this.Baudrate = decodeUtils.ConvertHexToUint(_baudrates);
                    this.ReadWriteMode = (AccessModes)decodeUtils.ConvertHexToByte(bytes[0]);

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

        //--- 4 bytes
        public bool DecodeDrone(DecodeUtils decodeUtils, byte[] bytes)
        {
            if (bytes.Length >= 4)
            {
                try
                {
                    var _baudrates = decodeUtils.GetDataFrame(ref bytes, 4);
                    this.Baudrate = decodeUtils.ConvertHexToUint(_baudrates);

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
