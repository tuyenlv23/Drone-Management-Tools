using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drone_Management_Tools.Organizer;
using Newtonsoft.Json;

namespace Drone_Management_Tools.Models
{
    public class DroneUART
    {
        [JsonProperty(PropertyName = "uart_1")]
        public Protocol_UART DroneUART1 { get; set; }

        [JsonProperty(PropertyName = "uart_2")]
        public Protocol_UART DroneUART2 { get; set; }

        public DroneUART()
        {
            this.DroneUART1 = new Protocol_UART();
            this.DroneUART2 = new Protocol_UART();
        }

        public void CreateNull()
        {
            this.DroneUART1 = null;
            this.DroneUART2 = null;
        }

        public byte[] Encode(EncodeUtils encodeUtils)
        {
            try
            {
                List<byte> _result = new List<byte>();

                var _uart1Code = this.DroneUART1.Encode(encodeUtils);
                _result.AddRange(_uart1Code);

                var _uart2Code = this.DroneUART2.Encode(encodeUtils);
                _result.AddRange(_uart2Code);

                return _result.ToArray();
            }
            catch
            {
                return null;
            }
        }

        //--- 14 bytes
        public bool Decode(DecodeUtils decodeUtils, byte[] bytes)
        {
            if (bytes.Length >= 14)
            {
                try
                {
                    var _uart1Bytes = decodeUtils.GetDataFrame(ref bytes, 7);
                    this.DroneUART1.Decode(decodeUtils, _uart1Bytes);

                    this.DroneUART2.Decode(decodeUtils, bytes);

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
