using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drone_Management_Tools.Organizer;
using Newtonsoft.Json;

namespace Drone_Management_Tools.Models
{
    public class DronePwm
    {
        [JsonProperty(PropertyName = "pwm_1")]
        public Protocol_PWM DronePwm1 { get; set; }

        [JsonProperty(PropertyName = "pwm_2")]
        public Protocol_PWM DronePwm2 { get; set; }

        [JsonProperty(PropertyName = "pwm_3")]
        public Protocol_PWM DronePwm3 { get; set; }

        [JsonProperty(PropertyName = "pwm_4")]
        public Protocol_PWM DronePwm4 { get; set; }

        [JsonProperty(PropertyName = "pwm_5")]
        public Protocol_PWM DronePwm5 { get; set; }

        [JsonProperty(PropertyName = "pwm_6")]
        public Protocol_PWM DronePwm6 { get; set; }

        [JsonProperty(PropertyName = "pwm_7")]
        public Protocol_PWM DronePwm7 { get; set; }

        [JsonProperty(PropertyName = "pwm_8")]
        public Protocol_PWM DronePwm8 { get; set; }

        public DronePwm()
        {
            this.DronePwm1 = new Protocol_PWM();
            this.DronePwm2 = new Protocol_PWM();
            this.DronePwm3 = new Protocol_PWM();
            this.DronePwm4 = new Protocol_PWM();
            this.DronePwm5 = new Protocol_PWM();
            this.DronePwm6 = new Protocol_PWM();
            this.DronePwm7 = new Protocol_PWM();
            this.DronePwm8 = new Protocol_PWM();
        }

        public void CreateNull()
        {
            this.DronePwm1 = null;
            this.DronePwm2 = null;
            this.DronePwm3 = null;
            this.DronePwm4 = null;
            this.DronePwm5 = null;
            this.DronePwm6 = null;
            this.DronePwm7 = null;
            this.DronePwm8 = null;
        }

        public byte[] Encode(EncodeUtils encodeUtils)
        {
            try
            {
                List<byte> _result = new List<byte>();

                var _pwm1Code = this.DronePwm1.Encode(encodeUtils);
                _result.AddRange(_pwm1Code);

                var _pwm2Code = this.DronePwm2.Encode(encodeUtils);
                _result.AddRange(_pwm2Code);

                var _pwm3Code = this.DronePwm3.Encode(encodeUtils);
                _result.AddRange(_pwm3Code);

                var _pwm4Code = this.DronePwm4.Encode(encodeUtils);
                _result.AddRange(_pwm4Code);

                var _pwm5Code = this.DronePwm5.Encode(encodeUtils);
                _result.AddRange(_pwm5Code);

                var _pwm6Code = this.DronePwm6.Encode(encodeUtils);
                _result.AddRange(_pwm6Code);

                var _pwm7Code = this.DronePwm7.Encode(encodeUtils);
                _result.AddRange(_pwm7Code);

                var _pwm8Code = this.DronePwm8.Encode(encodeUtils);
                _result.AddRange(_pwm8Code);

                return _result.ToArray();
            }
            catch
            {
                return null;
            }
        }

        //--- 96 bytes
        public bool Decode(DecodeUtils decodeUtils, byte[] bytes)
        {
            if (bytes.Length >= 96)
            {
                try
                {
                    var _pwm1Bytes = decodeUtils.GetDataFrame(ref bytes, 12);
                    this.DronePwm1.Decode(decodeUtils, _pwm1Bytes);

                    var _pwm2Bytes = decodeUtils.GetDataFrame(ref bytes, 12);
                    this.DronePwm2.Decode(decodeUtils, _pwm2Bytes);

                    var _pwm3Bytes = decodeUtils.GetDataFrame(ref bytes, 12);
                    this.DronePwm3.Decode(decodeUtils, _pwm3Bytes);

                    var _pwm4Bytes = decodeUtils.GetDataFrame(ref bytes, 12);
                    this.DronePwm4.Decode(decodeUtils, _pwm4Bytes);

                    var _pwm5Bytes = decodeUtils.GetDataFrame(ref bytes, 12);
                    this.DronePwm5.Decode(decodeUtils, _pwm5Bytes);

                    var _pwm6Bytes = decodeUtils.GetDataFrame(ref bytes, 12);
                    this.DronePwm6.Decode(decodeUtils, _pwm6Bytes);

                    var _pwm7Bytes = decodeUtils.GetDataFrame(ref bytes, 12);
                    this.DronePwm7.Decode(decodeUtils, _pwm7Bytes);

                    this.DronePwm8.Decode(decodeUtils, bytes);

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
