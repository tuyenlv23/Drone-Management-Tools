using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Drone_Management_Tools.Organizer;

namespace Drone_Management_Tools.Models
{
    public class DroneModel
    {
        [JsonProperty(PropertyName = "protocol_I2C")]
        public DroneI2C DroneI2C { get; set; }

        [JsonProperty(PropertyName = "protocol_UART")]
        public DroneUART DroneUART { get; set; }

        [JsonProperty(PropertyName = "protocol_CAN")]
        public DroneCAN DroneCAN { get; set; }

        [JsonProperty(PropertyName = "protocol_TCP")]
        public DroneTCP DroneTCP { get; set; }

        [JsonProperty(PropertyName = "protocol_PWM")]
        public DronePwm DronePwm { get; set; }

        public DroneModel()
        {
            this.DroneI2C = new DroneI2C();
            this.DroneUART = new DroneUART();
            this.DroneCAN = new DroneCAN();
            this.DroneTCP = new DroneTCP();
            this.DronePwm = new DronePwm();
        }

        public void CreateNull()
        {
            this.DroneI2C.CreateNull();
            this.DroneUART.CreateNull();
            this.DroneUART.CreateNull();
            this.DroneCAN.CreateNull();
            this.DroneTCP.CreateNull();
            //this.DronePwm.CreateNull();
        }

        //--- 124 bytes
        public byte[] Encode(EncodeUtils encodeUtils)
        {
            try
            {
                List<byte> _result = new List<byte>();

                var _i2cCode = this.DroneI2C.Encode(encodeUtils);
                _result.AddRange(_i2cCode);

                var _uartCode = this.DroneUART.Encode(encodeUtils);
                _result.AddRange(_uartCode);

                var _canCode = this.DroneCAN.Encode(encodeUtils);
                _result.AddRange(_canCode);

                var _tcpCode = this.DroneTCP.Encode(encodeUtils);
                _result.AddRange(_tcpCode);

                var _pwmCode = this.DronePwm.Encode(encodeUtils);
                _result.AddRange(_pwmCode);

                return _result.ToArray();
            }
            catch
            {
                return null;
            }
        }

        //--- 124 bytes
        public bool Decode(DecodeUtils decodeUtils, ref byte[] bytes)
        {
            if (bytes.Length >= 124)
            {
                try
                {
                    var _i2cBytes = decodeUtils.GetDataFrame(ref bytes, 4);
                    this.DroneI2C.Decode(decodeUtils, _i2cBytes);

                    var _uartBytes = decodeUtils.GetDataFrame(ref bytes, 14);
                    this.DroneUART.Decode(decodeUtils, _uartBytes);

                    var _canBytes = decodeUtils.GetDataFrame(ref bytes, 4);
                    this.DroneCAN.Decode(decodeUtils, _canBytes);

                    var _tcpBytes = decodeUtils.GetDataFrame(ref bytes, 6);
                    this.DroneTCP.Decode(decodeUtils, _tcpBytes);

                    var _pwmBytes = decodeUtils.GetDataFrame(ref bytes, 96);
                    this.DronePwm.Decode(decodeUtils, _pwmBytes);

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
