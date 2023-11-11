using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Drone_Management_Tools.Organizer;

namespace Drone_Management_Tools.Models.Device
{
    public class DevicePwm
    {
        [JsonProperty(PropertyName = "pwm_1")]
        [ReadOnly(true)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [DisplayName("PWM-1")]
        [Description("PWM-1 Parameters")]
        public Protocol_PWM DevicePwm1 { get; set; }

        [JsonProperty(PropertyName = "pwm_2")]
        [ReadOnly(true)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [DisplayName("PWM-2")]
        [Description("PWM-2 Parameters")]
        public Protocol_PWM DevicePwm2 { get; set; }

        [JsonProperty(PropertyName = "pwm_3")]
        [ReadOnly(true)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [DisplayName("PWM-3")]
        [Description("PWM-3 Parameters")]
        public Protocol_PWM DevicePwm3 { get; set; }

        [JsonProperty(PropertyName = "pwm_4")]
        [ReadOnly(true)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [DisplayName("PWM-4")]
        [Description("PWM-4 Parameters")]
        public Protocol_PWM DevicePwm4 { get; set; }

        public DevicePwm()
        {
            this.DevicePwm1 = new Protocol_PWM();
            this.DevicePwm2 = new Protocol_PWM();
            this.DevicePwm3 = new Protocol_PWM();
            this.DevicePwm4 = new Protocol_PWM();
        }

        public override string ToString()
        {
            return $"Freq: {DevicePwm1.Frequency}, Tmin: {DevicePwm1.Tmin}, Tmax: {DevicePwm1.Tmax}";
        }

        public byte[] Encode(EncodeUtils encodeUtils)
        {
            try
            {
                List<byte> _result = new List<byte>();

                var _pwm1Code = this.DevicePwm1.Encode(encodeUtils);
                _result.AddRange(_pwm1Code);

                var _pwm2Code = this.DevicePwm2.Encode(encodeUtils);
                _result.AddRange(_pwm2Code);

                var _pwm3Code = this.DevicePwm3.Encode(encodeUtils);
                _result.AddRange(_pwm3Code);

                var _pwm4Code = this.DevicePwm4.Encode(encodeUtils);
                _result.AddRange(_pwm4Code);

                return _result.ToArray();
            }
            catch
            {
                return null;
            }
        }

        //--- 48 bytes
        public bool Decode(DecodeUtils decodeUtils, byte[] bytes)
        {
            if (bytes.Length == 48)
            {
                try
                {
                    var _pwm1Bytes = decodeUtils.GetDataFrame(ref bytes, 12);
                    this.DevicePwm1.Decode(decodeUtils, _pwm1Bytes);

                    var _pwm2Bytes = decodeUtils.GetDataFrame(ref bytes, 12);
                    this.DevicePwm2.Decode(decodeUtils, _pwm2Bytes);

                    var _pwm3Bytes = decodeUtils.GetDataFrame(ref bytes, 12);
                    this.DevicePwm3.Decode(decodeUtils, _pwm3Bytes);
                    
                    this.DevicePwm4.Decode(decodeUtils, bytes);

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
