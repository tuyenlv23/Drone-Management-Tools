using System.ComponentModel;
using System.Collections.Generic;
using Newtonsoft.Json;
using Drone_Management_Tools.Organizer;

namespace Drone_Management_Tools.Models
{
    public class Protocol_UART
    {
        [JsonProperty(PropertyName = "baudrate")]
        [ReadOnly(true)]
        [DisplayName("Baudrate")]
        [Description("UART Protocol - Baudrate")]
        public uint Baudrate { get; set; }

        [JsonProperty(PropertyName = "dataBits")]
        [ReadOnly(true)]
        [DisplayName("Data Bits")]
        [Description("UART Protocol - Data Bits")]
        public byte DataBit { get; set; }

        [JsonProperty(PropertyName = "parity")]
        [ReadOnly(true)]
        [DisplayName("Parity")]
        [Description("UART Protocol - Parity")]
        public CommParity Parity { get; set; }

        [JsonProperty(PropertyName = "stopBits")]
        [ReadOnly(true)]
        [DisplayName("Stop Bits")]
        [Description("UART Protocol - Stop Bits")]
        public double StopBit { get; set; }

        public override string ToString()
        {
            return "";
        }

        public Protocol_UART()
        {
            this.Baudrate = CommDefaut.Baudrate;
            this.DataBit = CommDefaut.DataBit;
            this.Parity = CommDefaut.Parity;
            this.StopBit = CommDefaut.StopBit;
        }

        //--- 7 bytes
        public byte[] Encode(EncodeUtils encodeUtils)
        {
            try
            {
                List<byte> _result = new List<byte>();

                var _baudrateCode = encodeUtils.ConvertUintToHex(this.Baudrate);
                _result.AddRange(_baudrateCode);

                var _dataBitCode = encodeUtils.ConvertByteToHex(this.DataBit);
                _result.Add(_dataBitCode);

                var _parityCode = encodeUtils.ConvertByteToHex((byte)this.Parity);
                _result.Add(_parityCode);

                var _stopBitCode = encodeUtils.ConvertByteToHex(CommDefaut.ConvertStopBit(this.StopBit));
                _result.Add(_stopBitCode);

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
                    var _baudrates = decodeUtils.GetDataFrame(ref bytes, 4);
                    this.Baudrate = decodeUtils.ConvertHexToUint(_baudrates);
                    this.DataBit = decodeUtils.ConvertHexToByte(bytes[0]);
                    this.Parity = (CommParity)decodeUtils.ConvertHexToByte(bytes[1]);
                    this.StopBit = CommDefaut.ConvertStopBit(decodeUtils.ConvertHexToByte(bytes[2]));

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
