using System.ComponentModel;
using System.Collections.Generic;
using Newtonsoft.Json;
using Drone_Management_Tools.Organizer;

namespace Drone_Management_Tools.Models
{
    public class Protocol_PWM
    {
        [JsonProperty(PropertyName = "frequency")]
        [ReadOnly(true)]
        [DisplayName("Frequency")]
        [Description("PWM Protocol - Frequency")]
        public int Frequency { get; set; }

        [JsonProperty(PropertyName = "tmin")]
        [ReadOnly(true)]
        [DisplayName("Tmin")]
        [Description("PWM Protocol - Tmin")]
        public int Tmin { get; set; }

        [JsonProperty(PropertyName = "tmax")]
        [ReadOnly(true)]
        [DisplayName("Tmax")]
        [Description("PWM Protocol - Tmax")]
        public int Tmax { get; set; }

        public override string ToString()
        {
            return $"Freq: {Frequency} | Tmin: {Tmin} | Tmax: {Tmax}";
        }

        public Protocol_PWM()
        {
            this.Frequency = 0;
            this.Tmin = 0;
            this.Tmax = 0;
        }

        public byte[] Encode(EncodeUtils encodeUtils)
        {
            try
            {
                List<byte> _result = new List<byte>();

                var _freqCode = encodeUtils.ConvertIntToHex(this.Frequency);
                _result.AddRange(_freqCode);

                var _tminCode = encodeUtils.ConvertIntToHex(this.Tmin);
                _result.AddRange(_tminCode);

                var _tmaxCode = encodeUtils.ConvertIntToHex(this.Tmax);
                _result.AddRange(_tmaxCode);

                return _result.ToArray();
            }
            catch
            {
                return null;
            }
        }

        //--- 12 bytes
        public bool Decode(DecodeUtils decodeUtils, byte[] bytes)
        {
            if (bytes.Length >= 12)
            {
                try
                {
                    var _freqs = decodeUtils.GetDataFrame(ref bytes, 4);
                    this.Frequency = decodeUtils.ConvertHexToInt(_freqs);

                    var _tmins = decodeUtils.GetDataFrame(ref bytes, 4);
                    this.Tmin = decodeUtils.ConvertHexToInt(_tmins);

                    var _tmaxs = decodeUtils.GetDataFrame(ref bytes, 4);
                    this.Tmax = decodeUtils.ConvertHexToInt(_tmaxs);

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
