using System;
using System.ComponentModel;
using System.Collections.Generic;
using Newtonsoft.Json;
using Drone_Management_Tools.Organizer;

namespace Drone_Management_Tools.Models
{
    public class Protocol_TCP
    {
        [JsonProperty(PropertyName = "address")]
        [ReadOnly(true)]
        [DisplayName("Address")]
        [Description("TCP Protocol - Address")]
        public string Address { get; set; }

        [JsonProperty(PropertyName = "port")]
        [ReadOnly(true)]
        [DisplayName("Port")]
        [Description("TCP Protocol - Port")]
        public byte Port { get; set; }

        [JsonProperty(PropertyName = "serverClientMode")]
        [ReadOnly(true)]
        [DisplayName("Server/Client Mode")]
        [Description("TCP Protocol - Server/Client Mode")]
        public ServerClientModes ServerClientMode { get; set; }

        public override string ToString()
        {
            return "";
        }

        public Protocol_TCP()
        {
            this.Address = "127.0.0.1";
            this.Port = 123;
            this.ServerClientMode = ServerClientModes.Client;
        }

        //--- 6 bytes
        public byte[] Encode(EncodeUtils encodeUtils)
        {
            try
            {
                List<byte> _result = new List<byte>();

                string[] _address = this.Address.Split('.');
                if (_address.Length > 3)
                {
                    byte _num1 = Convert.ToByte(_address[0]);
                    var _num1Code = encodeUtils.ConvertByteToHex(_num1);
                    _result.Add(_num1Code);

                    byte _num2 = Convert.ToByte(_address[1]);
                    var _num2Code = encodeUtils.ConvertByteToHex(_num2);
                    _result.Add(_num2Code);

                    byte _num3 = Convert.ToByte(_address[2]);
                    var _num3Code = encodeUtils.ConvertByteToHex(_num3);
                    _result.Add(_num3Code);

                    byte _num4 = Convert.ToByte(_address[3]);
                    var _num4Code = encodeUtils.ConvertByteToHex(_num4);
                    _result.Add(_num4Code);
                }
                else
                    return null;                

                var _serverClientCode = encodeUtils.ConvertByteToHex((byte)this.ServerClientMode);
                _result.Add(_serverClientCode);

                var _portCode = encodeUtils.ConvertByteToHex(this.Port);
                _result.Add(_portCode);

                return _result.ToArray();
            }
            catch
            {
                return null;
            }
        }

        //--- 6 bytes
        public bool Decode(DecodeUtils decodeUtils, byte[] bytes)
        {
            if (bytes.Length >= 6)
            {
                try
                {
                    var _add1 = decodeUtils.ConvertHexToByte(bytes[0]);
                    var _add2 = decodeUtils.ConvertHexToByte(bytes[1]);
                    var _add3 = decodeUtils.ConvertHexToByte(bytes[2]);
                    var _add4 = decodeUtils.ConvertHexToByte(bytes[3]);

                    this.Address = $"{_add1}.{_add2}.{_add3}.{_add4}";
                    this.ServerClientMode = (ServerClientModes)decodeUtils.ConvertHexToByte(bytes[4]);
                    this.Port = decodeUtils.ConvertHexToByte(bytes[5]);                    

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
